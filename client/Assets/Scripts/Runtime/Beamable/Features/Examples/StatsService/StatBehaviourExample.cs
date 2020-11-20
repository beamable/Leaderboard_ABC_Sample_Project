﻿using DisruptorBeam.Stats;
using UnityEngine;

namespace Beamable.Features.Examples.Stats
{
   public class StatBehaviourExample : MonoBehaviour
   {
      //  Fields ---------------------------------------
      [SerializeField]
      private StatBehaviour _myStatBehaviour = null;

      //  Unity Methods   ------------------------------
      protected void Start()
      {
         //Async refresh value
         _myStatBehaviour.OnStatReceived.AddListener(MyStatBehaviour_OnStatReceived);

         // Set Value
         _myStatBehaviour.SetCurrentValue("0");
         _myStatBehaviour.SetCurrentValue("1");

         // Get Value
         Debug.Log("_statsBehaviour.value = " + _myStatBehaviour.Value);
      }

      //  Event Handlers -------------------------------
      private void MyStatBehaviour_OnStatReceived(string value)
      {
         // Observe Value Change
         Debug.Log("MyStatBehaviour_OnStatReceived() value = " + _myStatBehaviour.Value);
      }
   }
}