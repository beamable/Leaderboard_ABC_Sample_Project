using Beamable.Samples.ABC.Audio;
using Beamable.Samples.ABC.Data;
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Beamable.Samples.ABC.Views
{
   /// <summary>
   /// Handles the view concerns for the game scene UI elements.
   /// </summary>
   public class GameUIView : MonoBehaviour
   {
      //  Properties -----------------------------------
      public Button BackButton { get { return _backButton; } }
      public Button ClickMeButton { get { return _clickMeButton; } }
      public TMP_Text StatusText { get { return _statusText; } }

      //  Fields ---------------------------------------
      [SerializeField]
      private Configuration _configuration = null;

      [SerializeField]
      private TMP_Text _statusText = null;

      [SerializeField]
      private Button _backButton = null;

      [SerializeField]
      private Button _clickMeButton = null;

      [Header("Cosmetic Animation")]
      [SerializeField]
      private List<CanvasGroup> _canvasGroups = null;

      //  Unity Methods   ------------------------------
      protected void Start()
      {
         _statusText.text = "";

         _backButton.onClick.AddListener(BackButton_OnClicked);
         _clickMeButton.onClick.AddListener(ClickMeButton_OnClicked);

         ABCHelper.CanvasGroupsDoFade(_canvasGroups, 0, 1, 1, 0, _configuration.DelayFadeInUI);
      }

      //  Other Methods --------------------------------

      //  Event Handlers -------------------------------
      private void BackButton_OnClicked()
      {
      }


      private void ClickMeButton_OnClicked()
      {
      }
   }
}