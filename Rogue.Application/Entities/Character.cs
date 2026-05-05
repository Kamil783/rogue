namespace Rogue.Application.Entities;

public abstract class Character
{
    public int Health { get; set; }
    public int MaxHealth { get; set; }
    public int BaseAgility { get; set; }
    public int BaseStrength { get; set; }
    public bool Alive => Health > 0;
}
