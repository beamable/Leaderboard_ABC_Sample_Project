using Beamable.Common;
using Beamable.Samples.ABC.Data;
using Core.Platform.SDK.Leaderboard;
using DisruptorBeam;
using DisruptorBeam.Content;

namespace Beamable.Samples.ABC
{
   /// <summary>
   /// Create mock data. This is appropriate for a sample project, but not for
   /// production.
   /// </summary>
   public static class ABCMockDataCreator
   {
      //  Other Methods --------------------------------
      public static async void PopulateLeaderboardWithMockData(IDisruptorEngine disruptorEngine, 
         LeaderboardContent leaderboardContent, Configuration configuration)
      {
         LeaderboardService leaderboardService = disruptorEngine.LeaderboardService;

         LeaderBoardView leaderboardView = await leaderboardService.GetBoard(leaderboardContent.Id, 0, 100);

         Debug.Log($"SetupBeamableLeaderboard()1 c={leaderboardView.boardsize}");

         int targetItemCount = 10;
         if (leaderboardView.boardsize < targetItemCount)
         {
            int itemsToCreate = targetItemCount - (int)leaderboardView.boardsize;
            for (int i = 0; i < itemsToCreate; i++)
            {
               double mockScore = UnityEngine.Random.Range(configuration.TotalClicksMin, configuration.TotalClicksMax);
               mockScore = ABCHelper.GetRoundedScore(mockScore);

               await leaderboardService.SetScore(leaderboardContent.Id, mockScore);
            }
         }
         LeaderBoardView leaderboardView2 = await leaderboardService.GetBoard(leaderboardContent.Id, 0, 100);
         Debug.Log($"SetupBeamableLeaderboard()2 c={leaderboardView2.boardsize}");
      }
   }
}