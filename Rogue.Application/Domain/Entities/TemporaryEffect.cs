namespace Rogue.Application.Domain.Entities;

public sealed class TemporaryEffect
{
    public int RemainingTurns { get; set; }
    public int AgilityDelta { get; set; }
    public int StrengthDelta { get; set; }
    public int MaxHealthDelta { get; set; }
    public string SourceName { get; set; } = string.Empty;
}
