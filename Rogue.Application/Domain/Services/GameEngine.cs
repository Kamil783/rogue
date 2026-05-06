using Rogue.Application.Domain.Entities;
using Rogue.Application.Domain.Entities.Enemies;
using Rogue.Application.Domain.Entities.Items;
using Rogue.Application.Domain.Generation;

namespace Rogue.Application.Domain.Services;

public sealed class GameEngine : IGameWorld
{
    private readonly GameSession _session;
    private readonly LevelGenerator _generator;
    private readonly CombatService _combat;
    private readonly Random _rng;

    public Level CurrentLevel => _session.CurrentLevel;
    public Player Player => _session.Player;
    public Random Rng => _rng;
    public GameSession Session => _session;

    public GameEngine(GameSession session, Random rng)
    {
        _session = session;
        _rng = rng;
        _generator = new LevelGenerator(rng);
        _combat = new CombatService(rng);
    }

    public static GameEngine StartNew(int? seed = null)
    {
        var actualSeed = seed ?? new Random().Next();
        var rng = new Random(actualSeed);
        var session = new GameSession { Seed = actualSeed, CurrentLevelIndex = 1 };
        var engine = new GameEngine(session, rng);
        engine.LoadLevel(1);
        engine.Log($"Welcome to the dungeon, brave hero!");
        return engine;
    }

    public static GameEngine Resume(GameSession session)
    {
        var rng = new Random(session.Seed + session.CurrentLevelIndex * 31 + session.Player.Stats.TilesWalked);
        var engine = new GameEngine(session, rng);
        return engine;
    }

    public void LoadLevel(int dungeonLevel)
    {
        var level = _generator.Generate(dungeonLevel);
        _session.CurrentLevel = level;
        _session.CurrentLevelIndex = dungeonLevel;
        _session.Player.Position = level.StartPosition;
        _session.Player.Stats.LevelReached = Math.Max(_session.Player.Stats.LevelReached, dungeonLevel);
        Visibility.Recompute(level, _session.Player.Position);
    }

    public void Log(string message) => _session.Log(message);

    public bool IsWalkable(Position position)
    {
        if (!CurrentLevel.InBounds(position)) return false;
        return CurrentLevel.GetTile(position).IsWalkable;
    }

    public bool IsTransparent(Position position)
    {
        if (!CurrentLevel.InBounds(position)) return false;
        return CurrentLevel.GetTile(position).IsTransparent;
    }

    public Enemy? EnemyAt(Position position) => CurrentLevel.Enemies.FirstOrDefault(e => e.Position == position && e.Alive);

    public bool TryAttackPlayer(Enemy enemy)
    {
        return _combat.EnemyAttack(enemy, Player, Log);
    }

    public TurnResult MovePlayer(Direction direction)
    {
        if (_session.GameOver) return TurnResult.GameOver;
        if (Player.SleepTurns > 0)
        {
            Player.SleepTurns--;
            Log("You are asleep.");
            EndPlayerTurn();
            return TurnResult.Acted;
        }

        var (dx, dy) = direction.ToOffset();
        var target = Player.Position.Translate(dx, dy);
        if (!CurrentLevel.InBounds(target)) return TurnResult.Invalid;

        var enemy = EnemyAt(target);
        if (enemy != null)
        {
            _combat.PlayerAttack(Player, enemy, Log);
            if (!enemy.Alive)
            {
                OnEnemyKilled(enemy);
            }
            EndPlayerTurn();
            return TurnResult.Acted;
        }

        if (!IsWalkable(target)) return TurnResult.Invalid;

        Player.Position = target;
        Player.Stats.TilesWalked++;
        TryPickupItems();

        if (CurrentLevel.GetTile(target).Type == TileType.Exit)
        {
            return AdvanceLevel();
        }

        DiscoverRoomIfEntered();
        EndPlayerTurn();
        return TurnResult.Acted;
    }

    private TurnResult AdvanceLevel()
    {
        if (_session.CurrentLevelIndex >= GameSession.TotalLevels)
        {
            _session.Victory = true;
            _session.GameOver = true;
            Log("You have escaped the dungeon! Victory!");
            return TurnResult.Victory;
        }
        Log($"You descend to dungeon level {_session.CurrentLevelIndex + 1}...");
        LoadLevel(_session.CurrentLevelIndex + 1);
        return TurnResult.Acted;
    }

    private void DiscoverRoomIfEntered()
    {
        var room = CurrentLevel.RoomContaining(Player.Position);
        if (room != null) room.Discovered = true;
    }

    private void TryPickupItems()
    {
        var itemsHere = CurrentLevel.Items.Where(i => i.Position == Player.Position).ToList();
        foreach (var item in itemsHere)
        {
            if (Player.Backpack.TryAdd(item))
            {
                CurrentLevel.Items.Remove(item);
                if (item is Treasure t)
                {
                    Player.Treasure += t.Cost;
                    Player.Stats.TreasureCollected += t.Cost;
                    Log($"You pick up {t.Cost} gold.");
                }
                else
                {
                    Log($"You pick up {item.Name}.");
                }
            }
            else
            {
                Log($"Your backpack is full ({item.Name}).");
            }
        }
    }

    private void OnEnemyKilled(Enemy enemy)
    {
        Player.Stats.EnemiesDefeated++;
        var treasure = ItemFactory.CreateTreasure(_rng, _session.CurrentLevelIndex, enemy.Hostility, enemy.Strength, enemy.Agility, enemy.MaxHealth);
        Player.Treasure += treasure.Cost;
        Player.Stats.TreasureCollected += treasure.Cost;
        Player.Backpack.TotalTreasure += treasure.Cost;
        Log($"You slay the {enemy.Kind.ToString().ToLower()} and gain {treasure.Cost} gold!");
        CurrentLevel.Enemies.Remove(enemy);
    }

    public void EndPlayerTurn()
    {
        if (Player.PendingDropWeapon != null)
        {
            DropWeaponNearPlayer(Player.PendingDropWeapon);
            Player.PendingDropWeapon = null;
        }

        UpdateEnemyAggression();
        foreach (var enemy in CurrentLevel.Enemies.ToList())
        {
            if (!enemy.Alive) continue;
            enemy.TakeTurn(this, _rng);
            if (Player.Health <= 0) break;
        }

        Player.TickEffects();

        if (Player.Health <= 0) { _session.GameOver = true; Log("You have died..."); }

        Visibility.Recompute(CurrentLevel, Player.Position);
    }

    private void UpdateEnemyAggression()
    {
        foreach (var enemy in CurrentLevel.Enemies)
        {
            if (!enemy.Alive) continue;
            var distance = enemy.Position.ChebyshevDistance(Player.Position);
            if (distance <= enemy.Hostility) enemy.IsChasing = true;
            else if (distance > enemy.Hostility * 2) enemy.IsChasing = false;
        }
    }

    private void DropWeaponNearPlayer(Weapon weapon)
    {
        foreach (var pos in Player.Position.AllNeighbors())
        {
            if (IsWalkable(pos) && CurrentLevel.Items.All(i => i.Position != pos) && EnemyAt(pos) == null && Player.Position != pos)
            {
                weapon.Position = pos;
                CurrentLevel.Items.Add(weapon);
                Log($"You drop {weapon.Name} on the floor.");
                return;
            }
        }
        weapon.Position = Player.Position;
        CurrentLevel.Items.Add(weapon);
    }

    public void UseItem(Item item)
    {
        item.ApplyTo(Player, Log);
        if (item.Type != ItemType.Weapon)
        {
            Player.Backpack.Remove(item);
        }
        else
        {
            Player.Backpack.Remove(item);
        }
        EndPlayerTurn();
    }

    public void UnequipWeapon()
    {
        var weapon = Player.EquippedWeapon;
        if (weapon == null) return;
        Player.EquippedWeapon = null;
        if (Player.Backpack.Weapons.Count < Backpack.MaxPerType)
        {
            Player.Backpack.Weapons.Add(weapon);
            Log($"You stow {weapon.Name} in your backpack.");
        }
        else
        {
            DropWeaponNearPlayer(weapon);
        }
        EndPlayerTurn();
    }
}
