namespace Rogue.Application.Domain.Entities;

public sealed class GameSession
{
    public const int TotalLevels = 21;

    public int Seed { get; set; }
    public int CurrentLevelIndex { get; set; } = 1;
    public Level CurrentLevel { get; set; } = null!;
    public Player Player { get; set; } = Player.CreateDefault();
    public List<string> MessageLog { get; set; } = new();
    public bool GameOver { get; set; }
    public bool Victory { get; set; }

    public void Log(string message)
    {
        MessageLog.Add(message);
        if (MessageLog.Count > 50) MessageLog.RemoveAt(0);
    }
}
