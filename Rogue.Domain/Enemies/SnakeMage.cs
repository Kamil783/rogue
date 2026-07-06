using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rogue.Domain.Enemies
{
    internal class SnakeMage: Enemy
    {
        public SnakeMage() : base()
        {
            Type = EnemyType.SnakeMage;
            Health = 75;
            Agility = 75;
            Strength = 40;
            Hostility = 75;
        }
        // Add Pattern Move
    }
}
