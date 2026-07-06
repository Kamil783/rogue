using Rogue.Domain.Application;
using Rogue.UI;

var render = new ConsoleRender();
var input = new ConsoleInput();
var gamerunner = new GameRunner(render, input);
gamerunner.Run();

Console.ResetColor();
Console.CursorVisible = true;