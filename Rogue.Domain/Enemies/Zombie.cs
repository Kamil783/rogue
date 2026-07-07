using Rogue.Domain.LevelAtributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rogue.Domain.Enemies
{
    public class Zombie: Enemy
    {
        public Zombie(Position position): base(position)
        {
            Type = EnemyType.Zombie;
            Health = 65;
            Agility = 15;
            Strength = 40;
            Hostility = 40;
        }

        // Add Pattern Move
    }
}
