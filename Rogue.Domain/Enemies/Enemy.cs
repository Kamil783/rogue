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
        public bool HasSeenCharacter { get; set; } = false;
        public Enemy(Position position) : base(position) {}

        public virtual Direction GetDirectionOfPatternMove() 
        {
            return PatternDirection;
        } 
        public virtual void SuccessPatternMove() {}

        public virtual void ChangePattrenDirection() {}
        public virtual void OnCombatStarted() {}

        public virtual List<Position> GetPatternMoveCells()
        {
            var path = new List<Position>();
            var position = GetNextPosition();
            path.Add(position);
            return path;

        }

        public virtual Position GetNextPosition()
        {
            var position = new Position(); 
            switch (PatternDirection)
            {
                case Direction.South: position.X = Position.X; position.Y = Position.Y - 1; break;
                case Direction.North: position.X = Position.X; position.Y = Position.Y + 1; break;
                case Direction.East: position.X = Position.X + 1; position.Y = Position.Y; break;
                case Direction.West: position.X = Position.X - 1; position.Y = Position.Y; break;
            }
            return position;
        }
    }
}
