using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rogue.Domain.LevelAtributes
{
    public class Position
    {
        public int X {  get; set; }
        public int Y { get; set; }

        public Position() {}
        public Position(int x, int y)
        {
            X = x; 
            Y = y;
        }
    }
    internal class Room
    {
        public Position Position {  get; set; }
        public int Height { get; set; }
        public int Width { get; set; }

        public Room(int width, int height, Position position) 
        {
            Position = position;
            Height = height;
            Width = width;
        }
    }
}
