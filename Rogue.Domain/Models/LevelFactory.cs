using Rogue.Domain.Items;
using Rogue.Domain.LevelAtributes;
using Rogue.Domain.LevelAtributes.Generation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Rogue.Domain.Models
{
    internal class LevelFactory
    {
        static public Level CreateLevel(int number)
        {
            var random = new Random();
            var level = new Level(number);
           
            var levelGeometryGenerator = new LevelGeometryGenerator(random);
            levelGeometryGenerator.Generate(level);

            var settings = new LevelSettingsGenerator(level.Number);

            var fillLevel = new LevelFulfillGenerator(random);
            fillLevel.Generate(level, settings);
            return level;
        }
    }
}
