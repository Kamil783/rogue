using Rogue.Domain.LevelAtributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rogue.Domain.Enemies
{
    public class Ghost: Enemy
    {
        public Ghost(Position position) : base(position)
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
