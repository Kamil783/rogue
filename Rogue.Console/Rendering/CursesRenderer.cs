using Mindmagma.Curses;
using Rogue.Application.Abstractions;
using Rogue.Application.Domain.Entities;
using Rogue.Application.Domain.Entities.Items;

namespace Rogue.Console.Rendering;

public sealed class CursesRenderer : IRenderer
{
    private IntPtr _screen;
    private bool _initialized;
    private int _termWidth = 80;
    private int _termHeight = 24;

    private int MapTop => 0;
    private int StatusRow => Math.Min(_termHeight - 5, 18);
    private int StatsRow => StatusRow + 1;
    private int MessagesTop => StatusRow + 2;
    private int MessagesCount => Math.Max(1, _termHeight - MessagesTop - 1);

    public void Initialize()
    {
        _screen = NCurses.InitScreen();
        NCurses.NoEcho();
        NCurses.CBreak();
        NCurses.Keypad(_screen, true);
        NCurses.SetCursor(0);
        if (NCurses.HasColors())
        {
            ColorPairs.Initialize();
        }
        RefreshTerminalSize();
        _initialized = true;
    }

    public void Shutdown()
    {
        if (!_initialized) return;
        NCurses.EndWin();
        _initialized = false;
    }

    private void RefreshTerminalSize()
    {
        NCurses.GetMaxYX(_screen, out _termHeight, out _termWidth);
        if (_termWidth < 1) _termWidth = 80;
        if (_termHeight < 1) _termHeight = 24;
    }

    private void SafeMoveAddString(int y, int x, string text)
    {
        if (y < 0 || y >= _termHeight || x < 0 || x >= _termWidth) return;
        var available = _termWidth - x;
        if (available <= 0) return;
        var clipped = text.Length > available ? text[..available] : text;
        try { NCurses.MoveAddString(y, x, clipped); } catch { /* ignore edge writes */ }
    }

    private void SafeMoveAddChar(int y, int x, char ch)
    {
        if (y < 0 || y >= _termHeight || x < 0 || x >= _termWidth) return;
        if (y == _termHeight - 1 && x == _termWidth - 1) return;
        try { NCurses.MoveAddChar(y, x, ch); } catch { /* ignore edge writes */ }
    }

    private void FillRow(int y, char ch)
    {
        if (y < 0 || y >= _termHeight) return;
        var line = new string(ch, _termWidth);
        SafeMoveAddString(y, 0, line);
    }

    private void ClearRow(int y) => FillRow(y, ' ');

    public void RenderMainMenu(int selectedIndex, bool hasSave)
    {
        RefreshTerminalSize();
        NCurses.Erase();
        var title = new[]
        {
            "  ____   ___   ____ _   _ _____ ",
            " |  _ \\ / _ \\ / ___| | | | ____|",
            " | |_) | | | | |  _| | | |  _|  ",
            " |  _ <| |_| | |_| | |_| | |___ ",
            " |_| \\_\\\\___/ \\____|\\___/|_____|"
        };
        for (var i = 0; i < title.Length; i++)
        {
            SafeMoveAddString(2 + i, 4, title[i]);
        }

        var items = new[]
        {
            "[1] New game",
            hasSave ? "[2] Continue" : "[2] Continue (no save)",
            "[3] Leaderboard",
            "[4] Quit"
        };
        for (var i = 0; i < items.Length; i++)
        {
            var prefix = i == selectedIndex ? "-> " : "   ";
            var pair = i == selectedIndex ? ColorPairs.Player : ColorPairs.Default;
            NCurses.AttributeOn(NCurses.ColorPair(pair));
            SafeMoveAddString(10 + i, 6, prefix + items[i]);
            NCurses.AttributeOff(NCurses.ColorPair(pair));
        }

        SafeMoveAddString(_termHeight - 3, 4, "Use arrow keys to select, Enter to confirm.");
        SafeMoveAddString(_termHeight - 2, 4, "WASD = move,  h=weapon  j=food  k=elixir  e=scroll  q=save&quit");
        NCurses.Refresh();
    }

    public void RenderLeaderboard(IReadOnlyList<Statistics> stats)
    {
        RefreshTerminalSize();
        NCurses.Erase();
        NCurses.AttributeOn(NCurses.ColorPair(ColorPairs.Status));
        SafeMoveAddString(1, 2, "===== LEADERBOARD (top by treasure) =====");
        NCurses.AttributeOff(NCurses.ColorPair(ColorPairs.Status));
        SafeMoveAddString(3, 1, "  # | Treas | Lvl | Kills | Food | Elx | Scr | Hit | Miss | Steps | Result | Date");
        var rowLimit = _termHeight - 6;
        for (var i = 0; i < stats.Count && i < rowLimit; i++)
        {
            var s = stats[i];
            var line = $" {i + 1,2} | {s.TreasureCollected,5} | {s.LevelReached,3} | {s.EnemiesDefeated,5} | {s.FoodEaten,4} | {s.ElixirsDrunk,3} | {s.ScrollsRead,3} | {s.HitsLanded,3} | {s.HitsTaken,4} | {s.TilesWalked,5} | {(s.Completed ? "Won " : "Died")} | {s.Date:yyyy-MM-dd HH:mm}";
            SafeMoveAddString(4 + i, 1, line);
        }
        SafeMoveAddString(_termHeight - 2, 2, "Press any key to return to menu.");
        NCurses.Refresh();
    }

    public void RenderInventoryPrompt(string title, IReadOnlyList<string> entries, bool allowZero)
    {
        RefreshTerminalSize();
        var maxEntryLen = entries.Count == 0 ? 0 : entries.Max(e => e.Length);
        var width = Math.Min(_termWidth - 4, Math.Max(40, Math.Max(title.Length + 4, maxEntryLen + 6)));
        var height = Math.Min(_termHeight - 2, entries.Count + 5);
        var startY = Math.Max(1, (_termHeight - height) / 2);
        var startX = Math.Max(1, (_termWidth - width) / 2);

        for (var y = startY; y < startY + height; y++)
        for (var x = startX; x < startX + width; x++) SafeMoveAddChar(y, x, ' ');

        for (var x = startX; x < startX + width; x++)
        {
            SafeMoveAddChar(startY, x, '-');
            SafeMoveAddChar(startY + height - 1, x, '-');
        }
        for (var y = startY; y < startY + height; y++)
        {
            SafeMoveAddChar(y, startX, '|');
            SafeMoveAddChar(y, startX + width - 1, '|');
        }
        SafeMoveAddString(startY, startX + 2, $" {title} ");
        for (var i = 0; i < entries.Count && i < height - 4; i++)
        {
            SafeMoveAddString(startY + 1 + i, startX + 2, entries[i]);
        }
        var hint = allowZero ? "Choose 0-9, Esc to cancel." : "Choose 1-9, Esc to cancel.";
        SafeMoveAddString(startY + height - 2, startX + 2, hint);
        NCurses.Refresh();
    }

    public void RenderMessage(string message)
    {
        ClearRow(0);
        SafeMoveAddString(0, 0, message);
        NCurses.Refresh();
    }

    public void RenderGame(GameSession session)
    {
        RefreshTerminalSize();
        NCurses.Erase();
        var level = session.CurrentLevel;
        DrawMap(level);
        DrawItems(level);
        DrawEnemies(level);
        DrawPlayer(session.Player);
        DrawStatus(session);
        DrawMessages(session);
        NCurses.Refresh();
    }

    private void DrawMap(Level level)
    {
        for (var y = 0; y < level.Height; y++)
        for (var x = 0; x < level.Width; x++)
        {
            var tile = level.Map[x, y];
            if (!tile.Discovered) continue;

            char glyph;
            short pair;
            switch (tile.Type)
            {
                case TileType.Wall: glyph = '#'; pair = ColorPairs.Wall; break;
                case TileType.Floor: glyph = '.'; pair = ColorPairs.Floor; break;
                case TileType.Door: glyph = '+'; pair = ColorPairs.Door; break;
                case TileType.Corridor: glyph = '#'; pair = ColorPairs.Corridor; break;
                case TileType.Exit: glyph = '>'; pair = ColorPairs.Exit; break;
                default: continue;
            }

            if (!tile.Visible)
            {
                if (tile.Type is TileType.Wall or TileType.Door) pair = ColorPairs.Discovered;
                else continue;
            }

            NCurses.AttributeOn(NCurses.ColorPair(pair));
            SafeMoveAddChar(MapTop + y, x, glyph);
            NCurses.AttributeOff(NCurses.ColorPair(pair));
        }
    }

    private void DrawItems(Level level)
    {
        foreach (var item in level.Items)
        {
            var tile = level.GetTile(item.Position);
            if (!tile.Visible) continue;
            var pair = item.Type == ItemType.Treasure ? ColorPairs.Treasure : ColorPairs.Item;
            NCurses.AttributeOn(NCurses.ColorPair(pair));
            SafeMoveAddChar(MapTop + item.Position.Y, item.Position.X, item.Glyph);
            NCurses.AttributeOff(NCurses.ColorPair(pair));
        }
    }

    private void DrawEnemies(Level level)
    {
        foreach (var enemy in level.Enemies)
        {
            if (!enemy.Alive) continue;
            var tile = level.GetTile(enemy.Position);
            if (!tile.Visible) continue;
            if (enemy.IsInvisible && !enemy.IsChasing) continue;
            var pair = ColorPairs.ForEnemy(enemy.Kind);
            NCurses.AttributeOn(NCurses.ColorPair(pair));
            SafeMoveAddChar(MapTop + enemy.Position.Y, enemy.Position.X, enemy.Glyph);
            NCurses.AttributeOff(NCurses.ColorPair(pair));
        }
    }

    private void DrawPlayer(Player player)
    {
        NCurses.AttributeOn(NCurses.ColorPair(ColorPairs.Player));
        SafeMoveAddChar(MapTop + player.Position.Y, player.Position.X, '@');
        NCurses.AttributeOff(NCurses.ColorPair(ColorPairs.Player));
    }

    private void DrawStatus(GameSession session)
    {
        var p = session.Player;
        var line = $"Lvl:{session.CurrentLevelIndex}/21  HP:{p.Health}/{p.MaxHealth}  Str:{p.Strength}  Agi:{p.Agility}  Wpn:{p.EquippedWeapon?.Name ?? "none"}  Gold:{p.Treasure}";
        NCurses.AttributeOn(NCurses.ColorPair(ColorPairs.Status));
        SafeMoveAddString(StatusRow, 0, line);
        NCurses.AttributeOff(NCurses.ColorPair(ColorPairs.Status));

        var stats = $"K:{p.Stats.EnemiesDefeated} F:{p.Stats.FoodEaten} E:{p.Stats.ElixirsDrunk} S:{p.Stats.ScrollsRead} Hit:{p.Stats.HitsLanded} Miss:{p.Stats.HitsTaken} Steps:{p.Stats.TilesWalked}";
        SafeMoveAddString(StatsRow, 0, stats);
    }

    private void DrawMessages(GameSession session)
    {
        var startY = MessagesTop;
        var maxLines = MessagesCount;
        var msgs = session.MessageLog.TakeLast(maxLines).ToList();
        for (var i = 0; i < maxLines; i++)
        {
            var text = i < msgs.Count ? msgs[i] : string.Empty;
            SafeMoveAddString(startY + i, 0, text);
        }
    }
}
