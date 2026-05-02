using Mindmagma.Curses;
using Rogue.Application.Abstractions;

namespace Rogue.Console.Rendering;

public sealed class CursesInputProvider : IInputProvider
{
    public GameInput ReadInput()
    {
        var ch = NCurses.GetChar();
        return ch switch
        {
            (int)'w' or (int)'W' => GameInput.Up,
            (int)'s' or (int)'S' => GameInput.Down,
            (int)'a' or (int)'A' => GameInput.Left,
            (int)'d' or (int)'D' => GameInput.Right,
            var c when c == CursesKey.UP => GameInput.Up,
            var c when c == CursesKey.DOWN => GameInput.Down,
            var c when c == CursesKey.LEFT => GameInput.Left,
            var c when c == CursesKey.RIGHT => GameInput.Right,
            (int)'h' or (int)'H' => GameInput.UseWeapon,
            (int)'j' or (int)'J' => GameInput.UseFood,
            (int)'k' or (int)'K' => GameInput.UseElixir,
            (int)'e' or (int)'E' => GameInput.UseScroll,
            (int)'\n' => GameInput.Confirm,
            var c when c == CursesKey.ENTER => GameInput.Confirm,
            13 => GameInput.Confirm,
            (int)'0' => GameInput.Choice0,
            (int)'1' => GameInput.Choice1,
            (int)'2' => GameInput.Choice2,
            (int)'3' => GameInput.Choice3,
            (int)'4' => GameInput.Choice4,
            (int)'5' => GameInput.Choice5,
            (int)'6' => GameInput.Choice6,
            (int)'7' => GameInput.Choice7,
            (int)'8' => GameInput.Choice8,
            (int)'9' => GameInput.Choice9,
            CursesKey.ESC => GameInput.Cancel,
            (int)'q' or (int)'Q' => GameInput.Quit,
            _ => GameInput.None
        };
    }
}
