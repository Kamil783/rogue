namespace Rogue.Application.Domain.Entities.Items;

public enum ItemType
{
    Treasure,
    Food,
    Elixir,
    Scroll,
    Weapon
}

public enum ItemSubtype
{
    None,
    AgilityBoost,
    StrengthBoost,
    MaxHealthBoost,
    Apple,
    Bread,
    Meat,
    Sword,
    Dagger,
    Axe,
    Mace
}

public abstract class Item
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public ItemType Type { get; set; }
    public ItemSubtype Subtype { get; set; }
    public Position Position { get; set; }
    public int HealthBoost { get; set; }
    public int MaxHealthBoost { get; set; }
    public int AgilityBoost { get; set; }
    public int StrengthBoost { get; set; }
    public int Cost { get; set; }
    public char Glyph { get; set; }

    public abstract void ApplyTo(Character character, Action<string> log);
}
