using Beamable.Samples.ABC.Data;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using Beamable.Samples.Core.Utilities;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Beamable.Samples.ABC.Views
{
   /// <summary>
   /// Handles the audio/graphics rendering logic: Tree 
   /// </summary>
   [ExecuteAlways]
   public class TreeView : MonoBehaviour
   {
      //  Properties -----------------------------------
      
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

      [SerializeField]
      private bool _isRotatingInEditMode = true;

      //  Unity Methods   ------------------------------
      protected void OnEnable()
      {
#if UNITY_EDITOR
         // This trickery is optional and for ease-of development.
         // Game makers can see the results of SOME changes during edit-time
         EditorApplication.update += UpdateEditor;
#endif
      }

      protected void OnDisable()
      {
#if UNITY_EDITOR
         EditorApplication.update -= UpdateEditor;
#endif
      }

      protected void Start()
      {
         GrowthPercentage = 0;
      }


      protected void Update()
      {
         UpdateAlways();
      }


      //  Other Methods --------------------------------
      protected void UpdateEditor()
      {
         if (_isRotatingInEditMode)
         {
            UpdateAlways();
         }
      }


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
         if (_postProcessingView != null)
         {
            _postProcessingView.Intensity = _growthPercentage;
         }

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
               TweenHelper.TransformDOBlendableMoveBy(currentGo,
                  new Vector3(0, -ABCConstants.RoundedCubeMoveDeltaY, 0),
                  new Vector3(0, ABCConstants.RoundedCubeMoveDeltaY, 0),
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
         //Rotate faster as more is complete
         Vector3 rangeOfRotation = _configuration.TreeViewRotationMax - _configuration.TreeViewRotationMin;
         Vector3 nextRotation = rangeOfRotation * _growthPercentage + _configuration.TreeViewRotationMin;
         _target.transform.eulerAngles += nextRotation;

         RenderGrowth();
      }
   }
}