using System.Text.Json;
using Rogue.Application.Abstractions;
using Rogue.Application.Domain.Entities;

namespace Rogue.Data;

public sealed class JsonStatsRepository : IStatsRepository
{
    private readonly string _path;
    private static readonly JsonSerializerOptions Options = new()
    {
        WriteIndented = true,
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase
    };

    public JsonStatsRepository(string? directory = null)
    {
        var dir = directory ?? Path.Combine(AppContext.BaseDirectory, "save");
        Directory.CreateDirectory(dir);
        _path = Path.Combine(dir, "stats.json");
    }

    public void Append(Statistics stats)
    {
        var current = LoadAll();
        current.Add(Mappers.ToDto(stats));
        File.WriteAllText(_path, JsonSerializer.Serialize(current, Options));
    }

    public IReadOnlyList<Statistics> GetAll()
    {
        return LoadAll().Select(Mappers.FromStatsDto).ToList();
    }

    public IReadOnlyList<Statistics> GetTopByTreasure(int count)
    {
        return GetAll().OrderByDescending(s => s.TreasureCollected).Take(count).ToList();
    }

    private List<StatsDto> LoadAll()
    {
        if (!File.Exists(_path)) return new List<StatsDto>();
        try
        {
            return JsonSerializer.Deserialize<List<StatsDto>>(File.ReadAllText(_path), Options) ?? new List<StatsDto>();
        }
        catch
        {
            return new List<StatsDto>();
        }
    }
}
