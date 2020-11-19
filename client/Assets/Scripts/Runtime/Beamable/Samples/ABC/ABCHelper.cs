using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
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

      public static IEnumerator LoadScene(string sceneName, float delayBeforeLoading)
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
               text += "<color=#9A9A9A>";
               text += $"Connected as '{dbid}'.";
               text += "</color>";
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

            text += "This demo game showcases Beamable's Multiplayer feature " +
               "which allows game makers to create real-time and turn-based multi-user game experiences.\n\n";

            text += ABCHelper.GetBulletList("Resources", new List<string> {
               "Beamable's <u><link=http://docs.beamable.com>Documentation</link></u>",
               "<link=https://docs.beamable.com/docs/microservices>Microservices</link></u>"
            }); ;


            return text;
         }
      }

      private static string InternetOfflineInstructionsText
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
         text += "<indent=5%>" + "\n";
         foreach (string item in items)
         {
            text += $"• {item}" + "\n";
         }
         text += "</indent>" + "\n";
         return text;
      }

      /// <summary>
      /// Fades opacity of a 3D object over time.
      /// </summary>
      public static void RenderersDoFade(List<Renderer> renderers, float fromAlpha, float toAlpha, float delay, float duration)
      {
         foreach (Renderer r in renderers)
         {
            foreach (Material m in r.materials)
            {
               m.DOFade(0, 3).SetDelay(delay);
            }
         }
      }

      /// <summary>
      /// Changes color of a 3D object temporarily. (E.g. Flicker red to indicate taking damage)
      /// </summary>
      public static void RenderersDoColorFlicker(List<Renderer> renderers, Color color, float duration)
      {
         foreach (Renderer r in renderers)
         {
            foreach (Material m in r.materials)
            {
               Color oldColor = m.color;
               m.DOColor(color, duration).OnComplete(() =>
               {
                  m.color = oldColor;
               });
            }
         }
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