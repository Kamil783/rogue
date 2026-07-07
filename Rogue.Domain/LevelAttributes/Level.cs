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
        public int MaxWeaponOnLevel { get; set; }
        public int MaxEnemiesOnLevel { get; set; }

        public CellType[,] Map; 

        internal List<Room> rooms;
        internal List<Corridor> corridors;
        internal List<Enemy> enemies;
        internal List<Item> items;

        //Add ExitPosition? 
        public Level(int number)
        {
            StartPosition = new Position();
            ExitPosition = new Position();
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
            MaxFoodOnLevel = Number + 3;
            MaxScrollsOnLevel = Number + 3;
            MaxWeaponOnLevel = Number + 3;
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

        public bool IsInside(Position position)
        {
            return true;
        }

        public bool IsWalkable(Position position)
        {
            if (Map[position.X, position.Y] == CellType.Floor || Map[position.X, position.Y] == CellType.Corridor) return true;
            return false;
        }

        public bool HasEnemyAt(Position position)
        {
            foreach (var enemy in enemies)
            {
                if(enemy.Position == position) return true;
            } 
            return false;
        }

        public bool HasItemAt(Position position)
        {
            foreach(var item in items)
            {
                if (item.Position == position) return true;
            }
            return false;
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

        public Treasure AddTreasure(Position position)
        {
            var treasure = new Treasure(position);
            return treasure;
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
                case 0: return new Ghost(position);
                case 1: return new Ogre(position);
                case 2: return new SnakeMage(position);
                case 3: return new Vampire(position);
                case 4: return new Zombie(position);
                default: throw new NotImplementedException("Can't make an enemy");
            }
        }
    }
}
