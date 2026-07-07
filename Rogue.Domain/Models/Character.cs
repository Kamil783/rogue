using Rogue.Domain.Items;
using Rogue.Domain.LevelAtributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rogue.Domain.Models
{
    public class Character : Creature
    {
        public int MaxHealth { get; set; }
        public ItemType CurrentWeapon { get; set; }
        public Backpack Backpack { get; set; }
        public Character(Position position): base(position)
        {
            Health = 100;
            Strength = 50;
            Agility = 50;
            MaxHealth = 100;
            CurrentWeapon = ItemType.Weapon;
            Backpack = new Backpack();
        }

        void IncreaseHealth(int health)
        {
            if (health < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(health));
            }
            Health = Math.Min(Health + health, MaxHealth);
        }

        void GetWeapon(ItemType weapon)
        {
            CurrentWeapon = weapon;
        }

        void IncreaseMaxHealth(int health)
        {
            MaxHealth += health;
            Health += health;
        }

        void IncreaseAgility(int agility)
        {
            Agility += agility;
        }

        void IncreaseStrength(int strength)
        {
            Strength += strength;
        }
       
    }
}
