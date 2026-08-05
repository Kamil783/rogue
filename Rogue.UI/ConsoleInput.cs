using Rogue.Domain;

namespace Rogue.UI
{
    public class ConsoleInput : IInput
    {
        public PlayerCommand ReadInput()
        {
            while (true)
            {
                var key = Console.ReadKey(intercept: true).Key;
                switch (key)
                {
                    case ConsoleKey.UpArrow:
                    case ConsoleKey.W: 
                        return PlayerCommand.Up;
                    case ConsoleKey.DownArrow:
                    case ConsoleKey.S: 
                        return PlayerCommand.Down;
                    case ConsoleKey.LeftArrow:
                    case ConsoleKey.A: 
                        return PlayerCommand.Left;
                    case ConsoleKey.RightArrow:
                    case ConsoleKey.D: 
                        return PlayerCommand.Right;
                    case ConsoleKey.H: 
                        return PlayerCommand.UseWeapon;
                    case ConsoleKey.J: 
                        return PlayerCommand.UseFood;
                    case ConsoleKey.K: 
                        return PlayerCommand.UseElixir;
                    case ConsoleKey.E:
                        return PlayerCommand.UseScroll;
                    case ConsoleKey.Q:
                        return PlayerCommand.Quit;

                    // Number keys (top row and numpad) select an item in the
                    // backpack after an item-use key was pressed.
                    case ConsoleKey.D0:
                    case ConsoleKey.NumPad0:
                        return PlayerCommand.Choice0;
                    case ConsoleKey.D1:
                    case ConsoleKey.NumPad1:
                        return PlayerCommand.Choice1;
                    case ConsoleKey.D2:
                    case ConsoleKey.NumPad2:
                        return PlayerCommand.Choice2;
                    case ConsoleKey.D3:
                    case ConsoleKey.NumPad3:
                        return PlayerCommand.Choice3;
                    case ConsoleKey.D4:
                    case ConsoleKey.NumPad4:
                        return PlayerCommand.Choice4;
                    case ConsoleKey.D5:
                    case ConsoleKey.NumPad5:
                        return PlayerCommand.Choice5;
                    case ConsoleKey.D6:
                    case ConsoleKey.NumPad6:
                        return PlayerCommand.Choice6;
                    case ConsoleKey.D7:
                    case ConsoleKey.NumPad7:
                        return PlayerCommand.Choice7;
                    case ConsoleKey.D8:
                    case ConsoleKey.NumPad8:
                        return PlayerCommand.Choice8;
                    case ConsoleKey.D9:
                    case ConsoleKey.NumPad9:
                        return PlayerCommand.Choice9;
                }
            }
        }
    }
}
