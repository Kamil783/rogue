# CLAUDE.md

This file provides guidance to Claude Code (claude.ai/code) when working with code in this repository.

## Project Overview

**Rogue** is a classic 1980-style Rogue-like dungeon crawler implemented as a cross-platform .NET 8 console game using `dotnet-curses` (PDCurses on Windows / ncurses on Linux/macOS) for rendering. The game has 21 procedurally generated dungeon levels, five enemy types, a backpack/inventory system, fog of war via raycasting, JSON-based save/load, and a leaderboard.

## Architecture

The solution follows clean three-layer architecture with the dependency direction strictly inward (Console + Data → Application; Application has no outward dependencies):

```
Rogue.Console     (Presentation)
   └─ depends on Rogue.Application + Rogue.Data
Rogue.Data        (Data Access — JSON files)
   └─ depends on Rogue.Application
Rogue.Application (Domain + Application services + abstractions)
   └─ no project dependencies
```

### Rogue.Application — Domain & Application

- `Domain/Entities/` — core game entities: `Position`, `Tile`, `Room`, `Corridor`, `Level`, `Character`, `Player`, `Backpack`, `Statistics`, `GameSession`, `TemporaryEffect`.
- `Domain/Entities/Items/` — `Item` (abstract) with `Treasure`, `Food`, `Elixir`, `Scroll`, `Weapon`. Each item knows how to apply itself to a `Character`.
- `Domain/Entities/Enemies/` — `Enemy` (abstract) with `Zombie`, `Vampire`, `Ghost`, `Ogre`, `SnakeMage`. Movement helpers in `EnemyMovement.cs`; A* in `Pathfinder.cs`. Each enemy implements its own `TakeTurn(IGameWorld, Random)` pattern.
- `Domain/Generation/` — `LevelGenerator` builds 3×3 sectioned levels with room-and-corridor topology and validates the room graph is connected. `ItemFactory` produces randomized items and treasure drops.
- `Domain/Services/` — `CombatService` (hit/damage rolls, vampire and snake-mage special effects), `Visibility` (room reveal + Bresenham raycasting through corridors), `GameEngine` (the `IGameWorld` implementation that owns the turn loop).
- `Abstractions/` — `IRenderer`, `IInputProvider`, `ISaveRepository`, `IStatsRepository` — the interfaces the outer layers implement.
- `Application/GameController.cs` — orchestrates the main menu, play loop, inventory prompts, save/quit, and end-of-run statistics persistence.

### Rogue.Data — Persistence

- `SaveDtos.cs` — flat DTO mirror of the domain (Tile arrays flattened to `int[]`, items as discriminated DTOs).
- `Mappers.cs` — pure mapping between domain and DTOs (no reflection on init properties; properties are settable to keep this trivial).
- `JsonSaveRepository` — saves/loads the current `GameSession` to `save/session.json` next to the executable.
- `JsonStatsRepository` — appends per-run `Statistics` to `save/stats.json`; reads top-N sorted by treasure.

### Rogue.Console — Presentation (dotnet-curses)

- `Rendering/CursesRenderer.cs` — implements `IRenderer`. Draws map, items, enemies, player, status bar, and message log. Honors fog of war: tiles must be `Discovered` to draw, and only `Visible` tiles render entities; non-visible discovered walls render in dim color.
- `Rendering/CursesInputProvider.cs` — implements `IInputProvider`. Maps WASD + arrows + h/j/k/e + 0–9 + Enter/Esc/Q to `GameInput`.
- `Rendering/Colors.cs` — color pair initialization for ncurses.
- `Program.cs` — composition root: wires renderer, input, repositories, controller.

### Turn loop (the central flow)

1. `GameController.PlayLoop` calls `_renderer.RenderGame(session)` then `_input.ReadInput()`.
2. WASD invokes `GameEngine.MovePlayer(Direction)` which either attacks an adjacent enemy via `CombatService.PlayerAttack`, walks onto a tile (auto-pickup items), or descends through an exit tile via `AdvanceLevel`.
3. After a successful player action, `GameEngine.EndPlayerTurn` updates each enemy's chase state (Chebyshev distance vs. `Hostility`), invokes `enemy.TakeTurn(this, rng)` (which may call back into `IGameWorld` to attack the player), ticks player effects, and recomputes visibility.
4. Inventory keys (`h`/`j`/`k`/`e`) prompt via `RenderInventoryPrompt` and call `GameEngine.UseItem` / `UnequipWeapon`, both of which end the turn.
5. After every successful turn the controller persists the session via `JsonSaveRepository`. On death/victory the controller appends to `JsonStatsRepository` and deletes the save.

### Generation invariants

- 3×3 grid of "sections" (`SectionsX × SectionsY`); each section gets one room.
- Rooms connected by L-shaped corridors between adjacent sections; door tiles carved at room walls.
- The room graph is validated as connected (BFS); generation retries up to 30× otherwise.
- One random room is `IsStart` (player spawn here, no enemies/items), another is `IsExit` (contains the `>` exit tile).

### Visibility model

- When the player stands on a room floor tile (`ContainsInterior` + floor), the entire room is revealed in one frame.
- Otherwise (corridor / door), Bresenham line-of-sight is cast in 90 directions to radius 8.
- `Tile.Discovered` is sticky (used to draw remembered walls); `Tile.Visible` resets every recompute.

## Common Development Commands

```bash
# Build everything
dotnet build

# Run the game (requires a real terminal — curses won't initialize without a TTY)
dotnet run --project Rogue.Console

# Clean
dotnet clean

# Restore packages
dotnet restore

# Build a single project
dotnet build Rogue.Application
```

Save files live under `<bin>/save/session.json` and `<bin>/save/stats.json`. Delete them to reset state.

## Things to know when changing code

- Entity properties use `{ get; set; }` (not `init`) so the JSON layer can deserialize without reflection tricks. Keep it that way unless you're prepared to update `Mappers.cs`.
- `IGameWorld` is the only contract enemies see — when adding enemy behavior, prefer reading state through `IGameWorld` rather than reaching into `GameEngine` internals.
- Item application logic lives on each `Item` subtype's `ApplyTo`. Add new effects there, not in `GameController` or `GameEngine`.
- Combat formulas: hit chance ≈ `0.45 + 0.5 * (atkAgi / (atkAgi + defAgi))`; player damage uses weapon damage + Strength/3 if equipped, else Strength/2. Adjust in `CombatService`.
- Vampire's "first hit always misses" is tracked by `enemy.FirstHitTaken` (set on the very first player attack against that enemy).
- Snake-mage sleep: 25% on a successful hit, sets `Player.SleepTurns`, consumed by `MovePlayer` before any movement.
- When changing tile types, update `Tile.IsWalkable` / `IsTransparent` and `CursesRenderer.DrawMap` glyph mapping together.
