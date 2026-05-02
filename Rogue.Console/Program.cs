using Rogue.Application.Application;
using Rogue.Console.Rendering;
using Rogue.Data;

var renderer = new CursesRenderer();
var input = new CursesInputProvider();
var saveRepo = new JsonSaveRepository();
var statsRepo = new JsonStatsRepository();
var controller = new GameController(renderer, input, saveRepo, statsRepo);
controller.Run();
