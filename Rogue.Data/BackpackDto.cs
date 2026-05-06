namespace Rogue.Data;

public sealed class BackpackDto
{
    public List<WeaponDto> Weapons { get; set; } = new();
    public List<ItemDto> Foods { get; set; } = new();
    public List<ItemDto> Elixirs { get; set; } = new();
    public List<ItemDto> Scrolls { get; set; } = new();
    public int TotalTreasure { get; set; }
}
