namespace Rogue.Data;

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
