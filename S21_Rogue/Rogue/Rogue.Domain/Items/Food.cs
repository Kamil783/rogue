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

        protected Food () : base()
        {
            Random rnd = new Random();
            Health = rnd.Next(1, 20);
        }
    }
}
