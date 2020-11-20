using Beamable.Samples.ABC.Data;
using UnityEngine;
using System.Collections.Generic;
using DG.Tweening;
using System.Linq;
using System;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
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

      //Keep as serialized. Created at edit time, used at runtime
      [HideInInspector]
      [SerializeField]
      private Dictionary<int, Vector3> _roundedCubeOriginalPositions = new Dictionary<int, Vector3>();

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

      
      protected void Start()
      {
         _roundedCubeOriginalPositions = TreeView.GetOriginalPositions(_roundedCubes);
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
            _roundedCubeOriginalPositions = TreeView.GetOriginalPositions(_roundedCubes);
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
               SetActiveSafe(roundedCube, i, true);
            }
            else
            {
               SetActiveSafe(roundedCube, i, false);
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

      private void SetActiveSafe(GameObject go, int index, bool isActive)
      {
         if (go.activeInHierarchy != isActive)
         {

            if (isActive)
            {
               go.SetActive(true);

               go.transform.DOBlendableScaleBy(new Vector3(0, -RoundedCubeDeltaY, 0), 0);
               go.transform.DOBlendableScaleBy(new Vector3(0, RoundedCubeDeltaY, 0), 1).
                  SetDelay(0.01f).
                  SetEase(Ease.OutBack);

               go.transform.DOScale(new Vector3(0, 0, 0), 0);
               go.transform.DOScale(new Vector3(1, 1, 1), 1);
            }
            else
            {
               go.SetActive(false);
               go.transform.DOScale(new Vector3(1, 1, 1), 0);
               go.transform.DOScale(new Vector3(0, 0, 0), 0.25f).
                  SetDelay(0.01f).
                  OnComplete(() =>
                  {
                     go.SetActive(false);
                  });
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