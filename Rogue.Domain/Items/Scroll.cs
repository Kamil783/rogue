using Rogue.Domain.LevelAtributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rogue.Domain.Items
{
    public class Scroll : Item
    {
        private int MaxHealth { get; set; }
        private int Health { get; set; }
        private int Agility { get; set; }
        private int Strength { get; set; }
        public Scroll(Position position) : base(position)
        {
            Type = ItemType.Scroll;
            MaxHealth = 10;
            Health = MaxHealth / 3;
            Agility = 10;
            Strength = 10;
        }
    }
}
