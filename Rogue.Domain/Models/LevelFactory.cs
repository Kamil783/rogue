using Rogue.Domain.LevelAtributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
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
            level.ExitPosition = new Position(6, 14);
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
    }
}
