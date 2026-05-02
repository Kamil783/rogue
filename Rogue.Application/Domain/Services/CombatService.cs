using Rogue.Application.Domain.Entities;
using Rogue.Application.Domain.Entities.Enemies;

namespace Rogue.Application.Domain.Services;

public sealed class CombatService
{
    private readonly Random _rng;

    public CombatService(Random rng)
    {
        _rng = rng;
    }

    public bool PlayerAttack(Player player, Enemy enemy, Action<string> log)
    {
        if (enemy.Kind == EnemyKind.Vampire && !enemy.FirstHitTaken)
        {
            enemy.FirstHitTaken = true;
            log($"You swing at the vampire but miss!");
            return false;
        }

        if (!RollHit(player.Agility, enemy.Agility))
        {
            log("Your attack misses.");
            return false;
        }

        var damage = ComputePlayerDamage(player);
        enemy.Health -= damage;
        player.Stats.HitsLanded++;
        log($"You hit the {enemy.Kind.ToString().ToLower()} for {damage} damage.");
        return true;
    }

    public bool EnemyAttack(Enemy enemy, Player player, Action<string> log)
    {
        if (!RollHit(enemy.Agility, player.Agility))
        {
            log($"The {enemy.Kind.ToString().ToLower()} misses you.");
            return false;
        }

        var damage = Math.Max(1, enemy.Strength + _rng.Next(0, 3));
        player.Health -= damage;
        player.Stats.HitsTaken++;
        enemy.LastDamageDealt = damage;
        log($"The {enemy.Kind.ToString().ToLower()} hits you for {damage} damage.");

        if (enemy.Kind == EnemyKind.Vampire)
        {
            var maxLoss = Math.Min(2, player.MaxHealth - 1);
            if (maxLoss > 0)
            {
                player.MaxHealth -= maxLoss;
                if (player.Health > player.MaxHealth) player.Health = player.MaxHealth;
                log($"The vampire drains {maxLoss} max health!");
            }
        }
        else if (enemy.Kind == EnemyKind.SnakeMage)
        {
            if (_rng.Next(100) < 25)
            {
                player.SleepTurns = Math.Max(player.SleepTurns, 1);
                log("The snake-mage's hiss puts you to sleep!");
            }
        }

        return true;
    }

    public int ComputePlayerDamage(Player player)
    {
        var weapon = player.EquippedWeapon;
        if (weapon == null) return Math.Max(1, player.Strength / 2 + _rng.Next(0, 2));
        return Math.Max(1, weapon.Damage + player.Strength / 3 + _rng.Next(0, 2));
    }

    private bool RollHit(int attackerAgility, int defenderAgility)
    {
        var ratio = (double)attackerAgility / Math.Max(1, attackerAgility + defenderAgility);
        var chance = 0.45 + 0.5 * ratio;
        return _rng.NextDouble() < chance;
    }
}
