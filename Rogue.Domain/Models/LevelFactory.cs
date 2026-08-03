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
            var random = new Random();
            var level = new Level(number);
            Room[,] rooms = new Room[3, 3];
            for (int i  = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    rooms[i, j] = GenerateRoom(level, i, j);
                }  
            }
            
            GenerateCorridors(level);
            level.StartPosition = CreateRandomPosition(level.rooms[0], random);
            level.ExitPosition = CreateRandomPosition(level.rooms[8], random);
            level.Map[level.ExitPosition.X, level.ExitPosition.Y] = CellType.Exit;
            GenerateItems(level);
            GenerateEnemy(level);
            return level;
        }

        private static Room GenerateRoom(Level level, int i, int j)
        {
            Random random = new Random();
            var fieldWidth = level.Width / 3;
            var fieldHieght = level.Height / 3;

            var minWidth = 3;
            var maxWidth = fieldWidth - 2;
            var minHeight = 3;
            var maxHeight = fieldHieght - 2;

            for (int attempt = 0; attempt < 100; attempt++)
            {
                var width = random.Next(minWidth, maxWidth);
                var height = random.Next(minHeight, maxHeight);

                var minXStart = 1 + fieldWidth * j;
                var maxXStart = fieldWidth * (j + 1) - 1 - width;

                var minYStart = 1 + fieldHieght * i;
                var maxYStart = fieldHieght * (i + 1) - 1 - height;

                var XStart = random.Next(minXStart, maxXStart);
                var YStart = random.Next(minYStart, maxYStart);

                if (!level.CanPlaceRoom(new Position(XStart, YStart), width, height))
                {
                    continue;
                }
                return level.AddRoom(width, height, new Position(XStart, YStart));
            }
            throw new InvalidOperationException("Could not generate room after meny attempts.");
        }

        private static void GenerateCorridors(Level level)
        {
            level.FindConnectedRooms();
            foreach(var connection in level.chosenConnections)
            {
                var firstRoomIndex = connection.FirstRoomIndex;
                var secondRoomIndex = connection.SecondRoomIndex;

                var firstRoomX = firstRoomIndex % 3;
                var firstRoomY = firstRoomIndex / 3;

                var secondRoomX = secondRoomIndex % 3;
                var secondRoomY = secondRoomIndex / 3;

                if(firstRoomY ==  secondRoomY)
                {
                    var corridorCells = CallHorizontalCorridorBuilder(level.rooms[firstRoomIndex], level.rooms[secondRoomIndex]);
                    level.AddCorridor(corridorCells);
                }

                if(firstRoomX == secondRoomX)
                {
                    var corridorCells = CallVerticalCorridorBuilder(level.rooms[firstRoomIndex], level.rooms[secondRoomIndex]);
                    level.AddCorridor(corridorCells);
                }
            }
        }

        private static List<Position> CallVerticalCorridorBuilder(Room firstRoom, Room secondRoom)
        {
            var cells = new List<Position>();
            if(firstRoom.Position.Y >  secondRoom.Position.Y)
            {
                cells = BuildVerticalCorridor(secondRoom, firstRoom);
            }
            else
            {
                cells = BuildVerticalCorridor(firstRoom, secondRoom);
            }
            return cells;
        }

        private static List<Position> CallHorizontalCorridorBuilder(Room firstRoom, Room secondRoom)
        {
            var cells = new List<Position>();
            if(firstRoom.Position.X >  secondRoom.Position.X)
            {
                cells = BuildHorizontalCorridor(secondRoom, firstRoom);
            }
            else
            {
                cells = BuildHorizontalCorridor(firstRoom, secondRoom);
            }
            return cells;
        }

        private static List<Position> BuildHorizontalCorridor(Room leftRoom, Room rightRoom)
        {
            Random random = new Random();
            var cells = new List<Position>();

            int startX = leftRoom.Position.X + leftRoom.Width - 1;
            int finishX = rightRoom.Position.X;

            int minYStart = leftRoom.Position.Y + 1;
            int maxYStart = leftRoom.Position.Y + leftRoom.Height - 2;
            int yStart = random.Next(minYStart, maxYStart + 1);

            int minYFinish = rightRoom.Position.Y + 1;
            int maxYFinish = rightRoom.Position.Y + rightRoom.Height - 2;
            int yFinish = random.Next(minYFinish, maxYFinish + 1);

            if (Math.Abs(startX - finishX) == 2)
            {
                for (int x = startX; x <= finishX; x++)
                {
                    cells.Add(new Position(x, yStart));
                }
                return cells;
            }

            int xSwitchDirection = random.Next(startX + 2, finishX - 1);

            AddLine(ref cells, new Position(startX, yStart), new Position(xSwitchDirection, yStart));
            AddLine(ref cells, new Position(xSwitchDirection, yStart), new Position(xSwitchDirection, yFinish));
            AddLine(ref cells, new Position(xSwitchDirection + 1, yFinish), new Position(finishX, yFinish));
            return cells;
        }

        private static void AddLine(ref List<Position> cells, Position start, Position finish)
        {
            var current = start;
            cells.Add(new Position(current.X, current.Y));

            while(current.X != finish.X)
            {
                if (finish.X > current.X) current.X++;
                else current.X--;

                cells.Add(new Position (current.X, current.Y));
            }

            while(current.Y != finish.Y)
            {
                if (finish.Y > current.Y) current.Y++;
                else current.Y--;
                cells.Add(new Position(current.X, current.Y));
            }
        }

        private static List<Position> BuildVerticalCorridor(Room bottomRoom, Room topRoom)
        {
            Random random = new Random();
            var cells = new List<Position>();

            int yStart = bottomRoom.Position.Y + bottomRoom.Height - 1;
            int yFinish = topRoom.Position.Y;

            int minXStart = bottomRoom.Position.X + 1;
            int maxXStart = bottomRoom.Position.X + bottomRoom.Width - 2;
            int xStart = random.Next(minXStart, maxXStart + 1);

            int minXFinish = topRoom.Position.X + 1;
            int maxXFinish = topRoom.Position.X + topRoom.Width - 2;
            int xFinish = random.Next(minXFinish, maxXFinish + 1);

            if (Math.Abs(yStart - yFinish) == 2)
            {
               for(int y = yStart; y <= yFinish; y++)
                {
                    cells.Add(new Position(xStart, y));
                }
                return cells;
            } 

            int ySwitchDirection = random.Next(yStart + 1, yFinish - 1);

            AddLine(ref cells, new Position(xStart, yStart), new Position(xStart, ySwitchDirection));
            AddLine(ref cells, new Position(xStart, ySwitchDirection), new Position(xFinish, ySwitchDirection));
            AddLine(ref cells, new Position(xFinish, ySwitchDirection + 1), new Position(xFinish, yFinish));
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
            switch(itemType)
            {
                case ItemType.Food: MaxItemsOnLevel = level.MaxFoodOnLevel; break;
                case ItemType.Scroll: MaxItemsOnLevel = level.MaxScrollsOnLevel; break;
                case ItemType.Weapon: MaxItemsOnLevel = level.MaxWeaponOnLevel; break;
                case ItemType.Elixir: MaxItemsOnLevel = level.MaxWeaponOnLevel; break;

            }

            for(int i = 0; i < MaxItemsOnLevel; i++)
            {
                level.items.Add(CreateItem(level, itemType));
            }
        }

        static private Item CreateItem(Level level, ItemType itemType)
        {
            Random random = new Random();
            for (int attempt = 0; attempt < 100; attempt++)
            {
                int RoomIndex = random.Next(1, 9);
                var position = CreateRandomPosition(level.rooms[RoomIndex], random);
                if (level.Map[position.X, position.Y] != CellType.Floor)
                {
                    continue;
                }
                switch(itemType)
                {
                    case ItemType.Food: return level.AddFood(position);
                    case ItemType.Scroll: return level.AddScroll(position);
                    case ItemType.Weapon: return level.AddWeapon(position);
                    case ItemType.Elixir: return level.AddElixir(position);
                }
            }
            throw new InvalidOperationException("Could not generate item after many attempts.");
        }

        private static Position CreateRandomPosition(Room room, Random random)
        {
            int minY = room.Position.Y + 1;
            int maxY = minY + room.Height - 2;
            int minX = room.Position.X + 1;
            int maxX = minX + room.Width - 2;

            int x = random.Next(minX, maxX);
            int y = random.Next(minY, maxY);
            return new Position(x, y);
        }

        static void GenerateEnemy(Level level)
        {
            Random random = new Random();
            for (int i = 0; i < level.MaxEnemiesOnLevel; i++)
            {
                for (int attempt = 0; attempt <= 500; attempt++)
                {
                    int RoomIndex = random.Next(1, 9);
                    int EnemyTypeNumber = random.Next(0, 4);
                    var position = CreateRandomPosition(level.rooms[RoomIndex], random);
                    if (level.Map[position.X, position.Y] != CellType.Floor)
                    {
                        continue; 
                    }
                    var newEnemy = level.AddEnemy(position, EnemyTypeNumber);
                    level.enemies.Add(newEnemy);

                    if(EnemyTypeNumber == 3)
                    {
                        int NumberOfRandomPositions = random.Next(1, 6);
                        for (int j = 0; j <= NumberOfRandomPositions; j++)
                        {
                            var positionGost = CreateRandomPosition(level.rooms[RoomIndex], random);
                            newEnemy.CreatePatternForGhost(positionGost);
                        }
                    }
                    break;
                }
                //throw new InvalidOperationException("Could not generate item after many attempts.");
            }
            return;
        }
    }
}
