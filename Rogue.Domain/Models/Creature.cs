using Rogue.Domain.LevelAtributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rogue.Domain.Models
{
    abstract public class Creature
    {
        public Position Position { get; set; }
        public int Health { get; protected set; }
        public int Agility { get; protected set; }
        public int Strength { get; protected set; }

        public bool IsAlive => Health > 0;

        public Creature(Position position) 
        {
            Position = position;
        }

        public void TakeDamage (int damage)
        {
            if(damage < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(damage));
            }
            Health = Math.Max(0, Health - damage);
        }

        public bool HitCalculate(Creature defender)
        {
            int correction = 15;
            if (Agility + correction > defender.Agility) return true;
            return false;
        }
    }
}
