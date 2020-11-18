using Beamable.Samples.ABC.Audio;
using Beamable.Samples.ABC.Data;
using Beamable.Samples.ABC.Views;
using Core.Platform.SDK.Leaderboard;
using DisruptorBeam;
using DisruptorBeam.Content;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

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

               SetupBeamableLeaderboard(de.LeaderboardService);

            });
         }
         catch (Exception e)
         {
            _isBeamableSDKInstalledErrorMessage = e.Message;
            ConnectivityService_OnConnectivityChanged(false);
         }
      }

      private async void SetupBeamableLeaderboard(LeaderboardService leaderboardService)
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
               await leaderboardService.SetScore(leaderboardContent.Id, UnityEngine.Random.Range(0, 25));
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


      private IEnumerator LoadScene(string sceneName)
      {
         _introUIView.MenuCanvasGroup.interactable = false;

         SoundManager.Instance.PlayAudioClip(SoundConstants.Click01);

         yield return new WaitForSeconds(_configuration.Delay5BeforePointsView);
         SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Single);
      }


      //  Event Handlers -------------------------------
      private void ConnectivityService_OnConnectivityChanged(bool isConnected)
      {
         _isConnected = isConnected;
         RenderUI();
      }


      private void IntroUIView_OnViewLeaderboardButtonClicked()
      {
         StartCoroutine(LoadScene(_configuration.LeaderboardSceneName));
      }


      private void IntroUIView_OnStartGameButtonClicked()
      {
         StartCoroutine(LoadScene(_configuration.GameSceneName));
      }
   }
}