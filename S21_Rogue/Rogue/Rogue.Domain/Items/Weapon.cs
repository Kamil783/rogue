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
        protected Weapon(int x, int y, ItemType type, ItemSubtype subtype) : base(x, y, type, subtype)
        {
            Random rnd = new Random();
            Strength = rnd.Next(1, 10);
        }
    }
}
