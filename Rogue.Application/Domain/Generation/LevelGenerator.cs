using Rogue.Application.Domain.Entities;
using Rogue.Application.Domain.Entities.Enemies;
using Rogue.Application.Domain.Entities.Items;

namespace Rogue.Application.Domain.Generation;

public sealed class LevelGenerator
{
    public const int MapWidth = 78;
    public const int MapHeight = 18;
    public const int SectionsX = 3;
    public const int SectionsY = 3;
    public const int SectionWidth = MapWidth / SectionsX;
    public const int SectionHeight = MapHeight / SectionsY;

    private readonly Random _rng;

    public LevelGenerator(Random rng)
    {
        _rng = rng;
    }

    public Level Generate(int dungeonLevel)
    {
        for (var attempt = 0; attempt < 30; attempt++)
        {
            var level = TryGenerate(dungeonLevel);
            if (level != null) return level;
        }
        throw new InvalidOperationException("Failed to generate a valid level after multiple attempts.");
    }

    private Level? TryGenerate(int dungeonLevel)
    {
        var map = new Tile[MapWidth, MapHeight];
        for (var y = 0; y < MapHeight; y++)
        for (var x = 0; x < MapWidth; x++)
            map[x, y] = new Tile(TileType.Empty);

        var rooms = new List<Room>();
        var nextId = 1;

        for (var sy = 0; sy < SectionsY; sy++)
        for (var sx = 0; sx < SectionsX; sx++)
        {
            var sectionLeft = sx * SectionWidth + 1;
            var sectionTop = sy * SectionHeight + 1;
            var sectionRight = sectionLeft + SectionWidth - 2;
            var sectionBottom = sectionTop + SectionHeight - 2;

            var maxW = Math.Max(5, sectionRight - sectionLeft - 1);
            var maxH = Math.Max(4, sectionBottom - sectionTop - 1);
            var w = _rng.Next(5, Math.Max(6, Math.Min(10, maxW)));
            var h = _rng.Next(4, Math.Max(5, Math.Min(6, maxH)));
            var maxX = Math.Max(sectionLeft + 1, sectionRight - w);
            var maxY = Math.Max(sectionTop + 1, sectionBottom - h);
            var x = _rng.Next(sectionLeft, maxX);
            var y = _rng.Next(sectionTop, maxY);

            var room = new Room
            {
                Id = nextId++,
                X = x,
                Y = y,
                Width = w,
                Height = h,
                SectionRow = sy,
                SectionCol = sx
            };
            rooms.Add(room);
            CarveRoom(map, room);
        }

        var corridors = ConnectRooms(map, rooms);
        if (!IsConnected(rooms, corridors)) return null;

        var startRoom = rooms[_rng.Next(rooms.Count)];
        Room exitRoom;
        do
        {
            exitRoom = rooms[_rng.Next(rooms.Count)];
        } while (exitRoom.Id == startRoom.Id);

        startRoom.IsStart = true;
        startRoom.Discovered = true;
        exitRoom.IsExit = true;

        var startInterior = startRoom.InteriorPositions().ToList();
        var startPos = startInterior[_rng.Next(startInterior.Count)];

        var exitInterior = exitRoom.InteriorPositions().ToList();
        var exitPos = exitInterior[_rng.Next(exitInterior.Count)];
        map[exitPos.X, exitPos.Y].Type = TileType.Exit;

        var level = new Level
        {
            Index = dungeonLevel,
            Width = MapWidth,
            Height = MapHeight,
            Map = map,
            Rooms = rooms,
            Corridors = corridors,
            StartPosition = startPos,
            ExitPosition = exitPos
        };

        PopulateEnemies(level, dungeonLevel, startRoom);
        PopulateItems(level, dungeonLevel, startRoom);

        return level;
    }

    private static void CarveRoom(Tile[,] map, Room room)
    {
        for (var y = room.Y; y <= room.Bottom; y++)
        for (var x = room.X; x <= room.Right; x++)
        {
            var isEdge = x == room.X || x == room.Right || y == room.Y || y == room.Bottom;
            map[x, y].Type = isEdge ? TileType.Wall : TileType.Floor;
        }
    }

    private List<Corridor> ConnectRooms(Tile[,] map, List<Room> rooms)
    {
        var corridors = new List<Corridor>();
        var grid = new Room?[SectionsX, SectionsY];
        foreach (var r in rooms) grid[r.SectionCol, r.SectionRow] = r;

        for (var sy = 0; sy < SectionsY; sy++)
        for (var sx = 0; sx < SectionsX; sx++)
        {
            var current = grid[sx, sy];
            if (current == null) continue;

            if (sx + 1 < SectionsX && grid[sx + 1, sy] != null)
            {
                corridors.Add(CarveCorridor(map, current!, grid[sx + 1, sy]!, horizontal: true));
            }
            if (sy + 1 < SectionsY && grid[sx, sy + 1] != null)
            {
                corridors.Add(CarveCorridor(map, current!, grid[sx, sy + 1]!, horizontal: false));
            }
        }

        if (_rng.Next(100) < 50 && rooms.Count >= 2)
        {
            var a = rooms[_rng.Next(rooms.Count)];
            var b = rooms[_rng.Next(rooms.Count)];
            if (a.Id != b.Id)
            {
                corridors.Add(CarveCorridor(map, a, b, horizontal: _rng.Next(2) == 0));
            }
        }

        return corridors;
    }

    private Corridor CarveCorridor(Tile[,] map, Room a, Room b, bool horizontal)
    {
        Position fromPoint, toPoint;
        if (horizontal)
        {
            fromPoint = a.GetWallExitPoint(Direction.East, _rng);
            toPoint = b.GetWallExitPoint(Direction.West, _rng);
        }
        else
        {
            fromPoint = a.GetWallExitPoint(Direction.South, _rng);
            toPoint = b.GetWallExitPoint(Direction.North, _rng);
        }

        var path = new List<Position> { fromPoint };
        var current = fromPoint;
        var midX = (fromPoint.X + toPoint.X) / 2;
        var midY = (fromPoint.Y + toPoint.Y) / 2;

        if (horizontal)
        {
            while (current.X != midX) { current = current.Translate(Math.Sign(midX - current.X), 0); path.Add(current); }
            while (current.Y != toPoint.Y) { current = current.Translate(0, Math.Sign(toPoint.Y - current.Y)); path.Add(current); }
            while (current.X != toPoint.X) { current = current.Translate(Math.Sign(toPoint.X - current.X), 0); path.Add(current); }
        }
        else
        {
            while (current.Y != midY) { current = current.Translate(0, Math.Sign(midY - current.Y)); path.Add(current); }
            while (current.X != toPoint.X) { current = current.Translate(Math.Sign(toPoint.X - current.X), 0); path.Add(current); }
            while (current.Y != toPoint.Y) { current = current.Translate(0, Math.Sign(toPoint.Y - current.Y)); path.Add(current); }
        }

        foreach (var p in path)
        {
            if (p.X < 0 || p.X >= MapWidth || p.Y < 0 || p.Y >= MapHeight) continue;
            var tile = map[p.X, p.Y];
            if (p == fromPoint || p == toPoint)
            {
                tile.Type = TileType.Door;
            }
            else if (tile.Type is TileType.Empty or TileType.Wall)
            {
                tile.Type = TileType.Corridor;
            }
        }

        return new Corridor { FromRoomId = a.Id, ToRoomId = b.Id, Path = path };
    }

    private static bool IsConnected(List<Room> rooms, List<Corridor> corridors)
    {
        if (rooms.Count == 0) return false;
        var adj = new Dictionary<int, HashSet<int>>();
        foreach (var r in rooms) adj[r.Id] = new HashSet<int>();
        foreach (var c in corridors)
        {
            if (!adj.ContainsKey(c.FromRoomId) || !adj.ContainsKey(c.ToRoomId)) continue;
            adj[c.FromRoomId].Add(c.ToRoomId);
            adj[c.ToRoomId].Add(c.FromRoomId);
        }

        var visited = new HashSet<int>();
        var queue = new Queue<int>();
        queue.Enqueue(rooms[0].Id);
        visited.Add(rooms[0].Id);
        while (queue.Count > 0)
        {
            var current = queue.Dequeue();
            foreach (var nb in adj[current])
                if (visited.Add(nb)) queue.Enqueue(nb);
        }
        return visited.Count == rooms.Count;
    }

    private void PopulateEnemies(Level level, int dungeonLevel, Room startRoom)
    {
        var totalEnemies = Math.Min(2 + dungeonLevel / 2, 12);
        var availableRooms = level.Rooms.Where(r => r.Id != startRoom.Id).ToList();
        for (var i = 0; i < totalEnemies; i++)
        {
            var room = availableRooms[_rng.Next(availableRooms.Count)];
            var positions = room.InteriorPositions()
                .Where(p => level.Map[p.X, p.Y].Type == TileType.Floor && p != level.ExitPosition)
                .ToList();
            if (positions.Count == 0) continue;
            var pos = positions[_rng.Next(positions.Count)];
            if (level.Enemies.Any(e => e.Position == pos)) continue;

            var enemy = SpawnEnemy(dungeonLevel);
            enemy.Position = pos;
            enemy.RoomId = room.Id;
            level.Enemies.Add(enemy);
        }
    }

    private Enemy SpawnEnemy(int dungeonLevel)
    {
        var roll = _rng.Next(100);
        if (dungeonLevel < 3) return Zombie.Create(dungeonLevel);
        if (dungeonLevel < 6) return roll < 60 ? Zombie.Create(dungeonLevel) : Ghost.Create(dungeonLevel);
        if (dungeonLevel < 10) return roll switch
        {
            < 35 => Zombie.Create(dungeonLevel),
            < 60 => Ghost.Create(dungeonLevel),
            < 85 => SnakeMage.Create(dungeonLevel),
            _ => Vampire.Create(dungeonLevel)
        };
        return roll switch
        {
            < 20 => Zombie.Create(dungeonLevel),
            < 40 => Ghost.Create(dungeonLevel),
            < 60 => SnakeMage.Create(dungeonLevel),
            < 80 => Vampire.Create(dungeonLevel),
            _ => Ogre.Create(dungeonLevel)
        };
    }

    private void PopulateItems(Level level, int dungeonLevel, Room startRoom)
    {
        var beneficialItemCount = Math.Max(1, 6 - dungeonLevel / 4);
        var availableRooms = level.Rooms.Where(r => r.Id != startRoom.Id).ToList();

        for (var i = 0; i < beneficialItemCount; i++)
        {
            var room = availableRooms[_rng.Next(availableRooms.Count)];
            var positions = room.InteriorPositions()
                .Where(p => level.Map[p.X, p.Y].Type == TileType.Floor && p != level.ExitPosition)
                .Where(p => level.Enemies.All(e => e.Position != p))
                .Where(p => level.Items.All(it => it.Position != p))
                .ToList();
            if (positions.Count == 0) continue;
            var pos = positions[_rng.Next(positions.Count)];
            var item = ItemFactory.CreateRandomBeneficial(_rng, dungeonLevel);
            item.Position = pos;
            level.Items.Add(item);
        }
    }
}
