using Rogue.Domain.LevelAtributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rogue.Domain.Items
{
    public class ItemFactory
    {
        Random random;
        public ItemFactory(Random _random)
        {
            random = _random;
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

        public Elixir AddElixir(Position position)
        {
            int subtypeNumber = 1;
            var elixir = new Elixir(position);
            if (subtypeNumber == 1)
            {
                elixir.SubType = ItemSubtype.AgilityBoost;
            }
            if (subtypeNumber == 2)
            {
                elixir.SubType = ItemSubtype.MaxHealthBoost;
            }
            if (subtypeNumber == 3)
            {
                elixir.SubType = ItemSubtype.StrengthBoost;
            }
            return elixir;
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
    }
}
