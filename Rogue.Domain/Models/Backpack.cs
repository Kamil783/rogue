using Rogue.Domain.Items;
using Rogue.Domain.LevelAtributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rogue.Domain.Models
{
    public class Backpack
    {
        private int Capacity = 9;
        public readonly List<Food> _food;
        public readonly List<Scroll> _scrolls;
        public readonly List<Elixir> _elixirs;
        public readonly List<Weapon> _weapons;
        // Add elexirs after

        int TreasureValue;

        public Backpack()
        {
            TreasureValue = 0;

            _food = new List<Food>();
            _scrolls = new List<Scroll>();
            _elixirs = new List<Elixir>();
            _weapons = new List<Weapon>();
            var NoneWeapon = new Weapon(new Position(-1, -1));
            NoneWeapon.SubType = ItemSubtype.None;
            _weapons.Add(NoneWeapon);
        }

        public void AddItem(Item item) // rewrite to switch case?
        {
            switch (item.Type)
            {
                case ItemType.Food:
                    if (_food.Count < Capacity)
                    {
                        Food itemFood = (Food)item;
                        _food.Add(itemFood);
                    } break;
                case ItemType.Scroll:
                    if (_scrolls.Count < Capacity)
                    {
                        Scroll itemScroll = (Scroll)item;
                        _scrolls.Add(itemScroll);
                    } break;
                case ItemType.Elixir:
                    if (_elixirs.Count < Capacity)
                    {
                        Elixir ItemElixir = (Elixir)item;
                        _elixirs.Add(ItemElixir);
                    } break;
                case ItemType.Weapon:
                    if (_weapons.Count < Capacity)
                    {
                        Weapon itemWeapon = (Weapon)item;
                        _weapons.Add(itemWeapon);
                    } break;
                case ItemType.Treasure:
                    Treasure itemTreasure = (Treasure)item;
                    TreasureValue += itemTreasure.Value;
                    break;
            }
        }

        public void RemoveItem(Item item)
        {
            switch(item.Type)
            {
                case ItemType.Food:
                    Food ItemFood = (Food)item;
                    _food.Remove(ItemFood);
                    break;
                case ItemType.Scroll:
                    Scroll ItemScroll = (Scroll)item;
                    _scrolls.Remove(ItemScroll);
                    break;
                case ItemType.Elixir:
                    Elixir ItemElixir = (Elixir)item;
                    _elixirs.Remove(ItemElixir);
                    break;
                case ItemType.Weapon:
                    Weapon ItemWeapon = (Weapon)item;
                    _weapons.Remove(ItemWeapon);
                    break;
            }
        }
    }
}
