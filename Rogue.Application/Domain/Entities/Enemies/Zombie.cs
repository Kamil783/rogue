namespace Rogue.Application.Domain.Entities.Enemies;

public sealed class Zombie : Enemy
{
    public Zombie()
    {
        Kind = EnemyKind.Zombie;
        Glyph = 'z';
        Color = ConsoleColorKind.Green;
        Hostility = 3;
    }

    public override void TakeTurn(IGameWorld world, Random rng)
    {
        EnemyMovement.Default(this, world, rng);
    }

    public static Zombie Create(int dungeonLevel)
    {
        var hp = 14 + dungeonLevel * 2;
        return new Zombie
        {
            MaxHealth = hp,
            Health = hp,
            BaseAgility = 3 + dungeonLevel / 4,
            BaseStrength = 5 + dungeonLevel / 2
        };
    }
}
