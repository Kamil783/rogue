using Rogue.Application.Domain.Entities.Enemies;
using Rogue.Application.Domain.Entities.Items;

namespace Rogue.Application.Domain.Entities;

public sealed class Level
{
    public int Index { get; set; }
    public int Width { get; set; }
    public int Height { get; set; }
    public Tile[,] Map { get; set; } = new Tile[0, 0];
    public List<Room> Rooms { get; set; } = new();
    public List<Corridor> Corridors { get; set; } = new();
    public List<Enemy> Enemies { get; set; } = new();
    public List<Item> Items { get; set; } = new();
    public Position StartPosition { get; set; }
    public Position ExitPosition { get; set; }

    public Tile GetTile(Position p)
    {
        if (p.X < 0 || p.X >= Width || p.Y < 0 || p.Y >= Height) return new Tile(TileType.Empty);
        return Map[p.X, p.Y];
    }

    public bool InBounds(Position p) => p.X >= 0 && p.X < Width && p.Y >= 0 && p.Y < Height;

    public Room? RoomContaining(Position p) => Rooms.FirstOrDefault(r => r.Contains(p));
}
