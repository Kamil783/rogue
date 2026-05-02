using Rogue.Application.Domain.Entities.Enemies;

namespace Rogue.Application.Domain.Entities;

public interface IGameWorld
{
    Level CurrentLevel { get; }
    Player Player { get; }
    Random Rng { get; }
    void Log(string message);
    bool IsWalkable(Position position);
    bool IsTransparent(Position position);
    Enemy? EnemyAt(Position position);
    bool TryAttackPlayer(Enemy enemy);
}
