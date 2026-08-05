using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rogue.Domain.LevelAtributes.Generation
{
    internal class LevelGeometryGenerator
    {
        public Random random;
        public LevelGeometryGenerator(Random random)
        {
            this.random = random;
        }

        public void Generate(Level level)
        {
            GenerateRooms(level);
            GenerateCorridors(level);
            GenerateStartExitPosition(level);
        }

        private void GenerateRooms(Level level)
        {
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    CreateRoom(level, i, j);
                }
            }
        }

        private Room CreateRoom(Level level, int i, int j)
        {
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

        public class RoomConnection
        {
            public int FirstRoomIndex { get; set; }
            public int SecondRoomIndex { get; set; }
            public RoomConnection(int firstRoomIndex, int secondRoomIndex)
            {
                FirstRoomIndex = firstRoomIndex;
                SecondRoomIndex = secondRoomIndex;
            }
        }


        private void GenerateCorridors(Level level)
        {   
            var chosenConnections = FindConnectedRooms();
            foreach (var connection in chosenConnections)
            {
                var firstRoomIndex = connection.FirstRoomIndex;
                var secondRoomIndex = connection.SecondRoomIndex;

                var firstRoomX = firstRoomIndex % 3;
                var firstRoomY = firstRoomIndex / 3;

                var secondRoomX = secondRoomIndex % 3;
                var secondRoomY = secondRoomIndex / 3;

                if (firstRoomY == secondRoomY)
                {
                    var corridorCells = CallHorizontalCorridorBuilder(level.rooms[firstRoomIndex], level.rooms[secondRoomIndex]);
                    level.AddCorridor(corridorCells);
                }

                if (firstRoomX == secondRoomX)
                {
                    var corridorCells = CallVerticalCorridorBuilder(level.rooms[firstRoomIndex], level.rooms[secondRoomIndex]);
                    level.AddCorridor(corridorCells);
                }
            }
        }

        public List<RoomConnection> FindConnectedRooms()
        {
            bool[] connectedRooms = new bool[9];
            int RoomsConnectedCounter = 0;
            List<RoomConnection> chosenConnections = new List<RoomConnection>();

            List<RoomConnection> allPosibleConnections = new List<RoomConnection> { new RoomConnection(0, 1), new RoomConnection(1, 2), new RoomConnection(3, 4), new RoomConnection(4, 5), new RoomConnection(6, 7), new RoomConnection(7, 8), new RoomConnection(0, 3), new RoomConnection(1, 4), new RoomConnection(2, 5), new RoomConnection(3, 6), new RoomConnection(4, 7), new RoomConnection(5, 8) };

            Random random = new Random();
            int wantedCorridors = random.Next(8, 13);
            int startRoomIndex = random.Next(0, 9);
            connectedRooms[startRoomIndex] = true;
            RoomsConnectedCounter += 1;

            while (RoomsConnectedCounter < 9)
            {
                var candidates = new List<RoomConnection>();
                foreach (var conection in allPosibleConnections)
                {
                    if ((connectedRooms[conection.FirstRoomIndex] == false && connectedRooms[conection.SecondRoomIndex] == true) || (connectedRooms[conection.FirstRoomIndex] == true && connectedRooms[conection.SecondRoomIndex] == false))
                    {
                        candidates.Add(conection);
                    }
                }
                int randomConnection = random.Next(candidates.Count);
                chosenConnections.Add(candidates[randomConnection]);
                RoomsConnectedCounter++;
                if (connectedRooms[candidates[randomConnection].FirstRoomIndex] == false)
                {
                    connectedRooms[candidates[randomConnection].FirstRoomIndex] = true;
                }
                else
                {
                    connectedRooms[candidates[randomConnection].SecondRoomIndex] = true;
                }
            }
            while (chosenConnections.Count < wantedCorridors)
            {
                int randomConnection = random.Next(allPosibleConnections.Count);
                bool InCurrentList = false;
                foreach (var conection in chosenConnections)
                {
                    if (conection == allPosibleConnections[randomConnection])
                    {
                        InCurrentList = true;
                    }
                }
                if (!InCurrentList)
                {
                    chosenConnections.Add(allPosibleConnections[randomConnection]);
                }
            }
            return chosenConnections;
        }

        private List<Position> CallVerticalCorridorBuilder(Room firstRoom, Room secondRoom)
        {
            var cells = new List<Position>();
            if (firstRoom.Position.Y > secondRoom.Position.Y)
            {
                cells = BuildVerticalCorridor(secondRoom, firstRoom);
            }
            else
            {
                cells = BuildVerticalCorridor(firstRoom, secondRoom);
            }
            return cells;
        }

        private List<Position> CallHorizontalCorridorBuilder(Room firstRoom, Room secondRoom)
        {
            var cells = new List<Position>();
            if (firstRoom.Position.X > secondRoom.Position.X)
            {
                cells = BuildHorizontalCorridor(secondRoom, firstRoom);
            }
            else
            {
                cells = BuildHorizontalCorridor(firstRoom, secondRoom);
            }
            return cells;
        }

        private List<Position> BuildHorizontalCorridor(Room leftRoom, Room rightRoom)
        {
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

            while (current.X != finish.X)
            {
                if (finish.X > current.X) current.X++;
                else current.X--;

                cells.Add(new Position(current.X, current.Y));
            }

            while (current.Y != finish.Y)
            {
                if (finish.Y > current.Y) current.Y++;
                else current.Y--;
                cells.Add(new Position(current.X, current.Y));
            }
        }

        private List<Position> BuildVerticalCorridor(Room bottomRoom, Room topRoom)
        {
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
                for (int y = yStart; y <= yFinish; y++)
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

        private void GenerateStartExitPosition(Level level)
        {
            level.StartPosition = level.CreateRandomPosition(level.rooms[0], random);
            level.ExitPosition = level.CreateRandomPosition(level.rooms[8], random);
            level.Map[level.ExitPosition.X, level.ExitPosition.Y] = CellType.Exit;
        }
    }
}
