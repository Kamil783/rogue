using Rogue.Application.Domain.Entities;
using Rogue.Application.Domain.Entities.Enemies;
using Rogue.Application.Domain.Entities.Items;

namespace Rogue.Data;

internal static class Mappers
{
    public static SessionDto ToDto(GameSession session) => new()
    {
        Seed = session.Seed,
        CurrentLevelIndex = session.CurrentLevelIndex,
        GameOver = session.GameOver,
        Victory = session.Victory,
        MessageLog = new List<string>(session.MessageLog),
        Player = ToDto(session.Player),
        CurrentLevel = ToDto(session.CurrentLevel)
    };

    public static GameSession FromDto(SessionDto dto)
    {
        var session = new GameSession
        {
            Seed = dto.Seed,
            CurrentLevelIndex = dto.CurrentLevelIndex,
            GameOver = dto.GameOver,
            Victory = dto.Victory
        };
        session.MessageLog.AddRange(dto.MessageLog);
        ApplyPlayerFromDto(dto.Player, session.Player);
        session.CurrentLevel = FromDto(dto.CurrentLevel);
        return session;
    }

    private static PlayerDto ToDto(Player p) => new()
    {
        Name = p.Name,
        X = p.Position.X,
        Y = p.Position.Y,
        Health = p.Health,
        MaxHealth = p.MaxHealth,
        BaseAgility = p.BaseAgility,
        BaseStrength = p.BaseStrength,
        Treasure = p.Treasure,
        SleepTurns = p.SleepTurns,
        EquippedWeapon = p.EquippedWeapon != null ? (WeaponDto)ToItemDto(p.EquippedWeapon) : null,
        Backpack = ToDto(p.Backpack),
        Stats = ToDto(p.Stats),
        Effects = p.Effects.Select(e => new EffectDto
        {
            RemainingTurns = e.RemainingTurns,
            AgilityDelta = e.AgilityDelta,
            StrengthDelta = e.StrengthDelta,
            MaxHealthDelta = e.MaxHealthDelta,
            SourceName = e.SourceName
        }).ToList()
    };

    private static void ApplyPlayerFromDto(PlayerDto dto, Player p)
    {
        p.Name = dto.Name;
        p.Position = new Position(dto.X, dto.Y);
        p.Health = dto.Health;
        p.MaxHealth = dto.MaxHealth;
        p.BaseAgility = dto.BaseAgility;
        p.BaseStrength = dto.BaseStrength;
        p.Treasure = dto.Treasure;
        p.SleepTurns = dto.SleepTurns;
        if (dto.EquippedWeapon != null) p.EquippedWeapon = (Weapon)FromItemDto(dto.EquippedWeapon);
        ApplyBackpackFromDto(dto.Backpack, p.Backpack);
        ApplyStatsFromDto(dto.Stats, p.Stats);
        foreach (var e in dto.Effects)
        {
            p.Effects.Add(new TemporaryEffect
            {
                RemainingTurns = e.RemainingTurns,
                AgilityDelta = e.AgilityDelta,
                StrengthDelta = e.StrengthDelta,
                MaxHealthDelta = e.MaxHealthDelta,
                SourceName = e.SourceName
            });
        }
    }

    private static BackpackDto ToDto(Backpack b) => new()
    {
        TotalTreasure = b.TotalTreasure,
        Weapons = b.Weapons.Select(w => (WeaponDto)ToItemDto(w)).ToList(),
        Foods = b.Foods.Select(ToItemDto).ToList(),
        Elixirs = b.Elixirs.Select(ToItemDto).ToList(),
        Scrolls = b.Scrolls.Select(ToItemDto).ToList()
    };

    private static void ApplyBackpackFromDto(BackpackDto dto, Backpack b)
    {
        b.TotalTreasure = dto.TotalTreasure;
        foreach (var w in dto.Weapons) b.Weapons.Add((Weapon)FromItemDto(w));
        foreach (var f in dto.Foods) b.Foods.Add((Food)FromItemDto(f));
        foreach (var e in dto.Elixirs) b.Elixirs.Add((Elixir)FromItemDto(e));
        foreach (var s in dto.Scrolls) b.Scrolls.Add((Scroll)FromItemDto(s));
    }

    public static StatsDto ToDto(Statistics s) => new()
    {
        TreasureCollected = s.TreasureCollected,
        LevelReached = s.LevelReached,
        EnemiesDefeated = s.EnemiesDefeated,
        FoodEaten = s.FoodEaten,
        ElixirsDrunk = s.ElixirsDrunk,
        ScrollsRead = s.ScrollsRead,
        HitsLanded = s.HitsLanded,
        HitsTaken = s.HitsTaken,
        TilesWalked = s.TilesWalked,
        Date = s.Date,
        Completed = s.Completed
    };

    public static Statistics FromStatsDto(StatsDto d) => new()
    {
        TreasureCollected = d.TreasureCollected,
        LevelReached = d.LevelReached,
        EnemiesDefeated = d.EnemiesDefeated,
        FoodEaten = d.FoodEaten,
        ElixirsDrunk = d.ElixirsDrunk,
        ScrollsRead = d.ScrollsRead,
        HitsLanded = d.HitsLanded,
        HitsTaken = d.HitsTaken,
        TilesWalked = d.TilesWalked,
        Date = d.Date,
        Completed = d.Completed
    };

    private static void ApplyStatsFromDto(StatsDto d, Statistics s)
    {
        s.TreasureCollected = d.TreasureCollected;
        s.LevelReached = d.LevelReached;
        s.EnemiesDefeated = d.EnemiesDefeated;
        s.FoodEaten = d.FoodEaten;
        s.ElixirsDrunk = d.ElixirsDrunk;
        s.ScrollsRead = d.ScrollsRead;
        s.HitsLanded = d.HitsLanded;
        s.HitsTaken = d.HitsTaken;
        s.TilesWalked = d.TilesWalked;
        s.Date = d.Date == default ? DateTime.Now : d.Date;
        s.Completed = d.Completed;
    }

    private static ItemDto ToItemDto(Item item)
    {
        if (item is Weapon w)
        {
            return new WeaponDto
            {
                Id = w.Id, Name = w.Name, Type = (int)w.Type, Subtype = (int)w.Subtype,
                X = w.Position.X, Y = w.Position.Y, HealthBoost = w.HealthBoost, MaxHealthBoost = w.MaxHealthBoost,
                AgilityBoost = w.AgilityBoost, StrengthBoost = w.StrengthBoost, Cost = w.Cost, Damage = w.Damage
            };
        }
        var dto = new ItemDto
        {
            Id = item.Id, Name = item.Name, Type = (int)item.Type, Subtype = (int)item.Subtype,
            X = item.Position.X, Y = item.Position.Y, HealthBoost = item.HealthBoost,
            MaxHealthBoost = item.MaxHealthBoost, AgilityBoost = item.AgilityBoost,
            StrengthBoost = item.StrengthBoost, Cost = item.Cost
        };
        if (item is Elixir e) dto.DurationTurns = e.DurationTurns;
        return dto;
    }

    private static Item FromItemDto(ItemDto dto)
    {
        Item item = ((ItemType)dto.Type) switch
        {
            ItemType.Weapon => new Weapon { Damage = dto is WeaponDto wd ? wd.Damage : 0 },
            ItemType.Food => new Food(),
            ItemType.Elixir => new Elixir { DurationTurns = dto.DurationTurns },
            ItemType.Scroll => new Scroll(),
            ItemType.Treasure => new Treasure(),
            _ => throw new InvalidOperationException()
        };
        item.Id = dto.Id;
        item.Name = dto.Name;
        item.Subtype = (ItemSubtype)dto.Subtype;
        item.Position = new Position(dto.X, dto.Y);
        item.HealthBoost = dto.HealthBoost;
        item.MaxHealthBoost = dto.MaxHealthBoost;
        item.AgilityBoost = dto.AgilityBoost;
        item.StrengthBoost = dto.StrengthBoost;
        item.Cost = dto.Cost;
        return item;
    }

    private static LevelDto ToDto(Level level)
    {
        var tileTypes = new int[level.Width * level.Height];
        var discovered = new bool[level.Width * level.Height];
        for (var y = 0; y < level.Height; y++)
        for (var x = 0; x < level.Width; x++)
        {
            tileTypes[y * level.Width + x] = (int)level.Map[x, y].Type;
            discovered[y * level.Width + x] = level.Map[x, y].Discovered;
        }
        return new LevelDto
        {
            Index = level.Index,
            Width = level.Width,
            Height = level.Height,
            TileTypes = tileTypes,
            Discovered = discovered,
            StartX = level.StartPosition.X, StartY = level.StartPosition.Y,
            ExitX = level.ExitPosition.X, ExitY = level.ExitPosition.Y,
            Rooms = level.Rooms.Select(r => new RoomDto
            {
                Id = r.Id, X = r.X, Y = r.Y, Width = r.Width, Height = r.Height,
                SectionRow = r.SectionRow, SectionCol = r.SectionCol,
                IsStart = r.IsStart, IsExit = r.IsExit, Discovered = r.Discovered
            }).ToList(),
            Enemies = level.Enemies.Select(ToEnemyDto).ToList(),
            Items = level.Items.Select(ToItemDto).ToList()
        };
    }

    private static Level FromDto(LevelDto dto)
    {
        var map = new Tile[dto.Width, dto.Height];
        for (var y = 0; y < dto.Height; y++)
        for (var x = 0; x < dto.Width; x++)
        {
            map[x, y] = new Tile((TileType)dto.TileTypes[y * dto.Width + x])
            {
                Discovered = dto.Discovered[y * dto.Width + x]
            };
        }

        var level = new Level
        {
            Index = dto.Index,
            Width = dto.Width,
            Height = dto.Height,
            Map = map,
            StartPosition = new Position(dto.StartX, dto.StartY),
            ExitPosition = new Position(dto.ExitX, dto.ExitY)
        };

        foreach (var rd in dto.Rooms)
        {
            level.Rooms.Add(new Room
            {
                Id = rd.Id, X = rd.X, Y = rd.Y, Width = rd.Width, Height = rd.Height,
                SectionRow = rd.SectionRow, SectionCol = rd.SectionCol,
                IsStart = rd.IsStart, IsExit = rd.IsExit, Discovered = rd.Discovered
            });
        }
        foreach (var ed in dto.Enemies) level.Enemies.Add(FromEnemyDto(ed));
        foreach (var id in dto.Items) level.Items.Add(FromItemDto(id));
        return level;
    }

    private static EnemyDto ToEnemyDto(Enemy e) => new()
    {
        Kind = (int)e.Kind, X = e.Position.X, Y = e.Position.Y,
        Health = e.Health, MaxHealth = e.MaxHealth,
        BaseAgility = e.BaseAgility, BaseStrength = e.BaseStrength,
        Hostility = e.Hostility, RoomId = e.RoomId,
        FirstHitTaken = e.FirstHitTaken, IsChasing = e.IsChasing, IsInvisible = e.IsInvisible,
        RestTurns = e.RestTurns, InternalState = e.InternalState
    };

    private static Enemy FromEnemyDto(EnemyDto d)
    {
        Enemy e = ((EnemyKind)d.Kind) switch
        {
            EnemyKind.Zombie => new Zombie(),
            EnemyKind.Vampire => new Vampire(),
            EnemyKind.Ghost => new Ghost(),
            EnemyKind.Ogre => new Ogre(),
            EnemyKind.SnakeMage => new SnakeMage(),
            _ => throw new InvalidOperationException()
        };
        e.Position = new Position(d.X, d.Y);
        e.Health = d.Health;
        e.MaxHealth = d.MaxHealth;
        e.BaseAgility = d.BaseAgility;
        e.BaseStrength = d.BaseStrength;
        e.Hostility = d.Hostility;
        e.RoomId = d.RoomId;
        e.FirstHitTaken = d.FirstHitTaken;
        e.IsChasing = d.IsChasing;
        e.IsInvisible = d.IsInvisible;
        e.RestTurns = d.RestTurns;
        e.InternalState = d.InternalState;
        return e;
    }
}
