using Rogue.Domain.LevelAtributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rogue.Domain.Models
{
    enum Status { Inprogress, Won, Lost}
    internal class GameSession
    {
        public Character Character { get; protected set; }
        public Level CurrentLevel { get; protected set; }
        public Status CurrentStatus { get; protected set; }
        public bool IsGameOver { get; protected set; }

        public GameSession(Character character, Level currentLevel)
        {
            Character = character; 
            CurrentLevel = currentLevel;
            CurrentStatus = Status.Inprogress;
            IsGameOver = false;
        }
    }
}
