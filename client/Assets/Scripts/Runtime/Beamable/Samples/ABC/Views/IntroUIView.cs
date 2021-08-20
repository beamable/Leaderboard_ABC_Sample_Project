using Beamable.Samples.ABC.Data;
using System.Collections.Generic;
using Beamable.Samples.Core.Utilities;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Beamable.Samples.ABC.Views
{
   /// <summary>
   /// Handles the audio/graphics rendering logic: Intro UI
   /// </summary>
   public class IntroUIView : MonoBehaviour
   {
      //  Properties -----------------------------------
      public string AboutBodyText
      {
         set
         {
            _aboutBodyText.text = value;
         }
      }

      public CanvasGroup MenuCanvasGroup {  get { return _menuCanvasGroup; } }
      public Button ViewLeaderboardButton { get { return _viewLeaderboardButton; } }
      public Button StartGameButton { get { return _startGameButton; } }

      //  Fields ---------------------------------------
      [SerializeField]
      private Configuration _configuration = null;

      [SerializeField]
      private Button _viewLeaderboardButton = null;

      [SerializeField]
      private Button _startGameButton = null;

      [SerializeField]
      private TMP_Text _aboutBodyText = null;

      [SerializeField]
      private CanvasGroup _menuCanvasGroup = null;

      [Header ("Cosmetic Animation")]
      [SerializeField]
      private List<CanvasGroup> _canvasGroups = null;

      //  Unity Methods   ------------------------------
      protected void Start()
      {
         TweenHelper.CanvasGroupsDoFade(_canvasGroups, 0, 1, 1, 0, _configuration.DelayFadeInUI);
      }
   }
}