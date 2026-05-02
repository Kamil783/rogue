namespace Rogue.Application.Domain.Entities.Items;

public sealed class Treasure : Item
{
    public Treasure()
    {
        Type = ItemType.Treasure;
        Glyph = '$';
        Name = "Treasure";
    }

    public override void ApplyTo(Character character, Action<string> log)
    {
        character.Treasure += Cost;
        log($"You found {Cost} gold!");
    }
}
