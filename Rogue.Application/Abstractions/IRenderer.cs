using Rogue.Application.Domain.Entities;

namespace Rogue.Application.Abstractions;

public interface IRenderer
{
    void RenderGame(GameSession session);
    void RenderMainMenu(int selectedIndex, bool hasSave);
    void RenderLeaderboard(IReadOnlyList<Statistics> stats);
    void RenderInventoryPrompt(string title, IReadOnlyList<string> entries, bool allowZero);
    void RenderMessage(string message);
    void Shutdown();
    void Initialize();
}

public interface IInputProvider
{
    GameInput ReadInput();
}

public enum GameInput
{
    None,
    Up,
    Down,
    Left,
    Right,
    UseWeapon,
    UseFood,
    UseElixir,
    UseScroll,
    Confirm,
    Cancel,
    Quit,
    Choice0,
    Choice1,
    Choice2,
    Choice3,
    Choice4,
    Choice5,
    Choice6,
    Choice7,
    Choice8,
    Choice9
}
