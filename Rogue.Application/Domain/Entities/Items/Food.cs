namespace Rogue.Application.Domain.Entities.Items;

public sealed class Food : Item
{
    public Food()
    {
        Type = ItemType.Food;
        Glyph = '%';
    }

    public override void ApplyTo(Character character, Action<string> log)
    {
        var restored = Math.Min(HealthBoost, character.MaxHealth - character.Health);
        character.Health += restored;
        if (character.Health > character.MaxHealth) character.Health = character.MaxHealth;
        character.Stats.FoodEaten++;
        log($"You ate {Name} and restored {restored} HP.");
    }
}
