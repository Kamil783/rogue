namespace Rogue.Application.Domain.Entities.Items;

public sealed class Weapon : Item
{
    public int Damage { get; set; }

    public Weapon()
    {
        Type = ItemType.Weapon;
        Glyph = ')';
    }

    public override void ApplyTo(Character character, Action<string> log)
    {
        var previous = character.EquippedWeapon;
        character.EquippedWeapon = this;
        if (previous != null)
        {
            character.PendingDropWeapon = previous;
            log($"You equip {Name} and drop {previous.Name}.");
        }
        else
        {
            log($"You equip {Name}.");
        }
    }
}
