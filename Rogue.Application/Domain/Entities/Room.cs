namespace Rogue.Application.Domain.Entities;

public sealed class Room
{
    public int Id { get; set; }
    public int X { get; set; }
    public int Y { get; set; }
    public int Width { get; set; }
    public int Height { get; set; }
    public int SectionRow { get; set; }
    public int SectionCol { get; set; }
    public bool IsStart { get; set; }
    public bool IsExit { get; set; }
    public bool Discovered { get; set; }

    public int Right => X + Width - 1;
    public int Bottom => Y + Height - 1;

    public Position Center => new(X + Width / 2, Y + Height / 2);

    public bool Contains(Position p) =>
        p.X >= X && p.X <= Right && p.Y >= Y && p.Y <= Bottom;

    public bool ContainsInterior(Position p) =>
        p.X > X && p.X < Right && p.Y > Y && p.Y < Bottom;

    public IEnumerable<Position> InteriorPositions()
    {
        for (var y = Y + 1; y < Bottom; y++)
        for (var x = X + 1; x < Right; x++)
            yield return new Position(x, y);
    }

    public Position GetWallExitPoint(Direction side, Random rng)
    {
        return side switch
        {
            Direction.North => new Position(rng.Next(X + 1, Right), Y),
            Direction.South => new Position(rng.Next(X + 1, Right), Bottom),
            Direction.West => new Position(X, rng.Next(Y + 1, Bottom)),
            Direction.East => new Position(Right, rng.Next(Y + 1, Bottom)),
            _ => Center
        };
    }
}
