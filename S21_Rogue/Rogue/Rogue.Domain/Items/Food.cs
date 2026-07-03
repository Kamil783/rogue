using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rogue.Domain.Items
{
    internal class Food : Item
    {
        private int Health {  get; set; }

        public Food(int x, int y) : base(x, y)
        {
            Type = ItemType.Food;  
            Health = 10;
        }
    }
}
