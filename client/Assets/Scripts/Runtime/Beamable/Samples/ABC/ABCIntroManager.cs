using Beamable.Samples.ABC.Audio;
using Beamable.Samples.ABC.Data;
using Beamable.Samples.ABC.Views;
using Core.Platform.SDK.Leaderboard;
using Core.Platform.SDK.Stats;
using DisruptorBeam;
using DisruptorBeam.Content;
using DisruptorBeam.Stats;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Beamable.Samples.ABC
{
   /// <summary>
   /// Handles the intro menu scene logic.
   /// </summary>
   public class ABCIntroManager : MonoBehaviour
   {

      //  Fields ---------------------------------------
      [SerializeField]
      private IntroUIView _introUIView = null;

      [SerializeField]
      private Configuration _configuration = null;

      [SerializeField]
      private LeaderboardRef _leaderboardRef = null;

      [SerializeField]
      private StatBehaviour _highScoreStatBehaviour = null;

      private IDisruptorEngine _disruptorEngine = null;
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
         DisruptorEngine.Instance.Then(de =>
         {
            _disruptorEngine = null;
            de.ConnectivityService.OnConnectivityChanged -= ConnectivityService_OnConnectivityChanged;
         });
      }


      //  Other Methods --------------------------------

      /// <summary>
      /// Login with Beamable and fetch user/session information
      /// </summary>
      private void SetupBeamable()
      {

         // Attempt Connection to Beamable
         DisruptorEngine.Instance.Then(de =>
         {
            try
            {
               _disruptorEngine = de;
               _isBeamableSDKInstalled = true;

               // Handle any changes to the internet connectivity
               _disruptorEngine.ConnectivityService.OnConnectivityChanged += ConnectivityService_OnConnectivityChanged;
               ConnectivityService_OnConnectivityChanged(_disruptorEngine.ConnectivityService.HasConnectivity);

               PopulateLeaderboardWithMockData(_disruptorEngine.LeaderboardService);
               PopulateStats(_disruptorEngine.Stats, _disruptorEngine.LeaderboardService);

            }
            catch (Exception e)
            {
               // Failed to connect (e.g. not logged in)
               _isBeamableSDKInstalled = false;
               _isBeamableSDKInstalledErrorMessage = e.Message;
               ConnectivityService_OnConnectivityChanged(false);
            }
         });
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
            Debug.Log($"PopulateStats() highScore is rank={highScoreRankEntry.rank} value={highScore}");
         }

         // Set stat values to start a fresh game
         highScore = ABCHelper.GetRoundedScore(highScore);
         _highScoreStatBehaviour.SetCurrentValue(highScore.ToString());
      }


      private async void PopulateLeaderboardWithMockData(LeaderboardService leaderboardService)
      {
         LeaderboardContent leaderboardContent = await _leaderboardRef.Resolve();
         ABCMockDataCreator.PopulateLeaderboardWithMockData(_disruptorEngine, leaderboardContent, _configuration);
      }


      /// <summary>
      /// Render the user-facing text with success or helpful errors.
      /// </summary>
      private void RenderUI()
      {
         long dbid = 0;
         if (_isConnected)
         {
            dbid = _disruptorEngine.User.id;
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

         StartCoroutine(ABCHelper.LoadScene_Coroutine(_configuration.LeaderboardSceneName, 
            _configuration.DelayBeforeLoadScene));
      }


      private void StartGameButton_OnClicked()
      {
         _introUIView.MenuCanvasGroup.interactable = false;

         StartCoroutine(ABCHelper.LoadScene_Coroutine(_configuration.GameSceneName, 
            _configuration.DelayBeforeLoadScene));
      }
   }
}