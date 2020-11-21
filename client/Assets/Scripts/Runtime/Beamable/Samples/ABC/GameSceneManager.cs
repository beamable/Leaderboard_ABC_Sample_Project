using Beamable.Samples.ABC.Audio;
using Beamable.Samples.ABC.Data;
using Beamable.Samples.ABC.Views;
using DisruptorBeam;
using DisruptorBeam.Content;
using DisruptorBeam.Stats;
using System;
using System.Collections;
using UnityEngine;
using static Beamable.Samples.ABC.ABCConstants;

namespace Beamable.Samples.ABC
{
   /// <summary>
   /// Handles the main scene logic: Game
   /// </summary>
   public class GameSceneManager : MonoBehaviour
   {
      //  Fields ---------------------------------------
      [SerializeField]
      private Configuration _configuration = null;

      [SerializeField]
      private TreeView _treeView = null;

      [SerializeField]
      private GameUIView _gameUIView = null;

      [SerializeField]
      private LeaderboardRef _leaderboardRef = null;

      [SerializeField]
      private StatBehaviour _currentScoreStatBehaviour = null;

      [SerializeField]
      private StatBehaviour _highScoreStatBehaviour = null;

      private bool _hasFalseStart = false;
      private Coroutine _runGameCoroutine;
      private float _gameTimeRemaining = 0;
      private LeaderboardContent _leaderboardContent;
      private IDisruptorEngine _disruptorEngine = null;
      private GameState _gameState = GameState.PreGame;

      /// <summary>
      /// Calculated each time the main menu opens.
      /// </summary>
      private int _lastGlobalHighScore = ABCConstants.UnsetValue;

      //  Unity Methods   ------------------------------
      protected void Start ()
      {
         _gameUIView.BackButton.onClick.AddListener(BackButton_OnClicked);
         _gameUIView.ClickMeButton.onClick.AddListener(ClickMeButton_OnClicked);

         //Setup Stat - High Score set to default, unless higher backend score exists
         //This is used for the cosmetics of the tree & audio
         _highScoreStatBehaviour.OnStatReceived.AddListener(HighScoreStatBehaviour_OnStatReceived);

         //Setup Stat - Score
         _currentScoreStatBehaviour.OnStatReceived.AddListener(CurrentScoreStatBehaviour_OnStatReceived);

         //Set the tree back to a beginning of growth
         _treeView.GrowthPercentage = 0;

         SetupBeamable();
      }


      //  Other Methods --------------------------------
      private async void SetupBeamable()
      {
         _leaderboardContent = await _leaderboardRef.Resolve();

         await DisruptorEngine.Instance.Then(de =>
         {
            try
            {
               _disruptorEngine = de;
               RestartGame();

            }
            catch (Exception)
            {
               _gameUIView.StatusText.text = ABCHelper.InternetOfflineInstructionsText;
            }
         });
      }


      private void RestartGame()
      {
         if (_runGameCoroutine != null)
         {
            StopCoroutine(_runGameCoroutine);
         }
         _runGameCoroutine = StartCoroutine(RunGame_Coroutine());
      }


      private IEnumerator RunGame_Coroutine()
      {
         //Initialize
         _gameState = GameState.PreGame;
         _hasFalseStart = false;
         _currentScoreStatBehaviour.SetCurrentValue("0");
         _gameUIView.ClickMeButton.interactable = true; //allow false starts :)

         //Countdown Pregame
         float pregameDuration = _configuration.PregameDuration;
         float pregameElapsed = 0;
         while (pregameElapsed <= pregameDuration && !_hasFalseStart)
         {
            pregameElapsed += Time.deltaTime;
            float pregameRemaining = pregameDuration - pregameElapsed;

            //Show as "3.2 seconds"
            _gameUIView.StatusText.text = $"Prepare to click!\nStarting in {ABCHelper.GetRoundedTime(pregameRemaining)}...";
            yield return new WaitForEndOfFrame();
         }

         float gameDuration = _configuration.GameDuration;
         if (!_hasFalseStart)
         {
            //Motivation
            _gameState = GameState.Game;
            _gameUIView.StatusText.text = $"Go!";
            SoundManager.Instance.PlayAudioClip(SoundConstants.Coin01);

            //Countdown Game
            float gameElapsed = 0;
            while (gameElapsed <= gameDuration && !_hasFalseStart)
            {
               gameElapsed += Time.deltaTime;
               _gameTimeRemaining = gameDuration - gameElapsed;

               //Show as "3.2 seconds"
               yield return new WaitForEndOfFrame();
            }
         }

         //Gameover state
         _gameState = GameState.PostGame;
         _gameUIView.ClickMeButton.interactable = false;
         if (_hasFalseStart)
         {
            SoundManager.Instance.PlayAudioClip(SoundConstants.GameOverLoss);
            _gameUIView.StatusText.text = $"<color=#91291B>False start!</color>\nWait before clicking.\nTry again.";
         }
         else
         {
            SoundManager.Instance.PlayAudioClip(SoundConstants.GameOverWin);
            _gameUIView.StatusText.text = $"{_currentScoreStatBehaviour.Value} clicks" +
               $" in {gameDuration} seconds!\nCheck the Leaderboard.";

            double finalScore = ABCHelper.GetRoundedScore(_currentScoreStatBehaviour.Value);
            _disruptorEngine.LeaderboardService.SetScore(_leaderboardContent.Id, finalScore);
         }
      }


      //  Event Handlers -------------------------------
      private void ClickMeButton_OnClicked()
      {
         switch (_gameState)
         {
            case GameState.PreGame:
               _hasFalseStart = true;
               break;
            case GameState.Game:
               int value = Int32.Parse(_currentScoreStatBehaviour.Value) + 1;
               _currentScoreStatBehaviour.SetCurrentValue(value.ToString());
               break;
            default:
               throw new Exception("Not possible");
         }
      }


      private void BackButton_OnClicked()
      {
         //TEMP - restart game
         //RestartGame();

         StartCoroutine(ABCHelper.LoadScene_Coroutine(_configuration.IntroSceneName,
            _configuration.DelayBeforeLoadScene));
      }


      private void CurrentScoreStatBehaviour_OnStatReceived(string value)
      {
         if (_lastGlobalHighScore == ABCConstants.UnsetValue)
         {
            return;
         }

         // Beating the high score will 'complete' the tree
         float growthPercentageOf100 = (100 * Int32.Parse(value)) / _lastGlobalHighScore;
         _treeView.GrowthPercentage = Mathf.Clamp01(growthPercentageOf100 / 100);

         // Play sound that increases in pitch as GrowthPercentage increases
         float pitch = ABCHelper.GetAudioPitchByGrowthPercentage(_treeView.GrowthPercentage);
         SoundManager.Instance.PlayAudioClip(SoundConstants.Click02, pitch);

         _gameUIView.StatusText.text = $"{_currentScoreStatBehaviour.Value} clicks.\n" +
            $"{ABCHelper.GetRoundedTime(_gameTimeRemaining)} secs left! Keep going!";

         //Debug.Log("_treeView.GrowthPercentage() : " + _treeView.GrowthPercentage);
      }


      private void HighScoreStatBehaviour_OnStatReceived(string value)
      {
         _lastGlobalHighScore = Mathf.Max(_configuration.HighScoreDefault, int.Parse(value));

         Debug.Log("HighScoreStatBehaviour_OnStatReceived(): " + _lastGlobalHighScore);
      }
   }
}