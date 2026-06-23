using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rogue.Domain.Models
{
    abstract class Creature: GameObject
    {
        public int Health { get; protected set; }
        public int Agility { get; protected set; }
        public int Strength { get; protected set; }

        public bool IsAlive => Health > 0;

        public Creature(): base() {}

        public void TakeDamage (int damage)
        {
            if(damage < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(damage));
            }
            Health = Math.Max(0, Health - damage);
        }

        public virtual void Move(int x, int y) 
        {
            X += x;
            Y += y;
        }
    }
}
