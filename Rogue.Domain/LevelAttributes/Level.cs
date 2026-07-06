using Rogue.Domain.Enemies;
using Rogue.Domain.Items;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rogue.Domain.LevelAtributes
{
    public enum CellType
    {
        Empty, Wall, Floor, Corridor, Exit
    }
    public class Level
    {
        public int Number {  get; set; }
        public int Width { get; set; }
        public int Height { get; set; }

        public Position StartPosition { get; set; }
        public Position ExitPosition { get; set; }


        public CellType[,] Map; 

        internal List<Room> rooms;
        internal List<Corridor> corridors;
        internal List<Enemy> enemies;
        internal List<Item> items;

        //Add ExitPosition? 
        public Level(int number)
        {
            StartPosition = new Position(10, 3); // Change to random
            Number = number;
            Width = 78; //Change to random
            Height = 18; //Change to random
            Map = new CellType[Width, Height];
            for (int i = 0; i < Width; i++)
            {
                for(int j =  0; j < Height; j++)
                {
                    Map[i, j] = CellType.Empty;
                }
            }

            Map[ExitPosition.X, ExitPosition.Y] = CellType.Exit;

            rooms = new List<Room>();
            corridors = new List<Corridor>();
            enemies = new List<Enemy>();   
            items = new List<Item>();
        }

        internal Room AddRoom(int width, int height, Position position)
        {
            var room = new Room(width, height, position);
            rooms.Add(room);
            for (int x = room.Position.X; x < room.Position.X + room.Width; x++)
            {
                for (int y = room.Position.Y; y < room.Position.Y + room.Height; y++)
                {
                    if (x == room.Position.X || y == room.Position.Y || x == room.Position.X + room.Width - 1 || y == room.Position.Y + room.Height - 1)
                    {
                        Map[x, y] = CellType.Wall;
                    } else
                    {
                        Map[x, y] = CellType.Floor;
                    }
                }
            }
            return room;
        }

        public void AddCorridor(List <Position> positions)
        {
            var corridor = new Corridor(positions);
            corridors.Add(corridor);
            foreach (var cell in positions)
            {
                Map[cell.X, cell.Y] = CellType.Corridor;
            }
        } 

        public bool IsInside(Position position)
        {
            return true;
        }

        public bool IsWakable(Position position)
        {
            return true;
        }

        public bool CanPlaceRoom(Position position, int width, int height)
        {
            if (!IsRoomInside(position, width, height))
                return false;
            if(!IsAreaEmpty(position,  width, height))
                return false;
            return true;
        }

        private bool IsRoomInside(Position position, int width, int height)
        {
            return true;
        }

        private bool IsAreaEmpty(Position position, int width, int height)
        {
            return true;
        }
    }
}
