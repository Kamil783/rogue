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
        private int dx = 1;
        private int dy = -1;
        public SnakeMage(Position position) : base(position)
        {
            Type = EnemyType.SnakeMage;
            Health = 75;
            Agility = 75;
            Strength = 40;
            Hostility = 8;
        }

        public override List<Position> GetPatternMoveCells()
        {
            var path = new List<Position>();
            var position = new Position(); 
            position.X = Position.X + dx;
            position.Y = Position.Y + dy;
            path.Add(position);
            return path;
        }
        public override void SuccessPatternMove() 
        {
            dy = -dy;
        }

        public override void ChangePattrenDirection() 
        {
            dx = -dx;
            dy = -dy;
        }
    }
}
