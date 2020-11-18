using Beamable.Samples.ABC.Data;
using Beamable.Samples.ABC.Views;
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
      private TreeView _treeView = null;

      [SerializeField]
      private Configuration _configuration = null;

      //  Unity Methods   ------------------------------
      protected void Start ()
      {
      }

      //  Other Methods --------------------------------

      //  Event Handlers -------------------------------
      public void AttackButton_OnClicked ()
      {
      }
   }
}