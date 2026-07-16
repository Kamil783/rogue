using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Rogue.Domain.Enemies;
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
            if (!Session.CurrentLevel.IsInside(newposition) || !Session.CurrentLevel.IsWalkable(newposition))
            {
                return;
            }
            var findEnemy = Session.CurrentLevel.HasEnemyAt(newposition);
            if (findEnemy != null)
            {
                CombatProcess(findEnemy);
            }
            var findItem = Session.CurrentLevel.HasItemAt(newposition);
            if (findItem != null)
            {
                Session.Character.CharacterBackpack.AddItem(findItem);
                Session.CurrentLevel.items.Add(findItem);
            }
            Session.Character.Position = newposition;
            CheckExit(newposition);
            ProcessEnemyTurn();
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

        private void CheckExit(Position position)
        {
            if (position == Session.CurrentLevel.ExitPosition)
            {
                GoToNextLevel();
            }
            return;
        }

        public void Quit()
        {
            Session.CurrentStatus = Status.Quit;
        }

        private void CombatProcess(Enemy enemy)
        {
            while(enemy.IsAlive && Session.Character.IsAlive)
            {
                MakeAttack(Session.Character, enemy);
                if (!enemy.IsAlive)
                {
                    Session.CurrentLevel.enemies.Remove(enemy);
                    Session.CurrentLevel.AddTreasure(enemy.Position, Session.CurrentLevel.Number);
                    return;
                }
                MakeAttack(enemy, Session.Character);
                if(!Session.Character.IsAlive)
                {
                    Session.CurrentStatus = Status.Lost;
                    return;
                }
            }
        }

        private void MakeAttack(Creature forward, Creature defender)
        {
            if (forward.HitCalculate(defender))
            {
                int damage = DamageCalculate(forward);
                defender.TakeDamage(damage);
            }
            return;
        }

        private int DamageCalculate(Creature forward)
        {
            int damage = 0;
            if (forward is Character)
            {
                var castforward = (Character)forward;
                if (castforward.CurrentWeapon.SubType != ItemSubtype.None)
                damage = castforward.CurrentWeapon.Strength;
            }
            return damage + forward.Strength;
        }

        private void ProcessEnemyTurn()
        {
            foreach(var enemy in Session.CurrentLevel.enemies)
            {
                MoveEnemyByPattern(enemy);
            }
        }

        private void MoveEnemyByPattern(Enemy enemy)
        {
            var direction = enemy.GetDirectionOfPatternMove();
            var newposition = GetNextPosition(enemy.Position, direction);
            if (EnemyCanMove(newposition))
            {
                enemy.Position = newposition;
                enemy.SuccessPatternMove();
            }
            else
            {
                enemy.ChangePattrenDirection();
            }
        }

        private bool EnemyCanMove(Position position)
        {
            if (!Session.CurrentLevel.IsInside(position)) return false;
            if (Session.CurrentLevel.Map[position.X, position.Y] != CellType.Floor) return false;
            foreach(var enemy in Session.CurrentLevel.enemies)
            {
                if(enemy.Position == position) return false;
            }
            return true;
        }
    }
}
