using Beamable.Samples.ABC.Data;
using UnityEngine;
using System.Collections.Generic;
using System;
using System.Linq;
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
               SetActiveSafe(roundedCube, true);
            }
            else
            {
               SetActiveSafe(roundedCube, false);
            }
         }
      }

      public static List<GameObject> SortByDistance(List<GameObject> objects, Vector3 measureFrom)
      {
         return objects.OrderBy(x => Vector3.Distance(x.transform.position, measureFrom)).ToList();
      }

      private void SetActiveSafe(GameObject go, bool isActive)
      {
         if (go.activeInHierarchy != isActive)
         {
            go.SetActive(isActive);
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