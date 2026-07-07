using System;
using System.Collections.Generic;
using System.ComponentModel;
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
            var newposition = GetNextPosition(Session.Character.Position, direction);
            if (!Session.CurrentLevel.IsWalkable(newposition))
            {
                return;
            }

            if (Session.CurrentLevel.HasEnemyAt(newposition))
            {
                //AddingNewEventArgs fight!!
            }

            if (Session.CurrentLevel.HasItemAt(newposition))
            {
                // function put item to backpack;
            }
            Session.Character.Position = newposition;
        }

        static Position GetNextPosition(Position position, Direction direction)
        {
            int x = 0;
            int y = 0;

            switch (direction)
            {
                case Direction.South: y = -1; break;
                case Direction.North: y = 1; break;
                case Direction.East: x = 1; break;
                case Direction.West: x = -1; break;
            }

            return new Position(position.X + x, position.Y + y);
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
