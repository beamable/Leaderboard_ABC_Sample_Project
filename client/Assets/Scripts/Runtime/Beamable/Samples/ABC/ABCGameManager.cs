using Beamable.Samples.ABC.Data;
using DG.Tweening;
using System.Collections;
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

      private int _bossHealth = 0;


      //  Unity Methods   ------------------------------
      protected void Start ()
      {
         //
         _bossHealth = 100;
      }

      //  Other Methods --------------------------------

      //  Event Handlers -------------------------------
      public void AttackButton_OnClicked ()
      {
      }
   }
}