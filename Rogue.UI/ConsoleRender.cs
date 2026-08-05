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
        // The map is 78 wide; keep HUD lines within the same width.
        private const int HudWidth = 78;

        public void Initialize(GameEngine engine)
        {
            Console.CursorVisible = false;
            Console.OutputEncoding = Encoding.UTF8;
            TryResize(90, 32);
            Console.Clear();
        }

        public void RenderGame(GameSession session)
        {
            var level = session.CurrentLevel;
            var map = level.Map;

            // Redraw the whole map every frame so moved entities leave no trails.
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

            DrawHud(session);
        }

        // ---- Map entities ----------------------------------------------------

        private static void DrawEntities(Level level)
        {
            // Items/enemies live in internal lists, so we scan the map and use the
            // level's public point queries. Enemies draw over items; player over all.
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

        // ---- Stats + backpack panel -----------------------------------------

        private static void DrawHud(GameSession session)
        {
            var c = session.Character;
            var bp = c.CharacterBackpack;
            int row = session.CurrentLevel.Height; // first row below the map

            WriteRow(row++, new string('-', HudWidth), ConsoleColor.DarkGray);

            var weapon = c.CurrentWeapon.SubType == ItemSubtype.None
                ? "none"
                : c.CurrentWeapon.SubType.ToString();
            WriteRow(row++,
                $"Lvl:{session.CurrentLevelIndex}  HP:{c.Health}/{c.MaxHealth}  Str:{c.Strength}  Agi:{c.Agility}  Wpn:{weapon}",
                ConsoleColor.Cyan);

            // Active effects / sleep status.
            var status = new List<string>();
            if (c.IsSleep) status.Add("SLEEPING");
            foreach (var eff in c.ActiveEffects)
                status.Add($"{eff.Type} +{eff.Value} ({eff.TurnCounter})");
            WriteRow(row++, status.Count == 0 ? "" : "Effects: " + string.Join("   ", status),
                ConsoleColor.Magenta);

            WriteRow(row++, "BACKPACK — press the item key, then a number:", ConsoleColor.DarkGray);
            WriteRow(row++, "Food (j):    " + ListItems(bp._food, f => f.SubType.ToString()));
            WriteRow(row++, "Scrolls (e): " + ListItems(bp._scrolls, s => s.SubType.ToString()));
            WriteRow(row++, "Elixirs (k): " + ListItems(bp._elixirs, x => x.SubType.ToString()));
            WriteRow(row++, "Weapons (h): " + WeaponList(bp._weapons));

            WriteRow(row++,
                "Move WASD/arrows  Quit q    @you  O/z/v/g/s enemy  % food  ? scroll  ) weapon  $ gold",
                ConsoleColor.DarkGray);
        }

        // Number shown matches the index UseItem expects (0-based for these lists).
        private static string ListItems<T>(List<T> items, Func<T, string> label)
        {
            if (items.Count == 0) return "(empty)";
            var sb = new StringBuilder();
            for (int i = 0; i < items.Count; i++)
                sb.Append($"[{i}]{label(items[i])} ");
            return sb.ToString();
        }

        // Weapons are special: '0' unequips the current weapon, and a backpack
        // weapon at list index i is selected with the number i+1 (UseItem reads
        // _weapons[index-1]). The slot 0 "none" placeholder is skipped.
        private static string WeaponList(List<Weapon> weapons)
        {
            var sb = new StringBuilder("[0]unequip ");
            for (int i = 0; i < weapons.Count; i++)
            {
                if (weapons[i].SubType == ItemSubtype.None) continue;
                sb.Append($"[{i + 1}]{weapons[i].SubType} ");
            }
            return sb.ToString();
        }

        // ---- Low-level drawing ----------------------------------------------

        private static void DrawGlyph(Position position, char glyph, ConsoleColor color)
        {
            if (position.X < 0 || position.X >= Console.BufferWidth ||
                position.Y < 0 || position.Y >= Console.BufferHeight) return;
            Console.ForegroundColor = color;
            Console.SetCursorPosition(position.X, position.Y);
            Console.Write(glyph);
        }

        private static void WriteRow(int row, string text, ConsoleColor color = ConsoleColor.Gray)
        {
            if (row < 0 || row >= Console.BufferHeight) return;
            if (text.Length > HudWidth) text = text[..HudWidth];
            Console.ForegroundColor = color;
            Console.SetCursorPosition(0, row);
            Console.Write(text.PadRight(HudWidth));
            Console.ResetColor();
        }

        private static char GlyphFor(CellType cell) => cell switch
        {
            CellType.Wall => '#',
            CellType.Floor => '.',
            CellType.Corridor => '!',
            CellType.Exit => '>',
            _ => ' '
        };

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

        private static void TryResize(int width, int height)
        {
            try
            {
                if (!OperatingSystem.IsWindows()) return;
                width = Math.Min(width, Console.LargestWindowWidth);
                height = Math.Min(height, Console.LargestWindowHeight);
                Console.SetBufferSize(Math.Max(width, Console.BufferWidth), Math.Max(height, Console.BufferHeight));
                Console.SetWindowSize(width, height);
            }
            catch
            {
                // Console can't always be resized (redirected output, small terminal) — ignore.
            }
        }
    }
}
