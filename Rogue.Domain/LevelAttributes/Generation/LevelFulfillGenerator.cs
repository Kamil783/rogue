using Rogue.Domain.Enemies;
using Rogue.Domain.Items;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rogue.Domain.LevelAtributes.Generation
{
    public class LevelFulfillGenerator
    {
        Random random;
        public LevelFulfillGenerator(Random _random)
        {
            random = _random; 
        }

        public void Generate(Level level, LevelSettingsGenerator settings)
        {
            AddItemToList(level, ItemType.Food, settings);
            AddItemToList(level, ItemType.Scroll, settings);
            AddItemToList(level, ItemType.Weapon, settings);
            AddItemToList(level, ItemType.Elixir, settings);
            GenerateEnemy(level, settings);

        }

        private void AddItemToList(Level level, ItemType itemType, LevelSettingsGenerator settings)
        {
            int MaxItemsOnLevel = 0;
            switch (itemType)
            {
                case ItemType.Food: MaxItemsOnLevel = settings.FoodCount; break;
                case ItemType.Scroll: MaxItemsOnLevel = settings.ScrollCount; break;
                case ItemType.Weapon: MaxItemsOnLevel = settings.WeaponCount; break;
                case ItemType.Elixir: MaxItemsOnLevel = settings.ElixirCount; break;
            }

            for (int i = 0; i < MaxItemsOnLevel; i++)
            {
                level.items.Add(CreateItem(level, itemType));
            }
        }

        public Item CreateItem(Level level, ItemType itemType)
        {
            var itemfactory = new ItemFactory(random);
            for (int attempt = 0; attempt < 100; attempt++)
            {
                int RoomIndex = random.Next(1, 9);
                var position = level.CreateRandomPosition(level.rooms[RoomIndex], random);
                if (level.Map[position.X, position.Y] != CellType.Floor)
                {
                    continue;
                }
                switch (itemType)
                {
                    case ItemType.Food: return itemfactory.AddFood(position);
                    case ItemType.Scroll: return itemfactory.AddScroll(position);
                    case ItemType.Weapon: return itemfactory.AddWeapon(position);
                    case ItemType.Elixir: return itemfactory.AddElixir(position);
                }
            }
            throw new InvalidOperationException("Could not generate item after many attempts.");
        }

        private void GenerateEnemy(Level level, LevelSettingsGenerator settings)
        {
            Random random = new Random();
            for (int i = 0; i < settings.EnemyCount; i++)
            {
                for (int attempt = 0; attempt <= 500; attempt++)
                {
                    int RoomIndex = random.Next(1, 9);
                    int EnemyTypeNumber = random.Next(0, 6);
                    var position = level.CreateRandomPosition(level.rooms[RoomIndex], random);
                    if (level.Map[position.X, position.Y] != CellType.Floor)
                    {
                        continue;
                    }
                    var newEnemy = AddEnemy(position, EnemyTypeNumber);
                    level.enemies.Add(newEnemy);

                    if (EnemyTypeNumber == 3)
                    {
                        int NumberOfRandomPositions = random.Next(1, 6);
                        for (int j = 0; j <= NumberOfRandomPositions; j++)
                        {
                            var positionGost = level.CreateRandomPosition(level.rooms[RoomIndex], random);
                            newEnemy.CreatePatternForGhost(positionGost);
                        }
                    }
                    break;
                }
                //throw new InvalidOperationException("Could not generate item after many attempts.");
            }
            return;
        }

        public Enemy AddEnemy(Position position, int EnemyTypeNumber)
        {
            switch (EnemyTypeNumber)
            {
                case 0: return new Zombie(position);
                case 1: return new Vampire(position);
                case 2: return new Ogre(position);
                case 3: return new Ghost(position);
                case 4: return new SnakeMage(position);
                case 5: return new Mimic(position);
                default: throw new NotImplementedException("Can't make an enemy");
            }
        }
    }
}
