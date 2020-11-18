﻿using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Beamable.Samples.ABC.Views
{
   /// <summary>
   /// Handles the view concerns for the post processing <see cref="Volume"/>. 
   /// </summary>
   [ExecuteAlways]
   public class PostProcessingView : MonoBehaviour
   {
      //  Properties -----------------------------------
      public float Intensity 
      {  
         get 
         {
            return _intensity; 
         }
         set
         {
            _intensity = value;
            Render();
         }
      }

      //  Fields ---------------------------------------
      [SerializeField]
      private Volume _volume = null;

      private float _intensity = 1;

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

      private void Render()
      {
         LensDistortion lensDistortion = null;
         _volume.profile.TryGet<LensDistortion>(out lensDistortion);

         if (lensDistortion != null)
         {
            lensDistortion.intensity.value = _intensity;
            Debug.Log("set to : " + lensDistortion.intensity.value);
         }

      }

      //  Event Handlers -------------------------------
      private void UpdateAlways()
      {
         Render();
      }
   }
}