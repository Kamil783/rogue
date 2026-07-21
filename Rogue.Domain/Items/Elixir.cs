using Rogue.Domain.LevelAtributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rogue.Domain.Items
{
    public class Elixir : Item
    {
        public int MaxHealth { get; set; }
        public int Agility { get; set; }
        public int Strength { get; set; }
        public int TurnsCounter { get; set; }
        public Elixir(Position position) : base(position)
        {
            Type = ItemType.Elixir;
            MaxHealth = 10;
            Agility = 10;
            Strength = 10;
            TurnsCounter = 30;
        }
    }
}
