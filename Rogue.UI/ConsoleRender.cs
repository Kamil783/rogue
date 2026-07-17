using System.Text;
using Rogue.Domain;
using Rogue.Domain.Enemies;
using Rogue.Domain.Items;
using Rogue.Domain.LevelAtributes;
using Rogue.Domain.Models;

namespace Rogue.UI
{
    public class ConsoleRender : IRender
    {
        public void Initialize(GameEngine engine)
        {
            Console.CursorVisible = false;
            Console.OutputEncoding = Encoding.UTF8;
            Console.Clear();
        }

        public void RenderGame(GameSession session)
        {
            var level = session.CurrentLevel;
            var map = level.Map;

            var line = new char[level.Width];
            for (int y = 0; y < level.Height; y++)
            {
                for (int x = 0; x < level.Width; x++)
                {
                    line[x] = GlyphFor(map[x, y]);
                }
                Console.SetCursorPosition(0, y);
                Console.Write(line);
            }

            DrawGlyph(level.ExitPosition, '>', ConsoleColor.Green);

            DrawEntities(level);

            DrawGlyph(session.Character.Position, '@', ConsoleColor.Yellow);
            Console.ResetColor();

            Console.SetCursorPosition(0, level.Height + 1);
            Console.Write($"Level {level.Number}   [WASD/arrows] move   [q] quit          ");
            Console.SetCursorPosition(0, level.Height + 2);
            Console.Write("@ you   O/z/v/g/s enemy   % food   ? scroll   ) weapon   $ gold   > exit   ");
        }

        private static void DrawEntities(Level level)
        {
            for (int y = 0; y < level.Height; y++)
            {
                for (int x = 0; x < level.Width; x++)
                {
                    var pos = new Position(x, y);

                    var enemy = level.HasEnemyAt(pos);
                    if (enemy != null)
                    {
                        DrawGlyph(pos, EnemyGlyph(enemy.Type), ConsoleColor.Red);
                        continue;
                    }

                    var item = level.HasItemAt(pos);
                    if (item != null)
                    {
                        DrawGlyph(pos, ItemGlyph(item.Type), ItemColor(item.Type));
                    }
                }
            }
        }

        private static char ItemGlyph(ItemType type) => type switch
        {
            ItemType.Food => '%',
            ItemType.Scroll => '?',
            ItemType.Weapon => ')',
            ItemType.Elixir => '!',
            ItemType.Treasure => '$',
            _ => '*'
        };

        private static ConsoleColor ItemColor(ItemType type) => type switch
        {
            ItemType.Treasure => ConsoleColor.Yellow,
            ItemType.Weapon => ConsoleColor.White,
            ItemType.Scroll => ConsoleColor.Magenta,
            ItemType.Food => ConsoleColor.Green,
            ItemType.Elixir => ConsoleColor.Cyan,
            _ => ConsoleColor.Gray
        };

        private static char EnemyGlyph(EnemyType type) => type switch
        {
            EnemyType.Zombie => 'z',
            EnemyType.Vampire => 'v',
            EnemyType.Ghost => 'g',
            EnemyType.Ogre => 'O',
            EnemyType.SnakeMage => 's',
            _ => 'e'
        };

        private static void DrawGlyph(Position position, char glyph, ConsoleColor color)
        {
            Console.ForegroundColor = color;
            Console.SetCursorPosition(position.X, position.Y);
            Console.Write(glyph);
        }

        private static char GlyphFor(CellType cell) => cell switch
        {
            CellType.Wall => '#',
            CellType.Floor => '.',
            CellType.Corridor => '#',
            CellType.Exit => '>',
            _ => ' '
        };
    }
}
