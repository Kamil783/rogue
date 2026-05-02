namespace Rogue.Application.Domain.Entities;

public enum TileType
{
    Empty,
    Wall,
    Floor,
    Door,
    Corridor,
    Exit
}

public sealed class Tile
{
    public TileType Type { get; set; }
    public bool Discovered { get; set; }
    public bool Visible { get; set; }

    public Tile(TileType type)
    {
        Type = type;
    }

    public bool IsWalkable => Type is TileType.Floor or TileType.Door or TileType.Corridor or TileType.Exit;
    public bool IsTransparent => Type is TileType.Floor or TileType.Door or TileType.Corridor or TileType.Exit or TileType.Empty;
}
