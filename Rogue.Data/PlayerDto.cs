namespace Rogue.Data;

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
