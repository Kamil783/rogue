using Rogue.Application.Domain.Entities.Items;

namespace Rogue.Application.Domain.Entities;

public sealed class Backpack
{
    public const int MaxPerType = 9;

    public List<Weapon> Weapons { get; init; } = new();
    public List<Food> Foods { get; init; } = new();
    public List<Elixir> Elixirs { get; init; } = new();
    public List<Scroll> Scrolls { get; init; } = new();
    public int TotalTreasure { get; set; }

    public bool TryAdd(Item item)
    {
        switch (item)
        {
            case Treasure t:
                TotalTreasure += t.Cost;
                return true;
            case Weapon w when Weapons.Count < MaxPerType:
                Weapons.Add(w); return true;
            case Food f when Foods.Count < MaxPerType:
                Foods.Add(f); return true;
            case Elixir e when Elixirs.Count < MaxPerType:
                Elixirs.Add(e); return true;
            case Scroll s when Scrolls.Count < MaxPerType:
                Scrolls.Add(s); return true;
            default:
                return false;
        }
    }

    public IReadOnlyList<Item> GetByType(ItemType type) => type switch
    {
        ItemType.Weapon => Weapons,
        ItemType.Food => Foods,
        ItemType.Elixir => Elixirs,
        ItemType.Scroll => Scrolls,
        _ => Array.Empty<Item>()
    };

    public void Remove(Item item)
    {
        switch (item)
        {
            case Weapon w: Weapons.Remove(w); break;
            case Food f: Foods.Remove(f); break;
            case Elixir e: Elixirs.Remove(e); break;
            case Scroll s: Scrolls.Remove(s); break;
        }
    }
}
