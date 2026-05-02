using Rogue.Application.Domain.Entities.Items;

namespace Rogue.Application.Domain.Generation;

public static class ItemFactory
{
    private static int _nextId = 1;

    public static Item CreateRandomBeneficial(Random rng, int dungeonLevel)
    {
        var roll = rng.Next(100);
        return roll switch
        {
            < 35 => CreateFood(rng),
            < 60 => CreateElixir(rng, dungeonLevel),
            < 80 => CreateScroll(rng, dungeonLevel),
            _ => CreateWeapon(rng, dungeonLevel)
        };
    }

    public static Food CreateFood(Random rng)
    {
        var roll = rng.Next(100);
        return roll switch
        {
            < 50 => new Food { Id = _nextId++, Name = "apple", Subtype = ItemSubtype.Apple, HealthBoost = 5 },
            < 80 => new Food { Id = _nextId++, Name = "bread", Subtype = ItemSubtype.Bread, HealthBoost = 10 },
            _ => new Food { Id = _nextId++, Name = "meat", Subtype = ItemSubtype.Meat, HealthBoost = 18 }
        };
    }

    public static Elixir CreateElixir(Random rng, int dungeonLevel)
    {
        var stat = rng.Next(3);
        var amount = 2 + rng.Next(1, 3 + dungeonLevel / 4);
        return stat switch
        {
            0 => new Elixir { Id = _nextId++, Name = "potion of agility", Subtype = ItemSubtype.AgilityBoost, AgilityBoost = amount, DurationTurns = 25 },
            1 => new Elixir { Id = _nextId++, Name = "potion of strength", Subtype = ItemSubtype.StrengthBoost, StrengthBoost = amount, DurationTurns = 25 },
            _ => new Elixir { Id = _nextId++, Name = "potion of vitality", Subtype = ItemSubtype.MaxHealthBoost, MaxHealthBoost = amount * 2, DurationTurns = 25 }
        };
    }

    public static Scroll CreateScroll(Random rng, int dungeonLevel)
    {
        var stat = rng.Next(3);
        var amount = 1 + rng.Next(1, 2 + dungeonLevel / 5);
        return stat switch
        {
            0 => new Scroll { Id = _nextId++, Name = "scroll of agility", Subtype = ItemSubtype.AgilityBoost, AgilityBoost = amount },
            1 => new Scroll { Id = _nextId++, Name = "scroll of strength", Subtype = ItemSubtype.StrengthBoost, StrengthBoost = amount },
            _ => new Scroll { Id = _nextId++, Name = "scroll of vigor", Subtype = ItemSubtype.MaxHealthBoost, MaxHealthBoost = amount * 2 }
        };
    }

    public static Weapon CreateWeapon(Random rng, int dungeonLevel)
    {
        var roll = rng.Next(4);
        var bonus = dungeonLevel / 2;
        return roll switch
        {
            0 => new Weapon { Id = _nextId++, Name = "dagger", Subtype = ItemSubtype.Dagger, StrengthBoost = 2, Damage = 3 + bonus },
            1 => new Weapon { Id = _nextId++, Name = "sword", Subtype = ItemSubtype.Sword, StrengthBoost = 3, Damage = 5 + bonus },
            2 => new Weapon { Id = _nextId++, Name = "axe", Subtype = ItemSubtype.Axe, StrengthBoost = 4, Damage = 7 + bonus },
            _ => new Weapon { Id = _nextId++, Name = "mace", Subtype = ItemSubtype.Mace, StrengthBoost = 5, Damage = 9 + bonus }
        };
    }

    public static Treasure CreateTreasure(Random rng, int dungeonLevel, int hostility, int strength, int agility, int hp)
    {
        var value = (hostility + strength + agility + hp / 4) * (1 + dungeonLevel / 3);
        return new Treasure { Id = _nextId++, Cost = value, Name = "gold", Subtype = ItemSubtype.None };
    }
}
