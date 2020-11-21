using UnityEngine;

namespace Beamable.Samples.ABC.Data
{
   /// <summary>
   /// Store the common configuration for easy editing at
   /// EditTime and RuntTime with the Unity Inspector Window.
   /// </summary>
   [CreateAssetMenu(
      fileName = Title,
      menuName = ContentConstants.MENU_ITEM_PATH_ASSETS_BEAMABLE_SAMPLES + "/" +
      "Microservices/Create New " + Title,
      order = ContentConstants.MENU_ITEM_PATH_ASSETS_BEAMABLE_ORDER_1)]
   public class Configuration : ScriptableObject
   {
      //  Constants  -----------------------------------
      private const string Title = "ABC Configuration";

      //  Properties -----------------------------------
      public string IntroSceneName { get { return _introSceneName; } }
      public string GameSceneName { get { return _gameSceneName; } }
      public string LeaderboardSceneName { get { return _leaderboardSceneName; } }
      public float DelayBeforeLoadScene { get { return _delayBeforeLoadScene; } }

      //
      public Vector3 TreeViewRotationMin { get { return _treeViewRotationMin; } }
      public Vector3 TreeViewRotationMax { get { return _treeViewRotationMax; } }
      public float MockRandomScoreMin { get { return _mockRandomScoreMin; } }
      public float MockRandomScoreMax { get { return _mockRandomScoreMax; } }
      public int LeaderboardMinRowCount { get { return _leaderboardMinRowCount; } }
      //
      public float DelayFadeInUI { get { return _delayFadeInUI; } }
      public float DelayFadeInRoundedCube { get { return _delayFadeInRoundedCube; } }
      public float GameDuration { get { return _gameDuration; } }
      public float PregameDuration { get { return _pregameDuration; } }
      public int HighScoreDefault { get { return _highScoreDefault; } }


      //  Fields ---------------------------------------

      [Header("Scenes")]
      [SerializeField]
      private string _introSceneName = "";

      [SerializeField]
      private string _gameSceneName = "";

      [SerializeField]
      private string _leaderboardSceneName = "";

      [Header("Cosmetic Delays")]
      [SerializeField]
      private float _delayBeforeLoadScene = 0;

      [Header("Cosmetic Animation")]
      [SerializeField]
      private Vector3 _treeViewRotationMin = new Vector3(0, 0.5f, 0);

      [SerializeField]
      private Vector3 _treeViewRotationMax = new Vector3(0, 1f, 0);

      [SerializeField]
      private float _delayFadeInUI = 0.25f;

      [SerializeField]
      private float _delayFadeInRoundedCube = 1f;

      [Header("Game Data")]
      [SerializeField]
      private float _gameDuration = 5;

      [SerializeField]
      private float _pregameDuration = 3.9f;

      [Header("Mock Data")]
      [SerializeField]
      private int _leaderboardMinRowCount = 10;

      [SerializeField]
      private int _highScoreDefault = 30;

      [SerializeField]
      private float _mockRandomScoreMin = 0;

      [SerializeField]
      private float _mockRandomScoreMax = 25;

   }
}