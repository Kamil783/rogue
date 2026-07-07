using Rogue.Domain.LevelAtributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rogue.Domain.Enemies
{
    public class Vampire: Enemy
    {
        public Vampire(Position position) : base(position)
        {
            Type = EnemyType.Vampire;
            Health = 65;
            Agility = 65;
            Strength = 40;
            Hostility = 65;
        }
        // Add Pattern Move
    }
}
