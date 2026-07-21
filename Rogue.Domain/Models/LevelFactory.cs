using Rogue.Domain.Items;
using Rogue.Domain.LevelAtributes;
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
            
            var level = new Level(number);
            Room[,] rooms = new Room[3, 3];
            for (int i  = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    rooms[i, j] = GenerateRoom(level, i, j);
                }  
            }

            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 2; j++)
                {
                    var cellsCoridor = BuildHorizontalCorridor(rooms[i, j], rooms[i, j + 1]);
                    level.AddCorridor(cellsCoridor);
                }
            }

            for(int i = 0; i < 2; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    var cellsCorridor = BuildVerticalCorridor(rooms[i, j], rooms[i + 1, j]);
                    level.AddCorridor(cellsCorridor);
                }
            }
            level.StartPosition.X = 10;
            level.StartPosition.Y = 3;
            level.ExitPosition.X = 64;
            level.ExitPosition.Y = 14;
            level.Map[level.ExitPosition.X, level.ExitPosition.Y] = CellType.Exit;
            GenerateItems(level);
            GenerateEnemy(level);
            return level;
        }

        private static Room GenerateRoom(Level level, int i, int j)
        {
            int startX = 2;
            int startY = 1;

            int gapX = 8;
            int gapY = 1;

            int roomWidth = 16;
            int roomHeight = 5;
            for (int attempt = 0; attempt < 100; attempt++)
            {
                int x = startX + j * (roomWidth + gapX);
                int y = startY + i * (roomHeight + gapY);
                if (!level.CanPlaceRoom(new Position(x, y), roomWidth, roomHeight))
                {
                    continue;
                }
                return level.AddRoom(roomWidth, roomHeight, new Position(x, y));
            }
            throw new InvalidOperationException("Could not generate room after meny attempts.");
        }

        private static List<Position> BuildHorizontalCorridor(Room leftRoom, Room rightRoom)
        {
            var cells = new List<Position>();

            int y = leftRoom.Position.Y + leftRoom.Height / 2;

            int StartX = leftRoom.Position.X + leftRoom.Width - 1;
            int FinishX = rightRoom.Position.X;
            for (int x = StartX; x <= FinishX; x++)
            {
                cells.Add(new Position(x, y));
            }
            return cells;
        }

        private static List<Position> BuildVerticalCorridor(Room bottomRoom,  Room topRoom)
        {
            var cells = new List<Position>();
             int x = bottomRoom.Position.X + bottomRoom.Width / 2;

            int StartY = bottomRoom.Position.Y + bottomRoom.Height - 1;
            int FinishY = topRoom.Position.Y;
            for (int y = StartY; y <= FinishY; y++) {
                cells.Add(new Position(x, y));
            }
            return cells;
        }

        static private void GenerateItems(Level level)
        {
            AddItemToList(level, ItemType.Food);
            AddItemToList(level, ItemType.Scroll);
            AddItemToList(level, ItemType.Weapon);
            AddItemToList(level, ItemType.Elixir);
        }

        static private void AddItemToList(Level level, ItemType itemType)
        {
            int MaxItemsOnLevel = 0;
            int counter = 0;
            switch(itemType)
            {
                case ItemType.Food: MaxItemsOnLevel = level.MaxFoodOnLevel; break;
                case ItemType.Scroll: MaxItemsOnLevel = level.MaxScrollsOnLevel; counter = 6; break;
                case ItemType.Weapon: MaxItemsOnLevel = level.MaxWeaponOnLevel; counter = 2; break;
                case ItemType.Elixir: MaxItemsOnLevel = level.MaxWeaponOnLevel; counter = 1; break;

            }

            for(int i = 0; i < MaxItemsOnLevel; i++)
            {
                int x = counter + 3;
                int y = counter + 2;
                level.items.Add(CreateItem(level, x, y, itemType));
            }
        }

        static private Item CreateItem(Level level, int x, int y, ItemType itemType)
        {
            for (int attempt = 0; attempt < 100; attempt++)
            {
                if (level.Map[x, y] != CellType.Floor)
                {
                    continue;
                }
                switch(itemType)
                {
                    case ItemType.Food: return level.AddFood(new Position(x, y));
                    case ItemType.Scroll: return level.AddScroll(new Position(x, y));
                    case ItemType.Weapon: return level.AddWeapon(new Position(x, y));
                    case ItemType.Elixir: return level.AddElixir(new Position(x, y));
                }
            }
            throw new InvalidOperationException("Could not generate item after many attempts.");
        }

        static void GenerateEnemy(Level level)
        {
            for (int i = 0; i < level.MaxEnemiesOnLevel; i++)
            {
                int EnemyTypeNumber = 1 + i;
                int x = i * 3 + 30;
                int y = i * 6 + 3;
                for (int attempt = 0; attempt <= 100; attempt++)
                {
                    if (level.Map[x, y] != CellType.Floor)
                    {
                        continue; 
                    }
                    level.enemies.Add(level.AddEnemy(new Position(x, y), EnemyTypeNumber)); break;
                }
                //throw new InvalidOperationException("Could not generate item after many attempts.");
            }
            return;
        }
    }
}
