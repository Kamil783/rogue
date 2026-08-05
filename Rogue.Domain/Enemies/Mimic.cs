using Rogue.Domain.Items;
using Rogue.Domain.LevelAtributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rogue.Domain.Enemies
{
    public class Mimic: Enemy
    {
        public ItemType LooksLikeItem { get; set; } 
        public Mimic(Position position) : base(position)
        {
            Type = EnemyType.Mimic;
            Health = 65;
            Agility = 65;
            Strength = 5;
            Hostility = 5;
        }
    }
}
