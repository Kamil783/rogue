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
        public Weapon CurrentWeapon { get; set; }
        public Backpack CharacterBackpack { get; set; }
        public bool IsSleep { get; set; } = false;
        public List<TemporaryEffect> ActiveEffects { get; set; }
        public Character(Position position): base(position)
        {
            Health = 100;
            Strength = 50;
            Agility = 50;
            MaxHealth = 100;
            CurrentWeapon = new Weapon(new Position(0,0));
            CurrentWeapon.SubType = ItemSubtype.None;
            CharacterBackpack = new Backpack();
            ActiveEffects = new List<TemporaryEffect>();
        }

        public void IncreaseHealth(int health)
        {
            if (health < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(health));
            }
            Health = Math.Min(Health + health, MaxHealth);
        }

        public void GetWeapon(Weapon weapon)
        {
            CurrentWeapon = weapon;
        }

        public void IncreaseMaxHealth(int health)
        {
            MaxHealth += health;
            Health += health;
        }

        public void IncreaseAgility(int agility)
        {
            Agility += agility;
        }

        public void IncreaseStrength(int strength)
        {
            Strength += strength;
        }

        public void DecreaseMaxHealth(int health)
        {
            MaxHealth -= health;
            Health -= health;
            if (Health <= 0) Health = 1;
            if(Health > MaxHealth) Health = MaxHealth;
        }

        public void DecreaseAgility(int agility)
        {
            Agility -= agility;
        }

        public void DecreaseStrength(int strength)
        {
            Strength -= strength;
        }

        public class TemporaryEffect
        {
            public ItemSubtype Type { get; set; }
            public int Value { get; set; }
            public int TurnCounter { get; set; }

            public TemporaryEffect(ItemSubtype type, int value, int turnCounter)
            {
                Type = type;
                Value = value;
                TurnCounter = turnCounter;
            } 
        }
       
    }
}
