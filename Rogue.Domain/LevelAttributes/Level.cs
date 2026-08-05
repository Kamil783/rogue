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

        public int MaxFoodOnLevel {  get; set; }
        public int MaxScrollsOnLevel { get; set; }
        public int MaxElixirsOnLevel { get; set; }
        public int MaxWeaponOnLevel { get; set; }
        public int MaxEnemiesOnLevel { get; set; }

        public bool[] connectedRooms = new bool[9];

        private int RoomsConnectedCounter = 0;

        public CellType[,] Map; 

        internal List<Room> rooms;
        internal List<Corridor> corridors;
        internal List<Enemy> enemies;
        internal List<Item> items;

        public class RoomConnection
        {
            public int FirstRoomIndex { get; set; }
            public int SecondRoomIndex { get; set; }
            public RoomConnection(int firstRoomIndex,  int secondRoomIndex)
            {
                FirstRoomIndex = firstRoomIndex;
                SecondRoomIndex = secondRoomIndex;
            }
        }

        internal List<RoomConnection> chosenConnections = new List<RoomConnection>();
        internal List<RoomConnection> allPosibleConnections = new List<RoomConnection> { new RoomConnection(0, 1), new RoomConnection(1, 2), new RoomConnection(3, 4), new RoomConnection(4, 5), new RoomConnection(6, 7), new RoomConnection(7, 8), new RoomConnection(0, 3), new RoomConnection(1, 4), new RoomConnection(2, 5), new RoomConnection(3, 6), new RoomConnection(4, 7), new RoomConnection(5, 8)};

        public Level(int number)
        {
            StartPosition = new Position();
            ExitPosition = new Position();
            Number = number;
            Width = 90; //Change to random
            Height = 24; //Change to random
            Map = new CellType[Width, Height];
            for (int i = 0; i < Width; i++)
            {
                for(int j =  0; j < Height; j++)
                {
                    Map[i, j] = CellType.Empty;
                }
            }
            MaxFoodOnLevel = Number;
            MaxScrollsOnLevel = Number;
            MaxElixirsOnLevel = Number;
            MaxWeaponOnLevel = Number;
            MaxEnemiesOnLevel = Number + 1;
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

        public void FindConnectedRooms()
        {
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
                } else
                {
                    connectedRooms[candidates[randomConnection].SecondRoomIndex] = true;
                }
            }

            while(chosenConnections.Count < wantedCorridors)
            {
                int randomConnection = random.Next(allPosibleConnections.Count);
                bool InCurrentList = false;
                foreach(var conection in chosenConnections)
                {
                    if(conection == allPosibleConnections[randomConnection])
                    {
                        InCurrentList = true;
                    }
                }
                if(!InCurrentList)
                {
                    chosenConnections.Add(allPosibleConnections[randomConnection]);
                }
            }
        }

        public bool IsInside(Position position)
        {
            if(position.X >= Width || position.Y >= Height || position.X < 0 || position.Y < 0) return false;
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
                if(enemy.Position == position) return enemy;
            } 
            return null;
        }

        public Item? HasItemAt(Position position)
        {
            foreach(var item in items)
            {
                if (item.Position == position) return item;
            }
            return null;
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
            for (int x = position.X; x < position.X + width - 1; x++)
            {
                if(x >= Width || x < 0) return false;
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
                    if (Map[x,y] != CellType.Empty)
                    {
                        return false;
                    }
                }
            }
            return true;
        }

        public Food AddFood(Position position)
        {
            int subtypeNumber = 1;
            var food = new Food(position);
            if (subtypeNumber == 1)
            {
                food.SubType = ItemSubtype.Apple;
            }
            if (subtypeNumber == 2)
            {
                food.SubType = ItemSubtype.Bred;
            }
            if (subtypeNumber == 3)
            {
                food.SubType = ItemSubtype.Chicken;
            }
            return food;
        }

        public Scroll AddScroll(Position position)
        {
            int subtypeNumber = 1;
            var scroll = new Scroll(position);
            if (subtypeNumber == 1)
            {
                scroll.SubType = ItemSubtype.AgilityBoost;
            }
            if (subtypeNumber == 2)
            {
                scroll.SubType = ItemSubtype.MaxHealthBoost;
            }
            if (subtypeNumber == 3)
            {
                scroll.SubType = ItemSubtype.StrengthBoost;
            }
            return scroll;
        }

        public Elixir AddElixir(Position position)
        {
            int subtypeNumber = 1;
            var elixir = new Elixir(position);
            if (subtypeNumber == 1)
            {
                elixir.SubType = ItemSubtype.AgilityBoost;
            }
            if (subtypeNumber == 2)
            {
                elixir.SubType = ItemSubtype.MaxHealthBoost;
            }
            if (subtypeNumber == 3)
            {
                elixir.SubType = ItemSubtype.StrengthBoost;
            }
            return elixir;
        }

        public void AddTreasure(Position position, int value)
        {
            var treasure = new Treasure(position);
            treasure.Value = value;
            items.Add(treasure);
            return;
        }

        public Weapon AddWeapon(Position position)
        {
            int subtypeNumber = 1;
            var weapon = new Weapon(position);
            if (subtypeNumber == 1)
            {
                weapon.SubType = ItemSubtype.Axe;
            }
            if (subtypeNumber == 2)
            {
                weapon.SubType = ItemSubtype.Chopsticks;
            }
            if (subtypeNumber == 3)
            {
                weapon.SubType = ItemSubtype.Dagger;
            }
            if (subtypeNumber == 4)
            {
                weapon.SubType = ItemSubtype.Sword;
            }
            return weapon;
        }

        public Enemy AddEnemy(Position position, int EnemyTypeNumber)
        {
            switch(EnemyTypeNumber)
            {
                case 0: return new Zombie(position);
                case 1: return new Vampire(position);
                case 2: return new Ogre(position);
                case 3: return new Ghost(position);
                case 4: return new SnakeMage(position);
                default: throw new NotImplementedException("Can't make an enemy");
            }
        }
    }
}
