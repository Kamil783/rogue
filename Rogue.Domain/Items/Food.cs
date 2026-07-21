using Rogue.Domain.LevelAtributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rogue.Domain.Items
{
    public class Food : Item
    {
        public int Health {  get; set; }

        public Food(Position position) : base(position)
        {
            Type = ItemType.Food;  
            Health = 10;
        }
    }
}
