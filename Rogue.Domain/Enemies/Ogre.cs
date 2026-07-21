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
            Hostility = 5;
            PatternDirection = Direction.East;
        }

        public override List<Position> GetPatternMoveCells()
        {
            return GetNextPositionForOgre();
        }

        private List<Position> GetNextPositionForOgre()
        {
            var path = new List<Position>();
            var firstPosition = new Position();
            var secondPosition = new Position();
            switch (PatternDirection)
            {
                case Direction.South: 
                    firstPosition.X = Position.X; firstPosition.Y = Position.Y - 1;
                    secondPosition.X = Position.X; secondPosition.Y = Position.Y - 2;
                    break;
                case Direction.North: 
                    firstPosition.X = Position.X; firstPosition.Y = Position.Y + 1;
                    secondPosition.X = Position.X; secondPosition.Y = Position.Y + 2;
                    break;
                case Direction.East: 
                    firstPosition.X = Position.X + 1; firstPosition.Y = Position.Y;
                    secondPosition.X = Position.X + 2; secondPosition.Y = Position.Y;
                    break;
                case Direction.West: 
                    firstPosition.X = Position.X - 1; firstPosition.Y = Position.Y;
                    secondPosition.X = Position.X - 2; secondPosition.Y = Position.Y;
                    break;
            }
            path.Add(firstPosition);
            path.Add(secondPosition);
            return path;
        }

        public override void ChangePattrenDirection()
        {
            if (PatternDirection == Direction.East)
            {
                PatternDirection = Direction.West;
            }
            else if (PatternDirection == Direction.West)
            {
                PatternDirection = Direction.East;
            }
        }
    }
}
