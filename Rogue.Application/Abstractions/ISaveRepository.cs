using Rogue.Application.Domain.Entities;

namespace Rogue.Application.Abstractions;

public interface ISaveRepository
{
    void SaveSession(GameSession session);
    GameSession? LoadSession();
    bool HasSavedSession();
    void DeleteSession();
}
