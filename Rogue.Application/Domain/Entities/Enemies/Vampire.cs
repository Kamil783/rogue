namespace Rogue.Application.Domain.Entities.Enemies;

public sealed class Vampire : Enemy
{
    public Vampire()
    {
        Kind = EnemyKind.Vampire;
        Glyph = 'v';
        Color = ConsoleColorKind.Red;
        Hostility = 7;
    }

    public override void TakeTurn(IGameWorld world, Random rng)
    {
        EnemyMovement.Default(this, world, rng);
    }

    public static Vampire Create(int dungeonLevel)
    {
        var hp = 10 + dungeonLevel * 2;
        return new Vampire
        {
            MaxHealth = hp,
            Health = hp,
            BaseAgility = 8 + dungeonLevel / 3,
            BaseStrength = 4 + dungeonLevel / 2
        };
    }
}
