using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rogue.Domain.Enemies
{
    internal class Ogre: Enemy
    {
        public Ogre() : base()
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
