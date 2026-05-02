using Rogue.Application.Domain.Entities;

namespace Rogue.Application.Domain.Services;

public static class Visibility
{
    public static void Recompute(Level level, Position viewer)
    {
        for (var y = 0; y < level.Height; y++)
        for (var x = 0; x < level.Width; x++)
            level.Map[x, y].Visible = false;

        var room = level.RoomContaining(viewer);
        if (room != null && IsInsideRoomFloor(level, room, viewer))
        {
            for (var y = room.Y; y <= room.Bottom; y++)
            for (var x = room.X; x <= room.Right; x++)
            {
                var p = new Position(x, y);
                level.Map[x, y].Visible = true;
                level.Map[x, y].Discovered = true;
            }
            room.Discovered = true;
        }
        else
        {
            CastRays(level, viewer, 8);
        }
    }

    private static bool IsInsideRoomFloor(Level level, Room room, Position p)
    {
        return room.ContainsInterior(p) && level.Map[p.X, p.Y].Type == TileType.Floor;
    }

    private static void CastRays(Level level, Position viewer, int radius)
    {
        for (var angle = 0; angle < 360; angle += 4)
        {
            var rad = angle * Math.PI / 180.0;
            var tx = viewer.X + (int)Math.Round(Math.Cos(rad) * radius);
            var ty = viewer.Y + (int)Math.Round(Math.Sin(rad) * radius);
            BresenhamLine(level, viewer, new Position(tx, ty));
        }
        if (level.InBounds(viewer))
        {
            level.Map[viewer.X, viewer.Y].Visible = true;
            level.Map[viewer.X, viewer.Y].Discovered = true;
        }
    }

    private static void BresenhamLine(Level level, Position from, Position to)
    {
        var x0 = from.X;
        var y0 = from.Y;
        var x1 = to.X;
        var y1 = to.Y;
        var dx = Math.Abs(x1 - x0);
        var dy = -Math.Abs(y1 - y0);
        var sx = x0 < x1 ? 1 : -1;
        var sy = y0 < y1 ? 1 : -1;
        var err = dx + dy;

        while (true)
        {
            if (!level.InBounds(new Position(x0, y0))) return;
            var tile = level.Map[x0, y0];
            tile.Visible = true;
            tile.Discovered = true;
            if (!tile.IsTransparent) return;
            if (x0 == x1 && y0 == y1) return;
            var e2 = 2 * err;
            if (e2 >= dy) { err += dy; x0 += sx; }
            if (e2 <= dx) { err += dx; y0 += sy; }
        }
    }
}
