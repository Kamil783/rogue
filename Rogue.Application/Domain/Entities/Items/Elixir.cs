namespace Rogue.Application.Domain.Entities.Items;

public sealed class Elixir : Item
{
    public int DurationTurns { get; set; } = 20;

    public Elixir()
    {
        Type = ItemType.Elixir;
        Glyph = '!';
    }

    public override void ApplyTo(Character character, Action<string> log)
    {
        var effect = new TemporaryEffect
        {
            RemainingTurns = DurationTurns,
            AgilityDelta = AgilityBoost,
            StrengthDelta = StrengthBoost,
            MaxHealthDelta = MaxHealthBoost,
            SourceName = Name
        };
        character.AddEffect(effect);
        character.Stats.ElixirsDrunk++;
        log($"You drink {Name}. Effect lasts {DurationTurns} turns.");
    }
}
