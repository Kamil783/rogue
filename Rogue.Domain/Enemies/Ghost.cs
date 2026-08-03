using Rogue.Domain.LevelAtributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rogue.Domain.Enemies
{
    public class Ghost : Enemy
    {
        public List<Position> Path { get; set; }
        public int CurrentPathIndex { get; set; }
        public bool IsVisible { get; set; } = true;
        public bool IsInCombat { get; set; } = false;

        public Ghost(Position position) : base(position)
        {
            Type = EnemyType.Ghost;
            Health = 15;
            Agility = 65;
            Strength = 15;
            Hostility = 5;
            Path = new List<Position>();
            CurrentPathIndex = 0;
        }

        public override void OnCombatStarted()
        {
            IsInCombat = true;
            IsVisible = true;
        }

        public override void SuccessPatternMove() 
        {
            CurrentPathIndex++;
            if (CurrentPathIndex >= Path.Count)
            {
                CurrentPathIndex = 0;
            }
        }

        public override void ChangePattrenDirection() 
        {
            CurrentPathIndex++;
            if(CurrentPathIndex >= Path.Count)
            {
                CurrentPathIndex = 0;
            }
        }

        public override List<Position> GetPatternMoveCells()
        {
            var path = new List<Position>();
            var position = Path[CurrentPathIndex];
            path.Add(position);
            return path;

        }

        public override void CreatePatternForGhost(Position position)
        {
            Path.Add(position);
        }
    }
}
