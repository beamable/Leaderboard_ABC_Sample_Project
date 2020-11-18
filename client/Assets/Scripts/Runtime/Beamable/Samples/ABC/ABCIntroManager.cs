using Beamable.Samples.ABC.Views;
using DisruptorBeam;
using System;
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
      private IntroUI _introUI = null;

      private long _dbid = 0;
      private bool _isConnected = false;
      private bool _isBeamableSDKInstalled = false;
      private string _isBeamableSDKInstalledErrorMessage = "";

      //  Unity Methods   ------------------------------
      protected void Start()
      {
         _introUI.AboutBodyText = "";
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

            });
         }
         catch (Exception e)
         {
            _isBeamableSDKInstalledErrorMessage = e.Message;
            ConnectivityService_OnConnectivityChanged(false);
         }
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

         _introUI.AboutBodyText = aboutBodyText;
         _introUI.MenuCanvasGroup.interactable = _isConnected;
      }

      //  Event Handlers -------------------------------
      private void ConnectivityService_OnConnectivityChanged(bool isConnected)
      {
         _isConnected = isConnected;

         // Show some helpful debugging info
         //Debug.Log($"ConnectivityService_OnConnectivityChanged()...");
         //Debug.Log($"_dbid = {_dbid}. ");
         //Debug.Log($"_isConnected = {_isConnected}. ");
         //Debug.Log($"_isBeamableSDKInstalled = {_isBeamableSDKInstalled}. ");

         //if (!string.IsNullOrEmpty(_isBeamableSDKInstalledErrorMessage))
         //{
         //   Debug.Log($"_isBeamableSDKInstalledErrorMessage = {_isBeamableSDKInstalledErrorMessage}. ");
         //}

         RenderUI();
      }
   }
}