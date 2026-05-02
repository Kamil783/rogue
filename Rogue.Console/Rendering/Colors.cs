using Mindmagma.Curses;
using Rogue.Application.Domain.Entities.Enemies;

namespace Rogue.Console.Rendering;

internal static class ColorPairs
{
    public const short Default = 1;
    public const short Wall = 2;
    public const short Floor = 3;
    public const short Door = 4;
    public const short Corridor = 5;
    public const short Exit = 6;
    public const short Player = 7;
    public const short Item = 8;
    public const short Treasure = 9;
    public const short EnemyZombie = 10;
    public const short EnemyVampire = 11;
    public const short EnemyGhost = 12;
    public const short EnemyOgre = 13;
    public const short EnemySnakeMage = 14;
    public const short Status = 15;
    public const short Discovered = 16;

    public static void Initialize()
    {
        NCurses.StartColor();
        NCurses.UseDefaultColors();
        NCurses.InitPair(Default, CursesColor.WHITE, -1);
        NCurses.InitPair(Wall, CursesColor.WHITE, -1);
        NCurses.InitPair(Floor, CursesColor.WHITE, -1);
        NCurses.InitPair(Door, CursesColor.YELLOW, -1);
        NCurses.InitPair(Corridor, CursesColor.WHITE, -1);
        NCurses.InitPair(Exit, CursesColor.MAGENTA, -1);
        NCurses.InitPair(Player, CursesColor.CYAN, -1);
        NCurses.InitPair(Item, CursesColor.YELLOW, -1);
        NCurses.InitPair(Treasure, CursesColor.YELLOW, -1);
        NCurses.InitPair(EnemyZombie, CursesColor.GREEN, -1);
        NCurses.InitPair(EnemyVampire, CursesColor.RED, -1);
        NCurses.InitPair(EnemyGhost, CursesColor.WHITE, -1);
        NCurses.InitPair(EnemyOgre, CursesColor.YELLOW, -1);
        NCurses.InitPair(EnemySnakeMage, CursesColor.WHITE, -1);
        NCurses.InitPair(Status, CursesColor.CYAN, -1);
        NCurses.InitPair(Discovered, CursesColor.BLUE, -1);
    }

    public static short ForEnemy(EnemyKind kind) => kind switch
    {
        EnemyKind.Zombie => EnemyZombie,
        EnemyKind.Vampire => EnemyVampire,
        EnemyKind.Ghost => EnemyGhost,
        EnemyKind.Ogre => EnemyOgre,
        EnemyKind.SnakeMage => EnemySnakeMage,
        _ => Default
    };
}
