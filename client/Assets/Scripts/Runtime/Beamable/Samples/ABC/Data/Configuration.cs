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
      public float TotalClicksMin { get { return _totalClicksMin; } }
      public float TotalClicksMax { get { return _totalClicksMax; } }
      //
      public float Delay1BeforeAttack { get { return _delay1BeforeAttack; } }
      public float Delay2BeforeBackswing { get { return _delay2BeforeBackswing; } }
      public float Delay3BeforeForeswing { get { return _delay3BeforeForeswing; } }
      public float Delay4BeforeTakeDamage { get { return _delay4BeforeTakeDamage; } }
      public float Delay5BeforePointsView { get { return _delay5BeforePointsView; } }


      //  Fields ---------------------------------------

      [Header("Scenes")]
      [SerializeField]
      private string _introSceneName = "";

      [SerializeField]
      private string _gameSceneName = "";

      [SerializeField]
      private string _leaderboardSceneName = "";

      [SerializeField]
      private float _delayBeforeLoadScene = 0;

      [Header("Cosmetic")]
      [SerializeField]
      private Vector3 _treeViewRotationMin = new Vector3(0, 0.5f, 0);

      [SerializeField]
      private Vector3 _treeViewRotationMax = new Vector3(0, 1f, 0);

      [SerializeField]
      private float _totalClicksMin = 0;

      [SerializeField]
      private float _totalClicksMax = 25;

      [Header("Attack")]
      [SerializeField]
      private float _delay1BeforeAttack = 0;

      [SerializeField]
      private float _delay2BeforeBackswing = 0;

      [SerializeField]
      private float _delay3BeforeForeswing = 0;

      [SerializeField]
      private float _delay4BeforeTakeDamage = 0;

      [SerializeField]
      private float _delay5BeforePointsView = 0;

   }
}