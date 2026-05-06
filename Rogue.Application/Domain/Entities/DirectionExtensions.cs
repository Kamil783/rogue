namespace Rogue.Application.Domain.Entities;

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
