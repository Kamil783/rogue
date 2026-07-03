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

    //enum ItemSubtype {}
    abstract public class Item
    {
        public ItemType Type { get; set; }
        //ItemSubtype Subtype { get; set; }
        Position Position { get; set; }
        protected Item(int x, int y) 
        {
            Position.X = x;
            Position.Y = y;
        }
    }
}
