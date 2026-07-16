using Rogue.Domain.LevelAtributes;
using Rogue.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rogue.Domain.Enemies
{
    public enum EnemyType
    {
        Zombie,
        Vampire,
        Ghost,
        Ogre,
        SnakeMage
    }
    public abstract class Enemy : Creature
    {
        public EnemyType Type { get; set; }
        public int Hostility { get; set; }
        public Direction PatternDirection { get; set; }
        public Enemy(Position position) : base(position) {}

        public virtual Direction GetDirectionOfPatternMove() 
        {
            return PatternDirection;
        } 
        public virtual void SuccessPatternMove() {}

        public virtual void ChangePattrenDirection() {}
    }
}
