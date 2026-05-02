namespace Rogue.Application.Domain.Entities.Enemies;

internal static class EnemyMovement
{
    public static void Default(Enemy enemy, IGameWorld world, Random rng)
    {
        if (enemy.IsChasing)
        {
            if (enemy.Position.ChebyshevDistance(world.Player.Position) <= 1)
            {
                world.TryAttackPlayer(enemy);
                return;
            }
            MoveTowardsPlayer(enemy, world, rng, 1);
        }
        else
        {
            MoveRandomInRoom(enemy, world, rng);
        }
    }

    public static void MoveRandomInRoom(Enemy enemy, IGameWorld world, Random rng)
    {
        var room = world.CurrentLevel.Rooms.FirstOrDefault(r => r.Id == enemy.RoomId);
        if (room == null) return;
        var candidates = enemy.Position.AllNeighbors()
            .Where(room.ContainsInterior)
            .Where(world.IsWalkable)
            .Where(p => world.EnemyAt(p) == null && world.Player.Position != p)
            .ToList();
        if (candidates.Count == 0) return;
        enemy.Position = candidates[rng.Next(candidates.Count)];
    }

    public static void MoveRandomTwo(Enemy enemy, IGameWorld world, Random rng)
    {
        for (var i = 0; i < 2; i++) MoveRandomInRoom(enemy, world, rng);
    }

    public static void MoveDiagonalAlternating(Enemy enemy, IGameWorld world, Random rng)
    {
        var room = world.CurrentLevel.Rooms.FirstOrDefault(r => r.Id == enemy.RoomId);
        if (room == null) return;
        var diagonals = new (int dx, int dy)[] { (1, 1), (-1, -1), (1, -1), (-1, 1) };
        var first = diagonals[enemy.InternalState % diagonals.Length];
        var dest = enemy.Position.Translate(first.dx, first.dy);
        if (room.ContainsInterior(dest) && world.IsWalkable(dest) && world.EnemyAt(dest) == null && world.Player.Position != dest)
        {
            enemy.Position = dest;
        }
        enemy.InternalState = (enemy.InternalState + 1) % diagonals.Length;
    }

    public static void MoveTowardsPlayer(Enemy enemy, IGameWorld world, Random rng, int stepCount)
    {
        for (var s = 0; s < stepCount; s++)
        {
            if (enemy.Position.ChebyshevDistance(world.Player.Position) <= 1)
            {
                world.TryAttackPlayer(enemy);
                return;
            }
            var path = Pathfinder.FindPath(enemy.Position, world.Player.Position, world);
            if (path == null || path.Count < 2)
            {
                MoveRandomInRoom(enemy, world, rng);
                return;
            }
            var next = path[1];
            if (world.IsWalkable(next) && world.EnemyAt(next) == null && world.Player.Position != next)
            {
                enemy.Position = next;
            }
            else if (next == world.Player.Position)
            {
                world.TryAttackPlayer(enemy);
                return;
            }
            else
            {
                return;
            }
        }
    }
}
