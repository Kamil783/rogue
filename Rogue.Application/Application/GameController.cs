using Rogue.Application.Abstractions;
using Rogue.Application.Domain.Entities;
using Rogue.Application.Domain.Entities.Items;
using Rogue.Application.Domain.Services;

namespace Rogue.Application.Application;

public sealed class GameController
{
    private readonly IRenderer _renderer;
    private readonly IInputProvider _input;
    private readonly ISaveRepository _save;
    private readonly IStatsRepository _stats;

    public GameController(IRenderer renderer, IInputProvider input, ISaveRepository save, IStatsRepository stats)
    {
        _renderer = renderer;
        _input = input;
        _save = save;
        _stats = stats;
    }

    public void Run()
    {
        _renderer.Initialize();
        try
        {
            MainMenuLoop();
        }
        finally
        {
            _renderer.Shutdown();
        }
    }

    private void MainMenuLoop()
    {
        var selected = 0;
        while (true)
        {
            var hasSave = _save.HasSavedSession();
            _renderer.RenderMainMenu(selected, hasSave);
            var key = _input.ReadInput();
            switch (key)
            {
                case GameInput.Up: selected = (selected + 3) % 4; break;
                case GameInput.Down: selected = (selected + 1) % 4; break;
                case GameInput.Confirm:
                    var shouldExit = HandleMenuChoice(selected, hasSave);
                    if (shouldExit) return;
                    break;
                case GameInput.Quit: return;
            }
        }
    }

    private bool HandleMenuChoice(int index, bool hasSave)
    {
        switch (index)
        {
            case 0:
                StartNewGame();
                return false;
            case 1:
                if (hasSave) ContinueGame();
                return false;
            case 2:
                ShowLeaderboard();
                return false;
            case 3:
                return true;
        }
        return false;
    }

    private void StartNewGame()
    {
        var engine = GameEngine.StartNew();
        PlayLoop(engine);
    }

    private void ContinueGame()
    {
        var session = _save.LoadSession();
        if (session == null) return;
        var engine = GameEngine.Resume(session);
        PlayLoop(engine);
    }

    private void ShowLeaderboard()
    {
        var top = _stats.GetTopByTreasure(10);
        _renderer.RenderLeaderboard(top);
        while (true)
        {
            var key = _input.ReadInput();
            if (key is GameInput.Cancel or GameInput.Quit or GameInput.Confirm) return;
        }
    }

    private void PlayLoop(GameEngine engine)
    {
        while (!engine.Session.GameOver)
        {
            _renderer.RenderGame(engine.Session);
            var key = _input.ReadInput();
            HandlePlayInput(engine, key);
        }

        engine.Session.Player.Stats.Completed = engine.Session.Victory;
        engine.Session.Player.Stats.TreasureCollected = Math.Max(engine.Session.Player.Stats.TreasureCollected, engine.Session.Player.Treasure);
        _stats.Append(engine.Session.Player.Stats);
        _save.DeleteSession();

        _renderer.RenderGame(engine.Session);
        _renderer.RenderMessage(engine.Session.Victory
            ? $"Victory! Press any key. Treasure: {engine.Session.Player.Treasure}"
            : $"You died. Press any key. Treasure: {engine.Session.Player.Treasure}");
        _input.ReadInput();
    }

    private void HandlePlayInput(GameEngine engine, GameInput key)
    {
        switch (key)
        {
            case GameInput.Up: engine.MovePlayer(Direction.North); break;
            case GameInput.Down: engine.MovePlayer(Direction.South); break;
            case GameInput.Left: engine.MovePlayer(Direction.West); break;
            case GameInput.Right: engine.MovePlayer(Direction.East); break;
            case GameInput.UseWeapon: ChooseAndUse(engine, ItemType.Weapon, allowZero: true); break;
            case GameInput.UseFood: ChooseAndUse(engine, ItemType.Food, allowZero: false); break;
            case GameInput.UseElixir: ChooseAndUse(engine, ItemType.Elixir, allowZero: false); break;
            case GameInput.UseScroll: ChooseAndUse(engine, ItemType.Scroll, allowZero: false); break;
            case GameInput.Quit:
                _save.SaveSession(engine.Session);
                engine.Session.GameOver = true;
                engine.Log("Game saved. Goodbye.");
                break;
        }

        if (engine.Session.GameOver) return;
        _save.SaveSession(engine.Session);
    }

    private void ChooseAndUse(GameEngine engine, ItemType type, bool allowZero)
    {
        var items = engine.Session.Player.Backpack.GetByType(type);
        if (items.Count == 0 && !allowZero)
        {
            engine.Log($"You have no {type.ToString().ToLower()}s.");
            return;
        }

        var entries = new List<string>();
        if (allowZero) entries.Add("0) unequip current weapon");
        for (var i = 0; i < items.Count; i++) entries.Add($"{i + 1}) {items[i].Name}");

        _renderer.RenderInventoryPrompt($"Use {type.ToString().ToLower()}", entries, allowZero);
        var key = _input.ReadInput();
        var index = key switch
        {
            GameInput.Choice0 => 0,
            GameInput.Choice1 => 1,
            GameInput.Choice2 => 2,
            GameInput.Choice3 => 3,
            GameInput.Choice4 => 4,
            GameInput.Choice5 => 5,
            GameInput.Choice6 => 6,
            GameInput.Choice7 => 7,
            GameInput.Choice8 => 8,
            GameInput.Choice9 => 9,
            _ => -1
        };

        if (index < 0) return;
        if (index == 0)
        {
            if (allowZero) engine.UnequipWeapon();
            return;
        }
        var itemIndex = index - 1;
        if (itemIndex >= items.Count) return;
        engine.UseItem(items[itemIndex]);
    }
}
