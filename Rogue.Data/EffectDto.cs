namespace Rogue.Data;

public sealed class EffectDto
{
    public int RemainingTurns { get; set; }
    public int AgilityDelta { get; set; }
    public int StrengthDelta { get; set; }
    public int MaxHealthDelta { get; set; }
    public string SourceName { get; set; } = string.Empty;
}
