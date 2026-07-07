using Rogue.Domain.LevelAtributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rogue.Domain.Enemies
{
    public class Ogre: Enemy
    {
        public Ogre(Position position) : base(position)
        {
            Type = EnemyType.Ogre;
            Health = 75;
            Agility = 15;
            Strength = 50;
            Hostility = 40;
        }
        // Add Pattern Move
    }
}
