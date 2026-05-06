namespace Rogue.Data;

public sealed class LevelDto
{
    public int Index { get; set; }
    public int Width { get; set; }
    public int Height { get; set; }
    public int[] TileTypes { get; set; } = Array.Empty<int>();
    public bool[] Discovered { get; set; } = Array.Empty<bool>();
    public List<RoomDto> Rooms { get; set; } = new();
    public List<EnemyDto> Enemies { get; set; } = new();
    public List<ItemDto> Items { get; set; } = new();
    public int StartX { get; set; }
    public int StartY { get; set; }
    public int ExitX { get; set; }
    public int ExitY { get; set; }
}
