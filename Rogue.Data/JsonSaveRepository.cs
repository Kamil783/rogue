using System.Text.Json;
using Rogue.Application.Abstractions;
using Rogue.Application.Domain.Entities;

namespace Rogue.Data;

public sealed class JsonSaveRepository : ISaveRepository
{
    private readonly string _path;
    private static readonly JsonSerializerOptions Options = new()
    {
        WriteIndented = true,
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase
    };

    public JsonSaveRepository(string? directory = null)
    {
        var dir = directory ?? Path.Combine(AppContext.BaseDirectory, "save");
        Directory.CreateDirectory(dir);
        _path = Path.Combine(dir, "session.json");
    }

    public bool HasSavedSession() => File.Exists(_path);

    public void SaveSession(GameSession session)
    {
        var dto = Mappers.ToDto(session);
        File.WriteAllText(_path, JsonSerializer.Serialize(dto, Options));
    }

    public GameSession? LoadSession()
    {
        if (!File.Exists(_path)) return null;
        try
        {
            var dto = JsonSerializer.Deserialize<SessionDto>(File.ReadAllText(_path), Options);
            return dto == null ? null : Mappers.FromDto(dto);
        }
        catch
        {
            return null;
        }
    }

    public void DeleteSession()
    {
        if (File.Exists(_path)) File.Delete(_path);
    }
}
