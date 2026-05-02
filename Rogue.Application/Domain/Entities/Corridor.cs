namespace Rogue.Application.Domain.Entities;

public sealed class Corridor
{
    public int FromRoomId { get; init; }
    public int ToRoomId { get; init; }
    public List<Position> Path { get; init; } = new();
}
