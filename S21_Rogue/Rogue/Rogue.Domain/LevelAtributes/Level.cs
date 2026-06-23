using Rogue.Domain.Enemies;
using Rogue.Domain.Items;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rogue.Domain.LevelAtributes
{
    enum CellType
    {
        Empty, Wall, Floor, Corridor, Exit
    }
    internal class Level
    {
        public int Number {  get; protected set; }
        public int Width { get; protected set; }
        public int Height { get; protected set; }

        public CellType[,] Map; 

        public List<Room> rooms;
        public List<Corridor> corridors;
        public List<Enemy> enemies;
        public List<Item> items;

        //Add ExitPosition? 
        public Level()
        {
            Number = 1;
            Width = 2000; //Change!
            Height = 1000; //Change!
            Map = new CellType[Width, Height];
            for (int i = 0; i < Width; i++)
            {
                for(int j =  0; j < Height; j++)
                {
                    Map[i, j] = CellType.Empty;
                }
            }

            rooms = new List<Room>();
            corridors = new List<Corridor>();
            enemies = new List<Enemy>();   
            items = new List<Item>();
        }
    }
}
