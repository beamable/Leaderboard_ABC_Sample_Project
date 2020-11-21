using Beamable.Samples.ABC.Data;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using Beamable.Samples.ABC.Core;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Beamable.Samples.ABC.Views
{
   /// <summary>
   /// Handles the view concerns for the tree 
   /// </summary>
   [ExecuteAlways]
   public class TreeView : MonoBehaviour
   {
      //  Properties -----------------------------------
      private const float RoundedCubeDeltaY = 0.5f;

      public float GrowthPercentage 
      {  
         get 
         {
            return _growthPercentage; 
         }
         set
         {
            _growthPercentage = value;
            RenderGrowth();
         }
      }

      

      //  Fields ---------------------------------------
      [SerializeField]
      private Configuration _configuration = null;

      [SerializeField]
      private GameObject _growthOrigin = null;

      [SerializeField]
      private GameObject _target = null;

      [SerializeField]
      private PostProcessingView _postProcessingView = null;

      [SerializeField]
      private bool _isRotating = true;

      /// <summary>
      /// This list is created and sorted at edit time, and used
      /// at runtime to 'grow' the tree from bottom to top.
      /// </summary>
      [SerializeField]
      private List<GameObject> _roundedCubes;

      [Header ("Edit-Time Tools")]
      [Range (0, 1)]
      [SerializeField]
      private float _growthPercentage = 0;

      [SerializeField]
      private bool _willSort = false;

      //  Unity Methods   ------------------------------

      void OnEnable()
      {
         // This trickery is optional and for ease-of development.
         // Game makers can see the results of SOME changes during edit-time
         EditorApplication.update += UpdateAlways;
      }

      void OnDisable()
      {
         EditorApplication.update -= UpdateAlways;
      }

      protected void Update()
      {
         UpdateAlways();
      }

      //  Other Methods --------------------------------

      private void RenderGrowth()
      {
         if (_willSort)
         {
            _willSort = false;

            //Keep log
            Debug.Log("RenderGrowth() Sorted!");

            _roundedCubes = TreeView.SortByDistance(_roundedCubes, _growthOrigin.transform.position);
         }

         //Change effects during growth
         _postProcessingView.Intensity = _growthPercentage;

         //Toggle visibility during growth
         for (int i = 0; i < _roundedCubes.Count; i++)
         {
            GameObject roundedCube = _roundedCubes[i];
            float indexPercentage = (float)i / (float)_roundedCubes.Count;

            if (indexPercentage <= _growthPercentage)
            {
               SetRoundedCubeActive(i, true);
            }
            else
            {
               SetRoundedCubeActive(i, false);
            }
         }
      }

      /// <summary>
      /// Store original positions to help animation
      /// </summary>
      /// <param name="roundedCubes"></param>
      /// <returns></returns>
      private static Dictionary<int, Vector3> GetOriginalPositions(List<GameObject> roundedCubes)
      {
         Dictionary<int, Vector3> positionsByIndex = new Dictionary<int, Vector3>();

         for (int i = 0; i < roundedCubes.Count; i++)
         {
            GameObject nextGo = roundedCubes[i];
            positionsByIndex[i] = nextGo.transform.position;
         }

         return positionsByIndex;
      }

      public static List<GameObject> SortByDistance(List<GameObject> gos, Vector3 measureFrom)
      {
         return gos.OrderBy(x => Vector3.Distance(x.transform.position, measureFrom)).ToList();
      }

      private void SetRoundedCubeActive(int index, bool isActive)
      {
         GameObject currentGo = _roundedCubes[index];

         if (currentGo.activeInHierarchy != isActive)
         {

            if (isActive)
            {
               // Make visible
               currentGo.SetActive(true);

               // Move from DOWN to UP. Simulates "Growth"
               TweenHelper.TransformDOBlendableScaleBy(currentGo,
                  new Vector3(0, -RoundedCubeDeltaY, 0),
                  new Vector3(0, RoundedCubeDeltaY, 0),
                  _configuration.DelayFadeInRoundedCube);

               // Scale from 0 to 100%. Simulates "Growth"
               TweenHelper.TransformDoScale(currentGo, 
                  new Vector3(0, 0, 0), 
                  new Vector3(1, 1, 1),
                  _configuration.DelayFadeInRoundedCube);
               
            }
            else
            {
               currentGo.SetActive(false);
            }
         }
      }


      //  Event Handlers -------------------------------
      private void UpdateAlways()
      {
         if (_isRotating)
         {
            //Rotate faster as more is complete
            Vector3 rangeOfRotation = _configuration.TreeViewRotationMax - _configuration.TreeViewRotationMin;
            Vector3 nextRotation = rangeOfRotation * _growthPercentage + _configuration.TreeViewRotationMin;
            _target.transform.eulerAngles += nextRotation;

            RenderGrowth();
         }
      }
   }
}