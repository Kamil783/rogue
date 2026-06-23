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
        Food, Scroll, Elixir, Treasure
    }

    //enum ItemSubtype {}
    abstract class Item : GameObject
    {
        public ItemType Type { get; set; }
        //ItemSubtype Subtype { get; set; }   
        protected Item() : base() {}
    }
}
