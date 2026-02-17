# AI Test Game - Claude Code Project Context

## Project Overview
A real-time roguelike deckbuilder built in Unity (C#). Combines Diablo-style isometric action with Slay the Spire card mechanics.

## Architecture

### Core Systems (Assets/Scripts/Core/)
- **GameManager.cs** - Singleton game state manager. Coordinates stages, handles pause/game-over/level-up states.
- **PlayerController.cs** - WASD movement, health, shield management. Tracks facing direction for card attacks.
- **EnergySystem.cs** - Energy resource (starts at 3, regens 1 per 2 seconds). Cards cost energy to play.
- **CameraFollow.cs** - Smooth isometric camera follow.

### Card System (Assets/Scripts/Cards/)
- **CardData.cs** - ScriptableObject defining card properties (name, cost, damage, type, range).
- **CardManager.cs** - Deck, hand, draw pile, discard pile. Auto-draws every 5s. P/O/I/U keys play cards.
- **CardEffectExecutor.cs** - Executes card effects: Slash (180 arc), Block (shield), Whirlwind (360), Dash (lunge).

### Enemy System (Assets/Scripts/Enemies/)
- **EnemyBase.cs** - Base class. Movement toward player, telegraphed attacks, damage, death, XP rewards.
- **BladeZombie.cs** - Weak humanoid (200 HP, 1 XP, 1 spawn point).
- **Ogre.cs** - Rare brute (500 HP, 5 XP, 5 spawn points).

### Spawn System (Assets/Scripts/Spawning/)
- **SpawnManager.cs** - Point-based spawning with ramp and 3 reset points per stage.

### Progression (Assets/Scripts/Progression/)
- **ExperienceSystem.cs** - XP doubles each level (10, 20, 40, 80). Level ups unlock/upgrade cards.

### UI (Assets/Scripts/UI/)
- **GameHUD.cs** - Health, shield, energy, hand, XP, stage progress.
- **LevelUpUI.cs** - Level-up card selection screen.

## Key Design Values
- Base Slash damage: 100
- Blade Zombie HP: 200 (2 slashes to kill)
- Ogre HP: 500 (5 slashes to kill)
- Starting energy: 3, regen 1 per 2s
- Starting deck: 5 Slash + 5 Block + 1 Whirlwind + 1 Dash = 12 cards
- Stage duration: 5 minutes
- Draw interval: 5 seconds
- Max hand size: 10

## Coding Conventions
- C# with Unity conventions (PascalCase public, camelCase private)
- Use [Header] and [Tooltip] attributes
- XML doc comments on public methods
- ScriptableObjects for data definitions
- Enemy types inherit from EnemyBase
- All systems reference through GameManager singleton
