﻿using System.Collections.Generic;
using UnityEngine;
using System;
using System.Collections;
using Beamable.Samples.ABC.Audio;
using UnityEngine.SceneManagement;

namespace Beamable.Samples.ABC
{
   /// <summary>
   /// Store commonly reused functionality for any concern
   /// </summary>
   public static class ABCHelper
   {
      //  Other Methods --------------------------------
      public static IEnumerator LoadScene_Coroutine(string sceneName, float delayBeforeLoading)
      {
         SoundManager.Instance.PlayAudioClip(SoundConstants.Click01);

         yield return new WaitForSeconds(delayBeforeLoading);
         SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Single);
      }


      public static string GetAttackMissedText()
      {
         return "Miss";
      }


      public static string GetAttackButtonText(int heroWeaponIndex)
      {
         string text = "Attack!";

         if (heroWeaponIndex >= 0)
         {
            text += $"<size=-19>\n(Weapon {++heroWeaponIndex:00})</size>";
         }
         return text;
      }

      /// <summary>
      /// Return the intro menu text. This serves as a welcome to the game plot and game instructions.
      /// If error, help text is shown.
      /// </summary>
      public static string GetIntroAboutBodyText(bool isConnected, long dbid,
         bool isBeamableSDKInstalled, string isBeamableSDKInstalledErrorMessage)
      {
         string text = "";

         // Is Beamable SDK Properly Installed In Unity?
         if (isBeamableSDKInstalled)
         {
            // Is Game Properly Connected To Internet?
            if (isConnected)
            {
               text += ABCHelper.GameInstructionsText;
               text += ABCHelper.GetBulletList("Status", new List<string>
               {
                  $"Connected: {true}",
                  $"DBID: {dbid}", 
               });
            }
            else
            {
               // Error
               text += ABCHelper.InternetOfflineInstructionsText;
            }
         }
         else
         {
            // Error
            text += $"_isBeamableSDKInstalled = {isBeamableSDKInstalled}." + "\n\n";
            text += $"_isBeamableSDKInstalledErrorMessage = {isBeamableSDKInstalledErrorMessage}" + "\n\n";
            text += ABCHelper.BeamableSDKInstallInstructionsText;
         }

         return text;
      }

      private static string GameInstructionsText
      {
         get
         {
            string text = "";

            text += "Button clicks grow trees. Click as many times as possible within the time limit." + "\n\n";

            text += "This sample project allows game makers to understand and apply the benefits of the " +
               "Leaderboard Flow in game development.\n\n";

            text += ABCHelper.GetBulletList("Resources", new List<string> 
            {
               "Overview: <u><link=https://docs.beamable.com/docs/leaderboard-flow-sample>Leaderboard Flow Sample</link></u>",
               "Feature: <u><link=https://docs.beamable.com/docs/leaderboard-flow>Leaderboard Flow</link></u>",
               "Feature: <u><link=https://docs.beamable.com/docs/stats>Stats</link></u>"
            });


            return text;
         }
      }

      public static float GetAudioPitchByGrowthPercentage(float growthPercentage)
      {
         //From 0.5 to 1.5
         return 0.5f + Mathf.Clamp01(growthPercentage);
      }

      public static string InternetOfflineInstructionsText
      {
         get
         {
            string text = "";
            text += "You are offline." + "\n\n";
            text += "<color=#ff0000>";
            text += ABCHelper.GetBulletList("Todo", new List<string> {
               "Connect to the internet.",
               "Close and Restart the game."
            }); ;
            text += "</color>";
            return text;
         }
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


      private static string BeamableSDKInstallInstructionsText
      {
         get
         {
            string text = "";
            text += "<color=#ff0000>";
            text += ABCHelper.GetBulletList("Todo", new List<string> {
               "Download & Install <u><link=http://docs.beamable.com>Beamable SDK</link></u>",
               "Open the Beamable Toolbox Window in Unity",
               "Register or Sign In"
            });
            text += "</color>";
            return text;
         }
      }


      private static string GetBulletList(string title, List<string> items)
      {
         string text = "";
         text += $"{title}" + "\n";
         text += "<indent=5%>";
         foreach (string item in items)
         {
            text += $"• {item}" + "\n";
         }
         text += "</indent>" + "\n";
         return text;
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