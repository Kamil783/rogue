using Rogue.Domain.LevelAtributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rogue.Domain.Models
{
    public enum Status { InProgress, Won, Lost, Quit}
    public class GameSession
    {
        public Character Character { get; set; }
        public Level CurrentLevel { get; set; }
        internal Status CurrentStatus { get; set; }
        public int CurrentLevelIndex { get; set; }


        public GameSession() 
        {
            Character = new Character(new Position(0, 0));
            CurrentStatus = Status.InProgress;
        }
    }
}
