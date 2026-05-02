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

public sealed class PlayerDto
{
    public string Name { get; set; } = "Hero";
    public int X { get; set; }
    public int Y { get; set; }
    public int Health { get; set; }
    public int MaxHealth { get; set; }
    public int BaseAgility { get; set; }
    public int BaseStrength { get; set; }
    public int Treasure { get; set; }
    public int SleepTurns { get; set; }
    public WeaponDto? EquippedWeapon { get; set; }
    public BackpackDto Backpack { get; set; } = new();
    public StatsDto Stats { get; set; } = new();
    public List<EffectDto> Effects { get; set; } = new();
}

public sealed class EffectDto
{
    public int RemainingTurns { get; set; }
    public int AgilityDelta { get; set; }
    public int StrengthDelta { get; set; }
    public int MaxHealthDelta { get; set; }
    public string SourceName { get; set; } = string.Empty;
}

public sealed class BackpackDto
{
    public List<WeaponDto> Weapons { get; set; } = new();
    public List<ItemDto> Foods { get; set; } = new();
    public List<ItemDto> Elixirs { get; set; } = new();
    public List<ItemDto> Scrolls { get; set; } = new();
    public int TotalTreasure { get; set; }
}

public sealed class WeaponDto : ItemDto
{
    public int Damage { get; set; }
}

public class ItemDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public int Type { get; set; }
    public int Subtype { get; set; }
    public int X { get; set; }
    public int Y { get; set; }
    public int HealthBoost { get; set; }
    public int MaxHealthBoost { get; set; }
    public int AgilityBoost { get; set; }
    public int StrengthBoost { get; set; }
    public int Cost { get; set; }
    public int DurationTurns { get; set; }
}

public sealed class LevelDto
{
    public int Index { get; set; }
    public int Width { get; set; }
    public int Height { get; set; }
    public int[] TileTypes { get; set; } = Array.Empty<int>();
    public bool[] Discovered { get; set; } = Array.Empty<bool>();
    public List<RoomDto> Rooms { get; set; } = new();
    public List<EnemyDto> Enemies { get; set; } = new();
    public List<ItemDto> Items { get; set; } = new();
    public int StartX { get; set; }
    public int StartY { get; set; }
    public int ExitX { get; set; }
    public int ExitY { get; set; }
}

public sealed class RoomDto
{
    public int Id { get; set; }
    public int X { get; set; }
    public int Y { get; set; }
    public int Width { get; set; }
    public int Height { get; set; }
    public int SectionRow { get; set; }
    public int SectionCol { get; set; }
    public bool IsStart { get; set; }
    public bool IsExit { get; set; }
    public bool Discovered { get; set; }
}

public sealed class EnemyDto
{
    public int Kind { get; set; }
    public int X { get; set; }
    public int Y { get; set; }
    public int Health { get; set; }
    public int MaxHealth { get; set; }
    public int BaseAgility { get; set; }
    public int BaseStrength { get; set; }
    public int Hostility { get; set; }
    public int RoomId { get; set; }
    public bool FirstHitTaken { get; set; }
    public bool IsChasing { get; set; }
    public bool IsInvisible { get; set; }
    public int RestTurns { get; set; }
    public int InternalState { get; set; }
}

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
