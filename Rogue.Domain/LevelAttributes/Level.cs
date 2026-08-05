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
        public int Number { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }

        public Position StartPosition { get; set; } = new Position();
        public Position ExitPosition { get; set; } = new Position();

        public CellType[,] Map;

        public List<Room> rooms = new List<Room>();
        public List<Corridor> corridors = new List<Corridor>();
        public List<Enemy> enemies = new List<Enemy>();
        public List<Item> items = new List<Item>();

        public Level(int number)
        {
            Number = number;
            Width = 90; //Change to random
            Height = 24; //Change to random
            Map = new CellType[Width, Height];
            for (int i = 0; i < Width; i++)
            {
                for (int j = 0; j < Height; j++)
                {
                    Map[i, j] = CellType.Empty;
                }
            }
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
                    }
                    else
                    {
                        Map[x, y] = CellType.Floor;
                    }
                }
            }
            return room;
        }

        public void AddCorridor(List<Position> positions)
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
            if (position.X >= Width || position.Y >= Height || position.X < 0 || position.Y < 0) return false;
            return true;
        }

        public bool IsWalkable(Position position)
        {
            if (Map[position.X, position.Y] == CellType.Floor || Map[position.X, position.Y] == CellType.Corridor) return true;
            return false;
        }

        public Enemy? HasEnemyAt(Position position)
        {
            foreach (var enemy in enemies)
            {
                if (enemy.Position == position) return enemy;
            }
            return null;
        }

        public Item? HasItemAt(Position position)
        {
            foreach (var item in items)
            {
                if (item.Position == position) return item;
            }
            return null;
        }

        public bool CanPlaceRoom(Position position, int width, int height)
        {
            if (!IsRoomInside(position, width, height))
                return false;
            if (!IsAreaEmpty(position, width, height))
                return false;
            return true;
        }

        private bool IsRoomInside(Position position, int width, int height)
        {
            for (int x = position.X; x < position.X + width - 1; x++)
            {
                if (x >= Width || x < 0) return false;
            }
            for (int y = position.Y; y < position.Y + height - 1; y++)
            {
                if (y >= Height || y < 0) return false;
            }
            return true;
        }

        private bool IsAreaEmpty(Position position, int width, int height)
        {
            for (int x = position.X; x < position.X + width - 1; x++)
            {
                for (int y = position.Y; y < position.Y + height - 1; y++)
                {
                    if (Map[x, y] != CellType.Empty)
                    {
                        return false;
                    }
                }
            }
            return true;
        }

        public Position CreateRandomPosition(Room room, Random random)
        {
            int minY = room.Position.Y + 1;
            int maxY = minY + room.Height - 2;
            int minX = room.Position.X + 1;
            int maxX = minX + room.Width - 2;

            int x = random.Next(minX, maxX);
            int y = random.Next(minY, maxY);
            return new Position(x, y);
        }
        public void AddTreasure(Position position, int value)
        {
            var treasure = new Treasure(position);
            treasure.Value = value;
            items.Add(treasure);
            return;
        }
    }
}
