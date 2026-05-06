namespace Rogue.Data;

public sealed class EnemyDto
{
    public int Kind { get; set; }
    public int X { get; set; }
    public int Y { get; set; }
    public int Health { get; set; }
    public int MaxHealth { get; set; }
    public int BaseAgility { get; set; }
    public int BaseStrength { get; set; }
    public int Hostility { get; set; }
    public int RoomId { get; set; }
    public bool FirstHitTaken { get; set; }
    public bool IsChasing { get; set; }
    public bool IsInvisible { get; set; }
    public int RestTurns { get; set; }
    public int InternalState { get; set; }
}
