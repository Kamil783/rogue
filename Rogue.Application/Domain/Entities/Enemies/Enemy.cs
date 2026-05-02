namespace Rogue.Application.Domain.Entities.Enemies;

public enum EnemyKind
{
    Zombie,
    Vampire,
    Ghost,
    Ogre,
    SnakeMage
}

public abstract class Enemy : Character
{
    public EnemyKind Kind { get; set; }
    public int Hostility { get; set; }
    public int RoomId { get; set; }
    public bool IsChasing { get; set; }
    public bool IsInvisible { get; set; }
    public int RestTurns { get; set; }
    public int LastDamageDealt { get; set; }
    public bool FirstHitTaken { get; set; }
    public char Glyph { get; set; }
    public ConsoleColorKind Color { get; set; }
    public int InternalState { get; set; }

    public abstract void TakeTurn(IGameWorld world, Random rng);
}

public enum ConsoleColorKind
{
    White,
    Red,
    Green,
    Yellow,
    Blue,
    Cyan,
    Magenta,
    Gray
}
