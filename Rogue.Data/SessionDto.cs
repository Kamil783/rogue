namespace Rogue.Data;

public sealed class SessionDto
{
    public int Seed { get; set; }
    public int CurrentLevelIndex { get; set; }
    public PlayerDto Player { get; set; } = new();
    public LevelDto CurrentLevel { get; set; } = new();
    public List<string> MessageLog { get; set; } = new();
    public bool GameOver { get; set; }
    public bool Victory { get; set; }
}
