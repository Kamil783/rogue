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
                }
            }
        }
    }
}
