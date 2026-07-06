using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rogue.Domain.Enemies
{
    internal class Ghost: Enemy
    {
        public Ghost() : base()
        {
            Type = EnemyType.Ghost;
            Health = 15;
            Agility = 65;
            Strength = 15;
            Hostility = 15;
        }
        // Add Pattern Move
    }
}
