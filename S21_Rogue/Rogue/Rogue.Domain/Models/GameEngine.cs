using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Rogue.Domain.Items;
using Rogue.Domain.LevelAtributes;

namespace Rogue.Domain.Models
{
    public class GameEngine
    {
        public int MaxLevel { get; set; }
        public GameSession Session { get; set; }
        public GameEngine(GameSession session)
        { 
            Session = session;
            MaxLevel = 5; // CHANGE AFTER DEBUGING
        }
        public static GameEngine StartNewGame()
        {
            var gamesession = new GameSession();
            var engine = new GameEngine(gamesession);
            engine.LoadLevel(1);
            return engine;
        }

        public void LoadLevel(int levelIndex)
        {
            var level = LevelFactory.CreateLevel(levelIndex);
            Session.CurrentLevel = level;
            Session.CurrentLevelIndex = levelIndex;
            Session.Character.Position = level.StartPosition;

        }

        public void MovePlayer(Direction direction)
        {

        }

        private void GoToNextLevel()
        {

        }

        private void ChackExit()
        {

        }

        public void Quit()
        {
            Session.CurrentStatus = Status.Quit;
        }

    }
}
