namespace Rogue.Application.Domain.Entities.Items;

public sealed class Scroll : Item
{
    public Scroll()
    {
        Type = ItemType.Scroll;
        Glyph = '?';
    }

    public override void ApplyTo(Character character, Action<string> log)
    {
        if (MaxHealthBoost != 0)
        {
            character.MaxHealth += MaxHealthBoost;
            character.Health += MaxHealthBoost;
        }
        if (AgilityBoost != 0) character.BaseAgility += AgilityBoost;
        if (StrengthBoost != 0) character.BaseStrength += StrengthBoost;
        character.Stats.ScrollsRead++;
        log($"You read {Name}. You feel changed forever.");
    }
}
