namespace Beamable.Samples.ABC
{
   /// <summary>
   /// Store commonly used static values
   /// </summary>
   public static class ABCConstants
   {
      //  Fields ---------------------------------------
      public const float RoundedCubeMoveDeltaY = 0.5f;
      public const int UnsetValue = -1;
      public enum GameState
      {
         PreGame,
         Game,
         PostGame
      }
   }
}