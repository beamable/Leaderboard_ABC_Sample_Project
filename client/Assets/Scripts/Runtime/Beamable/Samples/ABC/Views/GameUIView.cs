using Beamable.Samples.ABC.Audio;
using Beamable.Samples.ABC.Data;
using DG.Tweening;
using System.Collections;
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

      //  Unity Methods   ------------------------------
      protected void Start()
      {
         _backButton.onClick.AddListener(BackButton_OnClicked);
         _clickMeButton.onClick.AddListener(ClickMeButton_OnClicked);

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