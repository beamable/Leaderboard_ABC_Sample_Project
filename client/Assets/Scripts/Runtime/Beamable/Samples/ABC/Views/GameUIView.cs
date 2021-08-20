using Beamable.Samples.ABC.Data;
using System.Collections.Generic;
using Beamable.Samples.Core.Utilities;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Beamable.Samples.ABC.Views
{
   /// <summary>
   /// Handles the audio/graphics rendering logic: Game
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
         TweenHelper.CanvasGroupsDoFade(_canvasGroups, 0, 1, 1, 0, _configuration.DelayFadeInUI);
      }
   }
}