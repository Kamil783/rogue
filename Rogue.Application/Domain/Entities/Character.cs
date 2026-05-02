using Rogue.Application.Domain.Entities.Items;

namespace Rogue.Application.Domain.Entities;

public abstract class Character
{
    public Position Position { get; set; }
    public int Health { get; set; }
    public int MaxHealth { get; set; }
    public int BaseAgility { get; set; }
    public int BaseStrength { get; set; }
    public bool Alive => Health > 0;

    public Weapon? EquippedWeapon { get; set; }
    public Weapon? PendingDropWeapon { get; set; }
    public Backpack Backpack { get; init; } = new();
    public Statistics Stats { get; init; } = new();
    public int Treasure { get; set; }
    public int SleepTurns { get; set; }

    public List<TemporaryEffect> Effects { get; init; } = new();

    public int Agility => BaseAgility + Effects.Sum(e => e.AgilityDelta);
    public int Strength => BaseStrength + Effects.Sum(e => e.StrengthDelta);
    public int EffectiveMaxHealth => MaxHealth + Effects.Sum(e => e.MaxHealthDelta);

    public void AddEffect(TemporaryEffect effect)
    {
        Effects.Add(effect);
        if (effect.MaxHealthDelta > 0)
        {
            Health += effect.MaxHealthDelta;
        }
    }

    public void TickEffects()
    {
        for (var i = Effects.Count - 1; i >= 0; i--)
        {
            var e = Effects[i];
            e.RemainingTurns--;
            if (e.RemainingTurns <= 0)
            {
                if (e.MaxHealthDelta > 0)
                {
                    Health -= e.MaxHealthDelta;
                    if (Health < 1) Health = 1;
                }
                Effects.RemoveAt(i);
            }
        }
    }
}
