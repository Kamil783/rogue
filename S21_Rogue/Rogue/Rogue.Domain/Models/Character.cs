using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rogue.Domain.Models
{
    enum WeaponType
    {
        None,
        Chopsticks,
        Knife,
        Sword,
        Axe,

    }
    internal class Character : Creature
    {
        public int MaxHealth { get; set; }
        public WeaponType CurrentWeapon { get; set; }
        public Character(): base()
        {
            MaxHealth = 100;
            CurrentWeapon = WeaponType.None;
        }

        void IncreseHealth(int health)
        {
            if (health < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(health));
            }
            Health = Math.Min(Health + health, MaxHealth);
        }

        void GetWepon(WeaponType weapon)
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
