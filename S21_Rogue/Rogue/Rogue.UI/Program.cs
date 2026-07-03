using Rogue.Domain.Application;
using Rogue.Domain;


var render = new IRender();
var input = new IInput();
var gamerunner = new GameRunner(render, input);
gamerunner.Run();
