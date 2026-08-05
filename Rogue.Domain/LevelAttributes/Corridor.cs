using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rogue.Domain.LevelAtributes
{
    public class Corridor
    {
       public List<Position> Cells { get; set; }
       
        public Corridor(List<Position> positions) 
        {
            Cells = positions;
        }

    }
}
