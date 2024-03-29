﻿using Beamable.Samples.ABC.Data;
using Beamable.Samples.Core.Utilities;
using UnityEngine;
using UnityEngine.UI;

namespace Beamable.Samples.ABC
{
   /// <summary>
   /// Handles the main scene logic: Leaderboard
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


      //  Event Handlers -------------------------------
      private void CloseButton_OnClicked()
      {
         StartCoroutine(CoreHelper.LoadScene_Coroutine(_configuration.IntroSceneName,
            _configuration.DelayBeforeLoadScene));
      }
   }
}