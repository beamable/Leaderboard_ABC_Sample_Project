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
      public string GameCASceneName { get { return _gameCASceneName; } }
      public string GameSASceneName { get { return _gameSASceneName; } }
      public float DelayBeforeLoadScene { get { return _delayBeforeLoadScene; } }

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
      private string _gameCASceneName = "";

      [SerializeField]
      private string _gameSASceneName = "";

      [SerializeField]
      private float _delayBeforeLoadScene = 0;


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