namespace Rogue.Application.Domain.Entities;

public sealed class Statistics
{
    public int TreasureCollected { get; set; }
    public int LevelReached { get; set; } = 1;
    public int EnemiesDefeated { get; set; }
    public int FoodEaten { get; set; }
    public int ElixirsDrunk { get; set; }
    public int ScrollsRead { get; set; }
    public int HitsLanded { get; set; }
    public int HitsTaken { get; set; }
    public int TilesWalked { get; set; }
    public DateTime Date { get; set; } = DateTime.Now;
    public bool Completed { get; set; }
}
