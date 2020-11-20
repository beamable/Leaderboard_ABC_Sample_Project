using DisruptorBeam;
using System.Collections.Generic;
using UnityEngine;

namespace Beamable.Features.Examples.Stats
{
   public class StatCodingExample : MonoBehaviour
   {
      //  Unity Methods   ------------------------------
      protected void Start()
      {
         DisruptorEngine.Instance.Then(de =>
         {
            UseStatCoding(de);
         });
      }

      //  Other Methods   ------------------------------
      private async void UseStatCoding(IDisruptorEngine de)
      {
         string statKey = "MyExampleStat";
         string access = "public";
         string domain = "client";
         string type = "player";
         long id = de.User.id;

         // Set Value
         Dictionary<string, string> setStats =
            new Dictionary<string, string>() { { statKey, "99" } };

         await de.Stats.SetStats(access, setStats);

         // Get Value
         Dictionary<string, string> getStats = 
            await de.Stats.GetStats(domain, access, type, id);

         string myExampleStatValue = "";
         getStats.TryGetValue(statKey, out myExampleStatValue);
         Debug.Log("myExampleStatValue: " + myExampleStatValue);
      }
   }
}