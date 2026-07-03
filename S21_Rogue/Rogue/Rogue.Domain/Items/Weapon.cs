using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rogue.Domain.Items
{
    internal class Weapon : Item
    {
        private int Strength { get; set; }
        protected Weapon(int x, int y) : base(x, y)
        {
            Type = ItemType.Weapon;
            Strength = 10;
        }
    }
}
