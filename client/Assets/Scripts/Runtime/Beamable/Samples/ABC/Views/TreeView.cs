using Beamable.Samples.ABC.Data;
using UnityEngine;
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

      //  Fields ---------------------------------------
      [SerializeField]
      private Configuration _configuration = null;

      [SerializeField]
      private GameObject _target = null;

      [SerializeField]
      private bool _isRotating = true;

      //  Unity Methods   ------------------------------
      protected void Start()
      {
      }

      void OnEnable()
      {
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

      //  Event Handlers -------------------------------
      private void UpdateAlways()
      {
         if (_isRotating)
         {
            _target.transform.eulerAngles += new Vector3(0, 0.05f, 0);
         }
      }
   }
}