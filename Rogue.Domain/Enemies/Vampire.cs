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
            Hostility = 65;
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
                currentDirectionIndex++;
            }
        }

        public override void ChangePattrenDirection() 
        {
            stepsInDirection = 0;
            currentDirectionIndex++;
        }
    }
}
