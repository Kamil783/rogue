namespace Rogue.Application.Domain.Entities.Enemies;

public sealed class Ghost : Enemy
{
    public Ghost()
    {
        Kind = EnemyKind.Ghost;
        Glyph = 'g';
        Color = ConsoleColorKind.White;
        Hostility = 2;
    }

    public override void TakeTurn(IGameWorld world, Random rng)
    {
        if (!IsChasing)
        {
            if (rng.Next(100) < 25) IsInvisible = !IsInvisible;
            if (rng.Next(100) < 35)
            {
                var room = world.CurrentLevel.Rooms.FirstOrDefault(r => r.Id == RoomId);
                if (room != null)
                {
                    var positions = room.InteriorPositions().ToList();
                    var dest = positions[rng.Next(positions.Count)];
                    if (world.IsWalkable(dest) && world.EnemyAt(dest) == null && world.Player.Position != dest)
                    {
                        Position = dest;
                        return;
                    }
                }
            }
        }
        else
        {
            IsInvisible = false;
        }
        EnemyMovement.Default(this, world, rng);
    }

    public static Ghost Create(int dungeonLevel)
    {
        var hp = 5 + dungeonLevel;
        return new Ghost
        {
            MaxHealth = hp,
            Health = hp,
            BaseAgility = 7 + dungeonLevel / 3,
            BaseStrength = 3 + dungeonLevel / 3
        };
    }
}
