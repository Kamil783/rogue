using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rogue.Domain.Items
{
    internal class Scrolls : Item
    {
        private int MaxHealth { get; set; }
        private int Health { get; set; }
        private int Agility { get; set; }
        private int Strength { get; set; }
        protected Scrolls() : base()
        {
            Random rnd = new Random();
            MaxHealth = rnd.Next(1, 20);
            Health = MaxHealth / 3;
            Agility = rnd.Next(1, 10);
            Strength = rnd.Next(1, 10);
        }
    }
}
