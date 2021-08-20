using System.Collections.Generic;
using UnityEngine;
using System;
using System.Collections;
using Beamable.Samples.Core.Audio;
using UnityEngine.SceneManagement;

namespace Beamable.Samples.Core.Utilities
{
   /// <summary>
   /// Store commonly reused functionality for any concern
   /// </summary>
   public static class CoreHelper
   {
      //  Other Methods --------------------------------
      public static IEnumerator LoadScene_Coroutine(string sceneName, float delayBeforeLoading)
      {
         SoundManager.Instance.PlayAudioClip(SoundConstants.Click01);

         yield return new WaitForSeconds(delayBeforeLoading);
         SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Single);
      }

      public static float GetAudioPitchByGrowthPercentage(float growthPercentage)
      {
         //From 0.5 to 1.5
         return 0.5f + Mathf.Clamp01(growthPercentage);
      }

      /// <summary>
      /// Convert the <see cref="float"/> to a <see cref="string"/>
      /// with rounding like "10.1";
      /// </summary>
      public static string GetRoundedTime(float value)
      {
         return string.Format("{0:0.0}", value);
      }

      /// <summary>
      /// Convert the <see cref="double"/> to a whole number like an <see cref="int"/>.
      /// </summary>
      public static double GetRoundedScore(double score)
      {
         return (int)score;
      }

      /// <summary>
      /// Convert the <see cref="string"/> to a whole number like an <see cref="int"/>.
      /// </summary>
      public static double GetRoundedScore(string score)
      {
         return GetRoundedScore(Double.Parse(score));
      }

      /// <summary>
      /// Return a random item from the list. 
      /// This provides cosmetic variety.
      /// </summary>
      public static string GetRandomString(List<string> items)
      {
         int index = UnityEngine.Random.Range(0, items.Count);
         return items[index];
      }
   }
}