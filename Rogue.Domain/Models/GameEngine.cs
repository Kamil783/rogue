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
            MaxLevel = 2; // CHANGE AFTER DEBUGING
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
            if (Session.CurrentLevel.Map[newposition.X, newposition.Y] == CellType.Exit)
            {
                GoToNextLevel();
                return;
            }
            if (!Session.CurrentLevel.IsInside(newposition) || !Session.CurrentLevel.IsWalkable(newposition))
            {
                return;
            }
            var findEnemy = Session.CurrentLevel.HasEnemyAt(newposition);
            if (findEnemy != null)
            {
                CombatProcess(findEnemy);
                ProcessEnemyTurn();
                return;
            }
            var findItem = Session.CurrentLevel.HasItemAt(newposition);
            if (findItem != null)
            {
                Session.Character.CharacterBackpack.AddItem(findItem);
                Session.CurrentLevel.items.Remove(findItem);
            }
            Session.Character.Position = newposition;
            ProcessTemporaryEffects();
            ProcessEnemyTurn();
        }

        static Position GetNextPosition(Position position, Direction direction)
        {
            int x = 0;
            int y = 0;

            switch (direction)
            {
                case Direction.South: y = 1; break;
                case Direction.North: y = -1; break;
                case Direction.East: x = 1; break;
                case Direction.West: x = -1; break;
            }

            return new Position(position.X + x, position.Y + y);
        }
     

        private void GoToNextLevel()
        {
            if(Session.CurrentLevelIndex + 1 > MaxLevel)
            {
                Session.CurrentStatus = Status.Won;
                return;
            }
            LoadLevel(Session.CurrentLevelIndex + 1);
        }

        public void Quit()
        {
            Session.CurrentStatus = Status.Quit;
        }

        private void CombatProcess(Enemy enemy)
        {
            MakeAttack(Session.Character, enemy);
            if (!enemy.IsAlive)
            {
                Session.CurrentLevel.enemies.Remove(enemy);
                Session.CurrentLevel.AddTreasure(enemy.Position, Session.CurrentLevel.Number);
                return;
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

        public void ProcessEnemyTurn()
        {
            foreach(var enemy in Session.CurrentLevel.enemies)
            {
                if (EnemyCanSeeCharacter(enemy))
                {
                    enemy.HasSeenCharacter = true;
                }
                if(CharacterAround(enemy.Position))
                {
                    MakeAttack(enemy, Session.Character);
                    if (!Session.Character.IsAlive)
                    {
                        Session.CurrentStatus = Status.Lost;
                        return;
                    }
                    return;
                }
                else if (enemy.HasSeenCharacter)
                {
                    var path = new List<Position>();
                    path = FindPath(enemy.Position, Session.Character.Position, enemy);
                    if(path != null) enemy.Position = path[0];
                }
                else MoveEnemyByPattern(enemy);
            }
        }

        private bool EnemyCanSeeCharacter(Enemy enemy)
        {
            var distance = Math.Abs(enemy.Position.X - Session.Character.Position.X) + Math.Abs(enemy.Position.Y - Session.Character.Position.Y);
            if (enemy.Hostility >= distance) return true;
            return false;
        }

        private bool CharacterAround(Position position)
        {
            var characterPosition = Session.Character.Position;
            if (characterPosition == new Position(position.X + 1, position.Y) || characterPosition == new Position(position.X - 1, position.Y) || characterPosition == new Position(position.X, position.Y + 1) || characterPosition == new Position(position.X, position.Y - 1)) return true;
                return false;
        }

        private void MoveEnemyByPattern(Enemy enemy)
        {
            var path = enemy.GetPatternMoveCells();
            if (IsCharacterThere(path))
            {
                enemy.OnCombatStarted();
                MakeAttack(enemy, Session.Character);
                if (!Session.Character.IsAlive)
                {
                    Session.CurrentStatus = Status.Lost;
                    return;
                }
                return;
            }
            else if (EnemyCanMoveByPath(path))
            {
                enemy.Position = path[path.Count - 1];
                enemy.SuccessPatternMove();
            }
            else enemy.ChangePattrenDirection();
        }

        private bool EnemyCanMoveByPath(List<Position> path)
        {
            foreach(var position in path)
            {
                if (!Session.CurrentLevel.IsInside(position)) return false;
                if (Session.CurrentLevel.Map[position.X, position.Y] != CellType.Floor) return false;
                foreach(var enemy in Session.CurrentLevel.enemies)
                {
                    if (enemy.Position == position) return false;
                }
            }
            return true;
        }
        private bool IsCharacterThere(List<Position> path)
        {
            foreach(var position in path)
            {
                if (position == Session.Character.Position) return true;
            }
            return false;
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

        public void UseItem(ItemType type, int index)
        {
            switch(type)
            {
                case ItemType.Food: 
                    var food = Session.Character.CharacterBackpack._food[index];
                    Session.Character.IncreaseHealth(food.Health);
                    Session.Character.CharacterBackpack._food.Remove(food);
                    break;
                case ItemType.Scroll:
                    var scroll = Session.Character.CharacterBackpack._scrolls[index];
                    switch (scroll.SubType)
                    {
                        case ItemSubtype.AgilityBoost: Session.Character.IncreaseAgility(scroll.Agility); break;
                        case ItemSubtype.StrengthBoost: Session.Character.IncreaseStrength(scroll.Strength); break;
                        case ItemSubtype.MaxHealthBoost:Session.Character.IncreaseMaxHealth(scroll.MaxHealth); break;
                    }
                    Session.Character.CharacterBackpack._scrolls.Remove(scroll);
                    break;
                case ItemType.Elixir: 
                    var elixir = Session.Character.CharacterBackpack._elixirs[index];
                    switch (elixir.SubType)
                    {
                        case ItemSubtype.AgilityBoost: 
                            Session.Character.IncreaseAgility(elixir.Agility);
                            Session.Character.ActiveEffects.Add(new Character.TemporaryEffect(ItemSubtype.AgilityBoost, elixir.Agility, elixir.TurnsCounter));
                            break;
                        case ItemSubtype.StrengthBoost: 
                            Session.Character.IncreaseStrength(elixir.Strength);
                            Session.Character.ActiveEffects.Add(new Character.TemporaryEffect(ItemSubtype.StrengthBoost, elixir.Strength, elixir.TurnsCounter));
                            break;
                        case ItemSubtype.MaxHealthBoost: 
                            Session.Character.IncreaseMaxHealth(elixir.MaxHealth);
                            Session.Character.ActiveEffects.Add(new Character.TemporaryEffect(ItemSubtype.MaxHealthBoost, elixir.MaxHealth, elixir.TurnsCounter));
                            break;
                    }
                    Session.Character.CharacterBackpack._elixirs.Remove(elixir);
                    break;
                case ItemType.Weapon: 
                    var weapon = Session.Character.CharacterBackpack._weapons[index];
                    if (Session.Character.CurrentWeapon.SubType != ItemSubtype.None)
                    {
                        Session.CurrentLevel.items.Add(Session.Character.CurrentWeapon);
                        Session.Character.CurrentWeapon.Position.X = Session.Character.Position.X + 1; //!
                        Session.Character.CurrentWeapon.Position.Y = Session.Character.Position.Y + 1; // !
                    }
                    Session.Character.GetWeapon(weapon);
                    Session.Character.CharacterBackpack._weapons.Remove(weapon);
                    break;
            }
        }

        public void ProcessTemporaryEffects()
        {
            for (int i = Session.Character.ActiveEffects.Count - 1; i >= 0; i--)
            {
                if (Session.Character.ActiveEffects[i].TurnCounter > 0) Session.Character.ActiveEffects[i].TurnCounter--;
                else
                {
                    switch(Session.Character.ActiveEffects[i].Type)
                    {
                        case ItemSubtype.AgilityBoost: Session.Character.DecreaseAgility(Session.Character.ActiveEffects[i].Value); break;
                        case ItemSubtype.StrengthBoost: Session.Character.DecreaseStrength(Session.Character.ActiveEffects[i].Value); break;
                        case ItemSubtype.MaxHealthBoost: Session.Character.DecreaseMaxHealth(Session.Character.ActiveEffects[i].Value); break;
                    }
                }
            }
        }

        private List<Position> FindPath(Position start, Position target, Enemy movingEnemy)
        {
            var queue = new Queue<Position>();
            var visited = new bool[Session.CurrentLevel.Width, Session.CurrentLevel.Height];
            var previous = new Position[Session.CurrentLevel.Width, Session.CurrentLevel.Height];

            queue.Enqueue(start);
            visited[start.X, start.Y] = true;

            while(queue.Count != 0)
            {
                var current = queue.Dequeue();
                if (current == target) return RestorePath(previous, start, target);
                var neighbors = GetNeighbors(current);
                foreach(var neighbor in neighbors)
                {
                    if (!Session.CurrentLevel.IsInside(neighbor)) continue;
                    if (visited[neighbor.X, neighbor.Y]) continue;
                    if (!Session.CurrentLevel.IsWalkable(neighbor)) continue;
                    if (EnemyInPosition(neighbor)) continue;
                    visited[neighbor.X, neighbor.Y] = true;
                    previous[neighbor.X, neighbor.Y] = current;
                    queue.Enqueue(neighbor);
                }
            }
            return null;
        }

        private bool EnemyInPosition(Position neighbor)
        {
            foreach (var enemy in Session.CurrentLevel.enemies)
            {
                if (enemy.Position == neighbor) return true;
            }
            return false;
        }

        private List<Position> RestorePath(Position[,] previous, Position start, Position target)
        {
            var previousPath = new List<Position>();
            var findingPath = new List<Position>(); 
            var step = target;
            while(step != start)
            {
                previousPath.Add(step);
                step = previous[step.X, step.Y];
                //if (step == null) return null;
            }
            for(int i = previousPath.Count - 1; i > 0; i--)
            {
                findingPath.Add(previousPath[i]);
            }
            return findingPath;
        }

        private List<Position> GetNeighbors(Position current)
        {
            var neighbors = new List<Position>();
            neighbors.Add(new Position(current.X + 1, current.Y));
            neighbors.Add(new Position(current.X - 1, current.Y));
            neighbors.Add(new Position(current.X, current.Y + 1));
            neighbors.Add(new Position(current.X, current.Y - 1));
            return neighbors;
        }
    }
}
