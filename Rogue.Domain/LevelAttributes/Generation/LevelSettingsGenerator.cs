using Rogue.Domain.Items;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rogue.Domain.LevelAtributes.Generation
{
    public class LevelSettingsGenerator
    {
        public int EnemyCount { get; set; }
        public int FoodCount { get; set; }
        public int ScrollCount { get; set; }
        public int ElixirCount { get; set; }
        public int WeaponCount { get; set; }
        public LevelSettingsGenerator(int levelNumber)
        {
            EnemyCount = 2 + Math.Max(1, 2 + levelNumber);
            FoodCount = Math.Max(1, 6 - levelNumber / 4);
            ScrollCount = Math.Max(1, 4 - levelNumber / 7);
            ElixirCount = Math.Max(1, 4 - levelNumber / 7);
            WeaponCount = Math.Max(1, 4 - levelNumber / 10);
        }
    }
}
