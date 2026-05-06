namespace Rogue.Data;

public sealed class StatsDto
{
    public int TreasureCollected { get; set; }
    public int LevelReached { get; set; }
    public int EnemiesDefeated { get; set; }
    public int FoodEaten { get; set; }
    public int ElixirsDrunk { get; set; }
    public int ScrollsRead { get; set; }
    public int HitsLanded { get; set; }
    public int HitsTaken { get; set; }
    public int TilesWalked { get; set; }
    public DateTime Date { get; set; }
    public bool Completed { get; set; }
}
