using Beamable.Samples.ABC.Data;
using Beamable.Samples.ABC.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using Beamable.Api.Leaderboard;
using Beamable.Api.Stats;
using Beamable.Common.Api.Leaderboards;
using Beamable.Common.Leaderboards;
using Beamable.Samples.Core.Data;
using Beamable.Samples.Core.Utilities;
using Beamable.Stats;
using UnityEngine;

namespace Beamable.Samples.ABC
{
   /// <summary>
   /// Handles the main scene logic: Intro
   /// </summary>
   public class IntroSceneManager : MonoBehaviour
   {
      //  Fields ---------------------------------------

      /// <summary>
      /// Determines if we are demo mode. Demo mode does several operations
      /// which are not recommended in a production project including 
      /// creating mock data for the game.
      /// </summary>
      private static bool IsDemoMode = true;

      [SerializeField]
      private IntroUIView _introUIView = null;

      [SerializeField]
      private Configuration _configuration = null;

      [SerializeField]
      private LeaderboardRef _leaderboardRef = null;

      [SerializeField]
      private StatBehaviour _highScoreStatBehaviour = null;

      private BeamContext _beamContext;
      private bool _isConnected = false;
      private bool _isBeamableSDKInstalled = false;
      private string _isBeamableSDKInstalledErrorMessage = "";

      //  Unity Methods   ------------------------------
      protected void Start()
      {
         _introUIView.AboutBodyText = "";
         _introUIView.StartGameButton.onClick.AddListener(StartGameButton_OnClicked);
         _introUIView.ViewLeaderboardButton.onClick.AddListener(ViewLeaderboardButton_OnClicked);
         SetupBeamable();
      }


      protected void OnDestroy()
      {
         _beamContext.Api.ConnectivityService.OnConnectivityChanged -= ConnectivityService_OnConnectivityChanged;
         _beamContext.ClearPlayerAndStop();
      }


      //  Other Methods --------------------------------

      /// <summary>
      /// Login with Beamable and fetch user/session information
      /// </summary>
      private async void SetupBeamable()
      {
         _beamContext = BeamContext.Default;
         await _beamContext.OnReady;
         // Attempt Connection to Beamable
         try
         {
            _isBeamableSDKInstalled = true;

            // Handle any changes to the internet connectivity
            _beamContext.Api.ConnectivityService.OnConnectivityChanged += ConnectivityService_OnConnectivityChanged;
            ConnectivityService_OnConnectivityChanged(_beamContext.Api.ConnectivityService.HasConnectivity);

            if (IsDemoMode)
            {
               //Set my player's name
               MockDataCreator.SetCurrentUserAlias(_beamContext.Api.StatsService, "This_is_you:)");

               //Populate the leaderboard with at least 10 mock users/scores
               PopulateLeaderboardWithMockData(_beamContext.Api.LeaderboardService);

               //Set the Beamable stat(s) to have initial values
               PopulateStats(_beamContext.Api.StatsService, _beamContext.Api.LeaderboardService);
            }
         }
         catch (Exception e)
         {
            // Failed to connect (e.g. not logged in)
            _isBeamableSDKInstalled = false;
            _isBeamableSDKInstalledErrorMessage = e.Message;
            ConnectivityService_OnConnectivityChanged(false);
         }
      }


      /// <summary>
      /// Set a highscore stat to the global high score. This is used to 
      /// calibrate the difficulty and rendering animations of the game.
      /// </summary>
      /// <param name="statsService"></param>
      /// <param name="leaderboardService"></param>
      private async void PopulateStats(StatsService statsService, LeaderboardService leaderboardService)
      {
         LeaderboardContent leaderboardContent = await _leaderboardRef.Resolve();
         LeaderBoardView leaderboardView = await leaderboardService.GetBoard(leaderboardContent.Id, 0, 100);
         List<RankEntry> rankEntries = leaderboardView.rankings;
         RankEntry highScoreRankEntry = rankEntries.FirstOrDefault();

         double highScore = 0;
         if (highScoreRankEntry != null)
         {
            highScore = highScoreRankEntry.score;
         }

         // Set stat values to start a fresh game
         highScore = CoreHelper.GetRoundedScore(highScore);
         _highScoreStatBehaviour.SetCurrentValue(highScore.ToString());
         await _highScoreStatBehaviour.Write(highScore.ToString());

         Debug.Log($"PopulateStats() _highScoreStatBehaviour={_highScoreStatBehaviour.Value}");
      }


      /// <summary>
      /// Add mock users with scores. This gives us as the user a sense of competition.
      /// </summary>
      /// <param name="leaderboardService"></param>
      private async void PopulateLeaderboardWithMockData(LeaderboardService leaderboardService)
      {
         LeaderboardContent leaderboardContent = await _leaderboardRef.Resolve();
         MockDataCreator.PopulateLeaderboardWithMockData(_beamContext.Api, leaderboardContent, _configuration);
      }


      /// <summary>
      /// Render the user-facing text with success or helpful errors.
      /// </summary>
      private void RenderUI()
      {
         long dbid = 0;
         if (_isConnected)
         {
            dbid = _beamContext.PlayerId;
         }

         string aboutBodyText = ABCHelper.GetIntroAboutBodyText(
            _isConnected, 
            dbid, 
            _isBeamableSDKInstalled, 
            _isBeamableSDKInstalledErrorMessage);

         _introUIView.AboutBodyText = aboutBodyText;
         _introUIView.MenuCanvasGroup.interactable = _isConnected;
      }


      //  Event Handlers -------------------------------
      private void ConnectivityService_OnConnectivityChanged(bool isConnected)
      {
         _isConnected = isConnected;
         RenderUI();
      }


      private void ViewLeaderboardButton_OnClicked()
      {
         _introUIView.MenuCanvasGroup.interactable = false;

         StartCoroutine(CoreHelper.LoadScene_Coroutine(_configuration.LeaderboardSceneName, 
            _configuration.DelayBeforeLoadScene));
      }


      private void StartGameButton_OnClicked()
      {
         _introUIView.MenuCanvasGroup.interactable = false;

         StartCoroutine(CoreHelper.LoadScene_Coroutine(_configuration.GameSceneName, 
            _configuration.DelayBeforeLoadScene));
      }
   }
}