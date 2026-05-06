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
