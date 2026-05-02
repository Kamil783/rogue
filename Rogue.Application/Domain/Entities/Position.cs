namespace Rogue.Application.Domain.Entities;

public readonly record struct Position(int X, int Y)
{
    public static Position Zero => new(0, 0);

    public int ManhattanDistance(Position other) => Math.Abs(X - other.X) + Math.Abs(Y - other.Y);

    public double EuclideanDistance(Position other)
    {
        var dx = X - other.X;
        var dy = Y - other.Y;
        return Math.Sqrt(dx * dx + dy * dy);
    }

    public int ChebyshevDistance(Position other) => Math.Max(Math.Abs(X - other.X), Math.Abs(Y - other.Y));

    public Position Translate(int dx, int dy) => new(X + dx, Y + dy);

    public IEnumerable<Position> CardinalNeighbors()
    {
        yield return new Position(X + 1, Y);
        yield return new Position(X - 1, Y);
        yield return new Position(X, Y + 1);
        yield return new Position(X, Y - 1);
    }

    public IEnumerable<Position> AllNeighbors()
    {
        for (var dy = -1; dy <= 1; dy++)
        for (var dx = -1; dx <= 1; dx++)
        {
            if (dx == 0 && dy == 0) continue;
            yield return new Position(X + dx, Y + dy);
        }
    }
}

public enum Direction
{
    North,
    South,
    East,
    West
}

public static class DirectionExtensions
{
    public static (int dx, int dy) ToOffset(this Direction direction) => direction switch
    {
        Direction.North => (0, -1),
        Direction.South => (0, 1),
        Direction.East => (1, 0),
        Direction.West => (-1, 0),
        _ => (0, 0)
    };
}
