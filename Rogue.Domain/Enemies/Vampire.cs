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
        private Direction[] directionarray = {Direction.North, Direction.East, Direction.South, Direction.West};
        private int currentDirectionIndex;
        private int stepsInDirection;
        private int sideLength;

        public Vampire(Position position) : base(position)
        {
            Type = EnemyType.Vampire;
            Health = 65;
            Agility = 65;
            Strength = 40;
            Hostility = 8;
            currentDirectionIndex = 0;
            stepsInDirection = 0;
            sideLength = 2;
        }

        public override Direction GetDirectionOfPatternMove()
        {
            return directionarray[currentDirectionIndex];
        }

        public override void SuccessPatternMove() 
        {
            stepsInDirection++;
            if(stepsInDirection >= sideLength)
            {
                stepsInDirection = 0;
                ChangePattrenDirection();
            }
        }

        public override Position GetNextPosition()
        {
            var position = new Position();
            switch (directionarray[currentDirectionIndex])
            {
                case Direction.South: position.X = Position.X; position.Y = Position.Y - 1; break;
                case Direction.North: position.X = Position.X; position.Y = Position.Y + 1; break;
                case Direction.East: position.X = Position.X + 1; position.Y = Position.Y; break;
                case Direction.West: position.X = Position.X - 1; position.Y = Position.Y; break;
            }
            return position;
        }

        public override void ChangePattrenDirection() 
        {
            stepsInDirection = 0;
            currentDirectionIndex++;
            if( currentDirectionIndex >= directionarray.Length)
            {
                currentDirectionIndex = 0;
            }
        }
    }
}
