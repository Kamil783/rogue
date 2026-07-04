using System.Text;
using Rogue.Domain;
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

            DrawGlyph(session.Character.Position, '@', ConsoleColor.Yellow);
            Console.ResetColor();

            Console.SetCursorPosition(0, level.Height + 1);
            Console.Write($"Level {level.Number}   [WASD/arrows] move   [q] quit          ");
        }

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
