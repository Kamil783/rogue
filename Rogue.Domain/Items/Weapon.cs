using Rogue.Domain.LevelAtributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rogue.Domain.Items
{
    public class Weapon : Item
    {
        private int Strength { get; set; }
        public Weapon(Position position) : base(position)
        {
            Type = ItemType.Weapon;
            Strength = 10;
        }
    }
}
