namespace Rogue.Data;

public sealed class RoomDto
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
}
