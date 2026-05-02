namespace Rogue.Application.Domain.Entities;

public sealed class Player : Character
{
    public string Name { get; set; } = "Hero";

    public static Player CreateDefault()
    {
        return new Player
        {
            MaxHealth = 30,
            Health = 30,
            BaseAgility = 8,
            BaseStrength = 10,
            Position = Position.Zero
        };
    }
}
