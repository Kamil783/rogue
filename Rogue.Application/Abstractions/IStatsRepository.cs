using Rogue.Application.Domain.Entities;

namespace Rogue.Application.Abstractions;

public interface IStatsRepository
{
    void Append(Statistics stats);
    IReadOnlyList<Statistics> GetTopByTreasure(int count);
    IReadOnlyList<Statistics> GetAll();
}
