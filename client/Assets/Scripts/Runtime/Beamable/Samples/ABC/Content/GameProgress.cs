using DisruptorBeam.Content;

[ContentType("game_progress")]
public class GameProgress : ContentObject
{
   public const string HighScoreKey = "HighScore";
   public const string CurrentScoreKey = "CurrentScore";
   public int HighScore = 0;
   public int CurrentScore = 0;
  
}

[System.Serializable]
public class GameProgressRef : ContentRef<GameProgress>
{
   public GameProgressRef() { }
}
