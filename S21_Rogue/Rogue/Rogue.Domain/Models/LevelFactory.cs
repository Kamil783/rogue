using Rogue.Domain.LevelAtributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rogue.Domain.Models
{
    internal class LevelFactory
    {
        public Level CreateFirstLevel()
        {
            var level = new Level();
            return level;   
        }
    }
}
