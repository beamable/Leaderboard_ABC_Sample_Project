using Beamable.Samples.ABC.Audio;
using Beamable.Samples.ABC.Data;
using DG.Tweening;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Beamable.Samples.ABC.Views
{
   /// <summary>
   /// Handles the view concerns for the intro scene UI elements.
   /// </summary>
   public class IntroUIView : MonoBehaviour
   {
      //  Properties -----------------------------------
      public string AboutBodyText
      {
         set
         {
            _aboutBodyText.text = value;
         }
      }

      public CanvasGroup MenuCanvasGroup {  get { return _menuCanvasGroup; } }

      [HideInInspector]
      public UnityEvent OnViewLeaderboardButtonClicked = new UnityEvent();

      [HideInInspector]
      public UnityEvent OnStartGameButtonClicked = new UnityEvent();

      //  Fields ---------------------------------------
      [SerializeField]
      private Configuration _configuration = null;

      [SerializeField]
      private Button _viewLeaderboardButton = null;

      [SerializeField]
      private Button _startGameButton = null;

      [SerializeField]
      private CanvasGroup _logoCanvasGroup = null;

      [SerializeField]
      private CanvasGroup _aboutCanvasGroup = null;

      [SerializeField]
      private TMP_Text _aboutBodyText = null;

      [SerializeField]
      private CanvasGroup _menuCanvasGroup = null;
      

      //  Unity Methods   ------------------------------
      protected void Start()
      {
         _viewLeaderboardButton.onClick.AddListener(ViewLeaderboardButton_OnClicked);
         _startGameButton.onClick.AddListener(StartGameButton_OnClicked);

         //
         _logoCanvasGroup.DOFade(0, 0);
         _logoCanvasGroup.DOFade(1, 1);

         //
         _aboutCanvasGroup.DOFade(0, 0);
         _aboutCanvasGroup.DOFade(1, 1).SetDelay(0.25f);

         //
         _menuCanvasGroup.DOFade(0, 0);
         _menuCanvasGroup.DOFade(1, 1).SetDelay(0.50f);
      }


      //  Other Methods --------------------------------
      private IEnumerator LoadScene(string sceneName)
      {
         _viewLeaderboardButton.interactable = false;
         _startGameButton.interactable = false;

         SoundManager.Instance.PlayAudioClip(SoundConstants.Click01);

         yield return new WaitForSeconds(_configuration.Delay5BeforePointsView);
         SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Single);
      }


      //  Event Handlers -------------------------------
      private void ViewLeaderboardButton_OnClicked()
      {
         OnViewLeaderboardButtonClicked.Invoke();
      }


      private void StartGameButton_OnClicked()
      {
         OnStartGameButtonClicked.Invoke();
      }
   }
}