using Rogue.Domain.Items;
using Rogue.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Collections.Specialized.BitVector32;

namespace Rogue.Domain.Application
{
    public class GameRunner
    {
        public IRender _render;
        public IInput _input;

        public GameRunner(IRender render, IInput input)
        {
            _input = input;
            _render = render;
        }  

        public void Run()
        {
            var engine = GameEngine.StartNewGame();
            _render.Initialize(engine);
            Playloop(engine);
        }

        public void Playloop(GameEngine engine)
        {
            while (engine.Session.CurrentStatus == Status.InProgress)
            {
                _render.RenderGame(engine.Session);
                var command = _input.ReadInput();
                ParseInput(engine, command);
            }
        }

        public void ParseInput(GameEngine engine, PlayerCommand command)
        {
            switch (command)
            {
                case PlayerCommand.Up: engine.MovePlayer(Direction.North); break;
                case PlayerCommand.Down: engine.MovePlayer(Direction.South); break;
                case PlayerCommand.Left: engine.MovePlayer(Direction.West); break;
                case PlayerCommand.Right: engine.MovePlayer(Direction.East); break;
                case PlayerCommand.UseElixir: ChooseAndUse(engine, ItemType.Elixir); break;
                case PlayerCommand.UseFood: ChooseAndUse(engine, ItemType.Food); break;
                case PlayerCommand.UseScroll: ChooseAndUse(engine, ItemType.Scroll); break;
                case PlayerCommand.UseWeapon: ChooseAndUse(engine, ItemType.Weapon); break;
                case PlayerCommand.Quit: engine.Quit(); break;
            }
        }

        public void ChooseAndUse(GameEngine engine, ItemType type)
        {
            var key = _input.ReadInput();
            var index = key switch
            {
                PlayerCommand.Choice0 => 0,
                PlayerCommand.Choice1 => 1,
                PlayerCommand.Choice2 => 2,
                PlayerCommand.Choice3 => 3,
                PlayerCommand.Choice4 => 4,
                PlayerCommand.Choice5 => 5,
                PlayerCommand.Choice6 => 6,
                PlayerCommand.Choice7 => 7,
                PlayerCommand.Choice8 => 8,
                PlayerCommand.Choice9 => 9,
                _ => -1
            };
            engine.UseItem(type, index);
            engine.ProcessTemporaryEffects();
            engine.ProcessEnemyTurn();
        }
    }
}
