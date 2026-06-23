using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rogue.Domain.LevelAtributes
{
    struct Position
    {
        public int X;
        public int Y;
    }
    internal class Room
    {
        public Position Position {  get; set; }
        public int Height { get; set; }
        public int Width { get; set; }

        public Room() 
        {
            Position = new Position();
            Height = 0;
            Width = 0;
        }

    }
}
