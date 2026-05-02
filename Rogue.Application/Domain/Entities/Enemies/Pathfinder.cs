namespace Rogue.Application.Domain.Entities.Enemies;

internal static class Pathfinder
{
    public static List<Position>? FindPath(Position from, Position to, IGameWorld world)
    {
        if (from == to) return new List<Position> { from };

        var open = new PriorityQueue<Position, int>();
        open.Enqueue(from, 0);
        var cameFrom = new Dictionary<Position, Position>();
        var costSoFar = new Dictionary<Position, int> { [from] = 0 };
        var maxSteps = 400;

        while (open.Count > 0 && maxSteps-- > 0)
        {
            var current = open.Dequeue();
            if (current == to) break;

            foreach (var next in current.AllNeighbors())
            {
                if (next != to && !world.IsWalkable(next)) continue;
                if (next != to && next != world.Player.Position && world.EnemyAt(next) != null) continue;

                var newCost = costSoFar[current] + 1;
                if (!costSoFar.TryGetValue(next, out var existing) || newCost < existing)
                {
                    costSoFar[next] = newCost;
                    var priority = newCost + next.ChebyshevDistance(to);
                    open.Enqueue(next, priority);
                    cameFrom[next] = current;
                }
            }
        }

        if (!cameFrom.ContainsKey(to)) return null;

        var path = new List<Position>();
        var step = to;
        while (step != from)
        {
            path.Add(step);
            step = cameFrom[step];
        }
        path.Add(from);
        path.Reverse();
        return path;
    }
}
