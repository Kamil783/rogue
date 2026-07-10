using Rogue.Domain.Items;
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
        private readonly List<Food> _food;
        private readonly List<Scroll> _scrolls;
        // Add elexirs after

        int TreasureValue;

        public Backpack()
        {
            TreasureValue = 0;

            _food = new List<Food>();
            _scrolls = new List<Scroll>();
        }

        public void AddItem(Item item) // rewrite to switch case?
        {
            if (item.Type == ItemType.Treasure)
            {
                Treasure itemTreasure = (Treasure)item;
                TreasureValue += itemTreasure.Value;
            }
            else if (item.Type == ItemType.Food) 
            { 
                if (_food.Count < Capacity)
                {
                    Food itemFood = (Food)item;
                    _food.Add(itemFood);
                }
            }
            else if (item.Type == ItemType.Scroll)
            {
                if(_scrolls.Count < Capacity)
                {
                    Scroll itemScroll = (Scroll)item;
                    _scrolls.Add(itemScroll);
                }
            }
            else if (item.Type == ItemType.Elixir)
            {
                //if(_elixirs.Count < Capacity)
                //{
                //add after make elixirs
                //}
            }
        }

        public void RemoveItem(Item item)
        {
            if (item.Type == ItemType.Food)
            {
                Food ItemFood = (Food)item;
                _food.Remove(ItemFood);
                //for (int i = 0; i < _food.Count; i++)
                //{
                //    if(ItemFood ==  _food[i])
                //    {
                //        _food.RemoveAt(i); break;
                //    }
                //}
            }
            else if (item.Type == ItemType.Scroll)
            {
                Scroll ItemScroll = (Scroll)item;
                for (int i = 0; i < _scrolls.Count; i++)
                {
                    if (ItemScroll == _scrolls[i])
                    {
                        _scrolls.RemoveAt(i); break;
                    }
                }
            }
            //else if - add elexirs and weapon
        }

    }
}
