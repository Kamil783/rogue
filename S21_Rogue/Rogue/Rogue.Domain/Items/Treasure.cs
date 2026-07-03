using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rogue.Domain.Items
{
    internal class Treasure: Item
    {
        public int Value { get; set; }

        public Treasure(int x, int y): base(x, y)
        {
            Type = ItemType.Treasure;
            Value = 100;
        }
    }
}
