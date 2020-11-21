﻿using Beamable.Samples.ABC.Data;
using UnityEngine;
using UnityEngine.UI;

namespace Beamable.Samples.ABC
{
   /// <summary>
   /// Handles the Leaderboard scene logic.
   /// </summary>
   public class LeaderboardSceneManager : MonoBehaviour
   {

      //  Fields ---------------------------------------
      [SerializeField]
      private Configuration _configuration = null;

      [SerializeField]
      private Button _closeButton = null;

      //  Unity Methods   ------------------------------
      protected void Start()
      {
         _closeButton.onClick.AddListener(CloseButton_OnClicked);
      }

      //  Other Methods --------------------------------

      //  Event Handlers -------------------------------
      private void CloseButton_OnClicked()
      {
         StartCoroutine(ABCHelper.LoadScene_Coroutine(_configuration.IntroSceneName,
            _configuration.DelayBeforeLoadScene));
      }
   }
}