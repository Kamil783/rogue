namespace Rogue.Application.Domain.Entities.Enemies;

public sealed class Ogre : Enemy
{
    public Ogre()
    {
        Kind = EnemyKind.Ogre;
        Glyph = 'O';
        Color = ConsoleColorKind.Yellow;
        Hostility = 4;
    }

    public override void TakeTurn(IGameWorld world, Random rng)
    {
        if (RestTurns > 0)
        {
            RestTurns--;
            if (IsAdjacent(world.Player.Position))
            {
                world.TryAttackPlayer(this);
            }
            return;
        }

        if (IsChasing)
        {
            if (IsAdjacent(world.Player.Position))
            {
                if (world.TryAttackPlayer(this))
                {
                    RestTurns = 1;
                }
                return;
            }
            EnemyMovement.MoveTowardsPlayer(this, world, rng, stepCount: 2);
        }
        else
        {
            EnemyMovement.MoveRandomTwo(this, world, rng);
        }
    }

    private bool IsAdjacent(Position other) => Position.ChebyshevDistance(other) <= 1;

    public static Ogre Create(int dungeonLevel)
    {
        var hp = 25 + dungeonLevel * 3;
        return new Ogre
        {
            MaxHealth = hp,
            Health = hp,
            BaseAgility = 2 + dungeonLevel / 5,
            BaseStrength = 10 + dungeonLevel
        };
    }
}
