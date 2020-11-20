using Beamable.Samples.ABC.Audio;
using Beamable.Samples.ABC.Data;
using Beamable.Samples.ABC.Views;
using DisruptorBeam.Stats;
using System;
using UnityEngine;

namespace Beamable.Samples.ABC
{
   /// <summary>
   /// The main entry point for in-game logic for the primary game scene.
   /// 
   /// </summary>
   public class ABCGameManager : MonoBehaviour
   {
      //  Fields ---------------------------------------
      [SerializeField]
      private Configuration _configuration = null;

      [SerializeField]
      private TreeView _treeView = null;

      [SerializeField]
      private GameUIView _gameUIView = null;

      [SerializeField]
      private StatBehaviour _currentScoreStatBehaviour = null;

      [SerializeField]
      private StatBehaviour _highScoreStatBehaviour = null;

      //  Unity Methods   ------------------------------
      protected void Start ()
      {
         _gameUIView.BackButton.onClick.AddListener(BackButton_OnClicked);
         _gameUIView.ClickMeButton.onClick.AddListener(ClickMeButton_OnClicked);

         //Sync refresh tree
         CurrentScoreStatBehaviour_OnStatReceived("0");

         //Async refresh value
         _currentScoreStatBehaviour.OnStatReceived.AddListener(CurrentScoreStatBehaviour_OnStatReceived);
         _currentScoreStatBehaviour.SetCurrentValue("0");
         

         Debug.Log("the global _highScoreStatBehaviour = " + _highScoreStatBehaviour.Value);
      }


      //  Other Methods --------------------------------

      //  Event Handlers -------------------------------
      private void ClickMeButton_OnClicked()
      {
      
         int value = Int32.Parse(_currentScoreStatBehaviour.Value) + 1;
         _currentScoreStatBehaviour.SetCurrentValue(value.ToString());
      }

      private void BackButton_OnClicked()
      {
         SoundManager.Instance.PlayAudioClip(SoundConstants.Click01);

         _currentScoreStatBehaviour.SetCurrentValue("0");
         ABCHelper.LoadScene_Coroutine(_configuration.IntroSceneName, 0);
      }

      private void CurrentScoreStatBehaviour_OnStatReceived(string score)
      {
         //TODO: Use stat here
         float highScore = 50;

         // Beating the high score will 'complete' the tree
         float growthPercentageOf100 = (100 * Int32.Parse(score)) / highScore;
         _treeView.GrowthPercentage = Mathf.Clamp01(growthPercentageOf100 / 100);
            
         float pitch = ABCHelper.GetAudioPitchByGrowthPercentage(_treeView.GrowthPercentage);
         SoundManager.Instance.PlayAudioClip(SoundConstants.Click02, pitch);

         _gameUIView.StatusText.text = $"3...2....1. You Clicked {score} times!";

         Debug.Log("_treeView.GrowthPercentage() : " + _treeView.GrowthPercentage);
      }
   }
}