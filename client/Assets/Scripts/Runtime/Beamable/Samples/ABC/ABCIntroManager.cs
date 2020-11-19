using Beamable.Samples.ABC.Audio;
using Beamable.Samples.ABC.Data;
using Beamable.Samples.ABC.Views;
using Core.Platform.SDK;
using Core.Platform.SDK.Leaderboard;
using Core.Platform.SDK.Stats;
using DisruptorBeam;
using DisruptorBeam.Content;
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
      private GameProgressRef _gameProgressRef = null;

      private long _dbid = 0;
      private bool _isConnected = false;
      private bool _isBeamableSDKInstalled = false;
      private string _isBeamableSDKInstalledErrorMessage = "";

      //  Unity Methods   ------------------------------
      protected void Start()
      {
         _introUIView.AboutBodyText = "";
         _introUIView.OnStartGameButtonClicked.AddListener(IntroUIView_OnStartGameButtonClicked);
         _introUIView.OnViewLeaderboardButtonClicked.AddListener(IntroUIView_OnViewLeaderboardButtonClicked);
         SetupBeamable();
      }


      protected void OnDestroy()
      {
         DisruptorEngine.Instance.Then(de =>
         {
            de.ConnectivityService.OnConnectivityChanged -= ConnectivityService_OnConnectivityChanged;
         });
      }


      //  Other Methods --------------------------------

      /// <summary>
      /// Login with Beamable and fetch user/session information
      /// </summary>
      private void SetupBeamable()
      {
         try
         {
            // Attempt Connection to Beamable
            DisruptorEngine.Instance.Then(de =>
            {
               // Fetch user information
               _dbid = de.User.id;
               _isBeamableSDKInstalled = true;
               // Handle any changes to the internet connectivity
               de.ConnectivityService.OnConnectivityChanged += ConnectivityService_OnConnectivityChanged;
               ConnectivityService_OnConnectivityChanged(de.ConnectivityService.HasConnectivity);

               PopulateLeaderboardWithMockData(de.LeaderboardService);
               PopulateStats(de.Stats, de.LeaderboardService);

            });
         }
         catch (Exception e)
         {
            _isBeamableSDKInstalledErrorMessage = e.Message;
            ConnectivityService_OnConnectivityChanged(false);
         }
      }

      private async void PopulateStats(StatsService statsService, LeaderboardService leaderboardService)
      {
         LeaderboardContent leaderboardContent = await _leaderboardRef.Resolve();
         LeaderBoardView leaderboardView = await leaderboardService.GetBoard(leaderboardContent.Id, 0, 100);

         Debug.Log($"SetupBeamableLeaderboard()1 c={leaderboardView.boardsize}");

         List<RankEntry> rankEntries = leaderboardView.rankings;
         RankEntry highScoreRankEntry = rankEntries.FirstOrDefault();
         double highScore = 0;
         //1
         if (highScoreRankEntry != null)
         {
            highScore = highScoreRankEntry.score;
            Debug.Log($"highScoreRankEntry()1 r={highScoreRankEntry.rank} score={highScore}");
         }

         //TODO: LEARN TO SET THE STATS, GET THE STATS
         GameProgress gameProgress = await _gameProgressRef.Resolve();
         await statsService.SetStats(gameProgress.Id, new Dictionary<string, string> ()
         {
            { GameProgress.HighScoreKey, highScore.ToString() },
            { GameProgress.CurrentScoreKey, "0" },
         });
      }

      private async void PopulateLeaderboardWithMockData(LeaderboardService leaderboardService)
      {
         LeaderboardContent leaderboardContent = await _leaderboardRef.Resolve();
         LeaderBoardView leaderboardView = await leaderboardService.GetBoard(leaderboardContent.Id, 0, 100);

         Debug.Log($"SetupBeamableLeaderboard()1 c={leaderboardView.boardsize}");

         int targetItemCount = 10;
         if (leaderboardView.boardsize < targetItemCount)
         {
            int itemsToCreate = targetItemCount - (int)leaderboardView.boardsize;
            for (int i = 0; i < itemsToCreate; i++)
            {
               await leaderboardService.SetScore(leaderboardContent.Id, 
                  UnityEngine.Random.Range(_configuration.TotalClicksMin, _configuration.TotalClicksMax));
            }
         }
         LeaderBoardView leaderboardView2 = await leaderboardService.GetBoard(leaderboardContent.Id, 0, 100);
         Debug.Log($"SetupBeamableLeaderboard()2 c={leaderboardView2.boardsize}");
      }

      /// <summary>
      /// Render the user-facing text with success or helpful errors.
      /// </summary>
      private void RenderUI()
      {
         string aboutBodyText = ABCHelper.GetIntroAboutBodyText(
            _isConnected, 
            _dbid, 
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


      private void IntroUIView_OnViewLeaderboardButtonClicked()
      {
         _introUIView.MenuCanvasGroup.interactable = false;

         StartCoroutine(ABCHelper.LoadScene(_configuration.LeaderboardSceneName, 
            _configuration.DelayBeforeLoadScene));
      }


      private void IntroUIView_OnStartGameButtonClicked()
      {
         _introUIView.MenuCanvasGroup.interactable = false;

         StartCoroutine(ABCHelper.LoadScene(_configuration.GameSceneName, 
            _configuration.DelayBeforeLoadScene));
      }
   }
}