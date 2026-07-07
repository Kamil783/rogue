using Rogue.Domain.LevelAtributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rogue.Domain.Enemies
{
    public class SnakeMage: Enemy
    {
        public SnakeMage(Position position) : base(position)
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
