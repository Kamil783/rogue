using Rogue.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rogue.Domain.Enemies
{
    enum EnemyType
    {
        Zombie,
        Vampire,
        Ghost,
        Ogre,
        SnakeMage
    }
    abstract class Enemy : Creature
    {
        public EnemyType Type { get; set; }
        public int Hostility { get; set; }
        public Enemy() : base() {}

        public virtual void PatternMove() {} 
    }
}
