using Rogue.Domain.LevelAtributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rogue.Domain.Items
{
    public class Treasure: Item
    {
        public int Value { get; set; }

        public Treasure(Position position): base(position)
        {
            Type = ItemType.Treasure;
            Value = 0;
        }
    }
}
