namespace Rogue.Application.Domain.Entities.Enemies;

public sealed class SnakeMage : Enemy
{
    public SnakeMage()
    {
        Kind = EnemyKind.SnakeMage;
        Glyph = 's';
        Color = ConsoleColorKind.White;
        Hostility = 6;
    }

    public override void TakeTurn(IGameWorld world, Random rng)
    {
        if (IsChasing)
        {
            EnemyMovement.MoveTowardsPlayer(this, world, rng, stepCount: 1);
        }
        else
        {
            EnemyMovement.MoveDiagonalAlternating(this, world, rng);
        }
    }

    public static SnakeMage Create(int dungeonLevel)
    {
        var hp = 8 + dungeonLevel;
        return new SnakeMage
        {
            MaxHealth = hp,
            Health = hp,
            BaseAgility = 10 + dungeonLevel / 2,
            BaseStrength = 4 + dungeonLevel / 2
        };
    }
}
