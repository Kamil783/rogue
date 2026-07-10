using Rogue.Domain.LevelAtributes;
using Rogue.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rogue.Domain.Items
{
    public enum ItemType
    {
        Food, Scroll, Elixir, Treasure, Weapon
    }
    public enum ItemSubtype
    {
        None, Apple, Bred, Chicken, AgilityBoost, StrengthBoost, MaxHealthBoost, Sword, Axe, Dagger, Chopsticks
    }

    //enum ItemSubtype {}
    abstract public class Item
    {
        public ItemType Type { get; set; }
        public ItemSubtype SubType { get; set; }
        public Position Position { get; set; }
        protected Item(Position position) 
        {
            Position = position;
        }
    }
}
