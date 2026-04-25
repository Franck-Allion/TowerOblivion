---
title: 'Game Architecture'
project: 'IA'
date: '2026-04-25'
author: 'Franck'
version: '1.0'
stepsCompleted: [1, 2, 3, 4, 5, 6, 7, 8, 9]
status: 'complete'
engine: 'Unity 6.3 LTS'
platform: 'PC / Steam'

# Source Documents
gdd: 'D:\Dev\Unity\TowerOblivion\_bmad-output\gdd.md'
epics: 'D:\Dev\Unity\TowerOblivion\_bmad-output\epics.md'
brief: 'D:\Dev\Unity\TowerOblivion\_bmad-output\game-brief.md'
---

# Game Architecture

## Executive Summary

**Tower Oblivion** architecture is designed for **Unity 6.3 LTS** targeting **PC / Steam**.

**Key Architectural Decisions:**

- Unity-free gameplay layer for deterministic 4x4 Souvenir combat, run generation, progression, narrative flags, and save-facing state.
- Unity-facing presentation, input, infrastructure, and editor code isolated behind mandatory asmdef boundaries.
- Local versioned JSON save system with validation, backup recovery, migration hooks, and deterministic replay/debug support.
- uGUI runtime UI baseline for combat board, HUD, menus, and gamepad navigation.
- ScriptableObject authoring converted into validated immutable runtime catalogs before gameplay systems consume content.

**Project Structure:** Hybrid Unity organization with mandatory assembly boundaries and 14 core systems mapped to explicit locations.

**Implementation Patterns:** 9 implementation contracts defined for assembly dependencies, gameplay purity, presentation adapters, infrastructure boundaries, mode orchestration, catalogs, narrative flags, events, saves, ownership, and testing.

**Ready for:** implementation planning, story creation, Unity project setup, and AI-assisted development.

## Document Status

This architecture document was created through the GDS Architecture Workflow.

**Steps Completed:** 9 of 9 (Complete)

---

## Architecture Summary

Tower Oblivion uses Unity 6.3 LTS with a hybrid Unity project structure, mandatory asmdef boundaries, and a Unity-free gameplay layer for deterministic combat, run generation, narrative flags, progression, and save-facing state. Unity-facing code is isolated in presentation, input, and infrastructure layers so uGUI, scene loading, ScriptableObjects, local files, and Steam achievements do not leak into core gameplay rules.

The architecture prioritizes deterministic replay/debuggability, local versioned save integrity, data-driven content validation, and a reusable foundation for future point-and-click narrative games with turn-based board combat.

## Project Context

### Game Overview

**Tower Oblivion** is a PC-first heroic-fantasy roguelite investigation game built around repeated tower ascents, point-and-click room exploration, persistent memory/progression, and deterministic 4x4 Souvenir board combat.

The MVP must validate the link between discovery, Souvenir acquisition, board-based combat decisions, death/rebirth, and retained progression.

### Technical Scope

**Platform:** PC / Steam  
**Engine:** Unity  
**Genre:** Roguelite / point-and-click adventure / tactical board combat hybrid  
**Project Level:** Medium-high complexity solo MVP  
**Networking:** None for MVP  
**Save Model:** Local, versioned, validated, recovery-capable

### Core Systems

| System | Complexity | Architecture Need |
|---|---|---|
| GameFoundation | High | Lifecycle, service registration/access, config loading, event routing, and screen flow only |
| GameStateOrchestrator | High | Owns mode transitions and validates state mutation flow |
| RoomSystem | Medium | Fixed-room point-and-click scenes, room definitions, transitions, room state |
| InteractionSystem | Medium | Hotspots, exits, object inspection, command/event emission |
| NarrativeState / Flag System | Medium-High | Persistent flags, clue/memory unlocks, bounded consequence tracking |
| CombatSystem / CombatEngine | High | Deterministic 4x4 board model, Souvenir data, placement rules, trigger resolution, AI hooks |
| ProgressionSystem | Medium | XP, meta-currency, upgrade tiers, skills, Souvenir unlocks, death conversion |
| RunGenerator | Medium | Fixed floor sequence with seeded randomized encounters, rewards, modifiers |
| SaveSystem | High | Versioned snapshots, migrations, atomic writes, validation, recovery |
| ContentDataSystem | Medium | Data-driven definitions for rooms, encounters, Souvenirs, rewards, flags, modifiers |
| ContentValidationPipeline | High | Validates references, assets, progression gates, encounters, saves, and provenance |
| UI/Input System | Medium | Unity Input System action maps, mouse/keyboard + gamepad, context switching |
| Asset Pipeline | Medium | Room asset contract, import presets, memory budgets, provenance tracking |
| PlatformServices | Low-Medium | Steam achievements behind interfaces, local test doubles |

### Technical Requirements

- 60 FPS target at 1080p on mid-tier PC hardware.
- 30 FPS minimum acceptable.
- Mouse + keyboard and gamepad support.
- Steam achievements for MVP.
- No online dependency.
- Local save only for MVP.
- Derived combat stats recomputed on load.
- Core modules must avoid hard Tower-specific dependencies where practical.
- Content should be data-driven for rooms, encounters, Souvenirs, rewards, narrative events, and modifiers.
- Asset usage must be provenance-tracked for commercial release.
- Static authored data may use ScriptableObjects, but mutable runtime state must not live inside ScriptableObject assets.
- Combat logic should be testable in plain C# outside Unity scene presentation.

### Ownership and State Boundaries

- `GameFoundation` owns shared lifecycle, service access, config loading, event routing, and screen flow primitives only.
- `GameStateOrchestrator` owns high-level mode transitions such as room exploration -> dialogue -> combat -> reward -> room update.
- `RunState` owns the current procedural run, current floor/room context, active modifiers, and transient run values.
- `CombatState` owns isolated board state, turn order, placed Souvenirs, pending effects, and combat result.
- `MetaState` / `PlayerProfile` owns persistent unlocks, settings, upgrade tiers, Souvenir ownership, memory progress, and currencies.
- `WorldSeedState` owns deterministic seed/config inputs for room generation, encounters, rewards, and narrative variation.
- `NarrativeState` owns flags and narrative variables; other systems query or request mutations through controlled APIs/events.
- `ProgressionSystem` consumes combat, room, and narrative events rather than directly mutating unrelated state.
- `PlatformServices` owns Steam APIs behind interfaces so the MVP can run without the Steam runtime.

### Complexity Drivers

**High Complexity**
- Deterministic 4x4 Souvenir combat with trigger effects, replay/debug support, and AI hooks.
- Save integrity across run state, permanent progression, narrative flags, unlocks, and future schema changes.
- Game state orchestration across room, dialogue, combat, reward, death/rebirth, and hub flows.
- Content validation for data-driven rooms, encounters, rewards, Souvenirs, flags, assets, and progression gates.
- Reusable foundation boundaries without overbuilding a generic engine.

**Novel Concepts**
- Souvenirs as both narrative memory fragments and combat cards.
- Combat bonus requirements tied to upgrade tiers rather than hard stat locks.
- Death/rebirth loop preserving memory, clues, skills, and bounded consequences.
- Hybrid point-and-click room flow feeding turn-based board combat.
- Future-game reusability requirement constrained to module boundaries and data-driven content.

### Technical Risks

- Combat readability degrades as effects stack.
- Save corruption or migration errors damage player trust.
- Reusable engine ambition expands beyond MVP needs.
- Narrative flags become too coupled to scenes, combat, or progression.
- Unity scene scripts directly touching save, flag, progression, or combat internals.
- Asset licensing/provenance gaps block commercial release.
- Solo part-time scope expands through content and system overlap.
- Data-driven content bugs remain hidden without validation tooling.
- Seeded run generation becomes unreproducible without config versioning and decision logs.

### Testability and Quality Gates

- **Save integrity:** schema versioning, atomic write strategy, rollback/recovery, corruption detection, migration tests, malformed-save tests, and interrupted-save tests.
- **Combat determinism:** same seed/config/action log must produce identical board states, effects, rewards, and outcomes.
- **Run generation determinism:** run generation should be snapshot-testable from seed, build/content config version, and generation settings.
- **Narrative/progression integrity:** flags must be traceable to triggers, save/load persistence, unlock conditions, and mutually exclusive branches.
- **Asset provenance:** missing or unknown provenance should block release-candidate status.
- **Performance profiling:** test worst-case combat effects, room transitions, inventory/progression UI, and save/load moments.
- **Steam achievements:** platform calls must sit behind an interface with local test doubles.

### Debug and Inspectability Requirements

The MVP should expose enough development-only tooling to reproduce failures quickly:
- current run seed
- content/config version
- decision/action log for combat and run flow
- combat state dump
- final state hash for deterministic checks
- save schema version and validation result
- flag inspection/reset tools
- save reset/export tools

## Engine & Framework

### Selected Engine

**Unity 6.3 LTS**

**Rationale:** PC/Steam target, 2D fixed-room exploration, UI-heavy point-and-click flow, deterministic C# combat logic, local save, achievements, and reusable engine boundaries. Unity 6.3 LTS is the current LTS target for this architecture and should be locked to an exact installed patch version in `ProjectVersion.txt`.

### Project Initialization

Clean Unity 6.3 LTS project. No third-party gameplay starter.

Baseline:

- 2D project setup
- URP 2D selected now for lighting overlays, glow, divine effects, silhouettes, and layered room presentation
- Unity Input System mandatory
- Unity Test Framework mandatory
- TextMeshPro baseline
- Assembly Definitions from the start
- MCP Unity + Context7 optional and dev-only

### Engine-Provided Architecture

| Component | Solution | Notes |
|---|---|---|
| Rendering | Unity 2D + URP 2D | Supports layered rooms, glow, divine overlays, and stylized 2D presentation |
| Audio | Unity Audio | Sufficient for MVP SFX/music routing |
| Input | Unity Input System | Required for mouse/keyboard + gamepad |
| UI | uGUI | Runtime UI baseline for MVP combat board, HUD, menus, and gamepad navigation |
| Scene Management | Unity SceneManager | Boring bootstrap scene plus controlled content scenes |
| Data Assets | ScriptableObjects | Static authored data only; mutable runtime state must not live in ScriptableObjects |
| Testing | Unity Test Framework | Required from start |
| Build System | Unity Build Pipeline | Steam build automation can be added later |
| Platform Integration | Steam behind `PlatformServices` | Achievements must be abstracted behind interfaces |

### Starter Template Decision

No third-party starter template.

Reason: Tower Oblivion needs strict ownership boundaries around combat, save, narrative flags, run state, and reusable engine modules. A starter would likely introduce hidden assumptions and coupling.

### Package Baseline

- Unity Input System
- Unity Test Framework
- TextMeshPro
- URP 2D
- Optional Cinemachine only if camera behavior requires it
- No legacy input
- MCP Unity / Context7 optional and dev-only, not runtime or CI dependencies

### Assembly Definition Baseline

Initial asmdef boundaries:

- `Core`
- `Gameplay`
- `Presentation`
- `Input`
- `Infrastructure`
- `Editor`
- `Tests`

Rule: keep feature splits minimal until coupling or compile pressure justifies more assemblies.

### Engine Rules

- Gameplay/domain logic should not live in MonoBehaviours by default.
- MonoBehaviours adapt Unity events, presentation, and input to domain services.
- Combat core must run in EditMode tests without scenes.
- Randomness must use injectable seeded RNG; do not call `UnityEngine.Random` directly in core logic.
- Commit `.meta` files.
- Ignore generated/cache folders.
- Exact Unity patch version must be locked in `ProjectVersion.txt` once installed.
- CI/editor setup must use the same Unity version.

### AI Development Tools

Optional dev-only tools:

- **MCP Unity:** `CoderGamester/mcp-unity`, for AI/editor integration with Unity.
- **Context7:** `upstash/context7`, for current documentation lookup by AI tools.

## Development Environment

### Prerequisites

- Unity Hub with Unity 6.3 LTS installed.
- Exact installed Unity patch version locked in `ProjectSettings/ProjectVersion.txt`.
- Unity modules for Windows PC build support.
- Git with Unity `.meta` files committed.
- C# IDE/editor of choice.
- Steamworks SDK or Steamworks.NET later in implementation, only when Steam achievement integration begins.
- Optional dev-only MCP tools: MCP Unity and Context7.

### Project Initialization

Create a clean Unity project from Unity Hub:

```text
Unity Hub -> New Project -> 2D (URP) -> TowerOblivion
```

Initial setup actions:

1. Enable URP 2D.
2. Install/enable Unity Input System.
3. Enable Unity Test Framework.
4. Add TextMeshPro.
5. Create mandatory asmdefs: `TowerOblivion.Core`, `TowerOblivion.Gameplay`, `TowerOblivion.Presentation`, `TowerOblivion.Infrastructure`, `TowerOblivion.Input`, `TowerOblivion.Editor`, `TowerOblivion.Tests.EditMode`, `TowerOblivion.Tests.PlayMode`.
6. Create the folder structure from the Project Structure section.
7. Add `.gitignore` for Unity generated/cache folders while preserving `.meta` files.

### AI Tooling

The architecture allows optional dev-only AI tooling:

| MCP Server | Purpose | Install Type |
|---|---|---|
| MCP Unity | AI/editor integration for Unity scene and asset inspection | Dev-only |
| Context7 | Current documentation lookup for AI tools | Dev-only |

These tools must not become runtime, build, CI, or gameplay dependencies.

### First Implementation Steps

1. Create the Unity project and folder/asmdef structure.
2. Implement `TowerOblivion.Core` primitives: `Result`, IDs, `IRng`, `IClock`, `IEventBus`, logging interfaces.
3. Implement the first EditMode tests proving `Gameplay` has no Unity dependency.
4. Implement local save snapshot skeleton and content catalog skeleton.
5. Build Epic 1 foundation before expanding gameplay content.

## Architectural Decisions

### Decision Summary

| Category | Decision | Version | Rationale |
|---|---|---|---|
| State Management | Explicit game mode state machine + plain C# serializable state models | N/A | Keeps flow controlled while preventing state ownership from collapsing into a god object |
| Save System | Local versioned JSON snapshots with temp-write, validation, backup, and migration hooks | N/A | Supports offline Steam MVP, recovery, deterministic debugging, and future schema changes |
| Scene Structure | Boring bootstrap scene + controlled content scenes | N/A | Keeps initialization stable without turning the bootstrap scene into gameplay logic |
| Combat Architecture | Unity-free deterministic C# combat assembly + Unity presentation adapter | N/A | Enables EditMode tests, replay/debug logs, AI simulation, and reuse in future games |
| Content Data | ScriptableObject assets as editor/content inputs; validated immutable DTOs at runtime | N/A | Preserves Unity authoring workflow while preventing mutable asset/save bugs |
| RNG | Injectable seeded PRNG service | N/A | Required for reproducible runs, combat tests, reward generation, and bug reports |
| UI Framework | uGUI for MVP runtime UI | N/A | Pragmatic choice for combat board interaction, gamepad navigation, HUD, and fast iteration |
| Asset Loading | Direct references + scene-based loading for MVP | N/A | Simpler than Addressables and sufficient for first chapter scope |
| Narrative System | Typed flag registry + narrative event log API | N/A | Enables bounded choices and future-run consequences without raw string flag decay |
| Enemy AI | Deterministic heuristic board AI with stable tie-breaking | N/A | Keeps enemy behavior testable and reproducible |
| Audio | Unity Audio + mixer groups | N/A | Sufficient for MVP music, SFX, ambience, UI, and combat readability |
| Platform Services | Steam achievements behind `IPlatformServices` with local intent tracking | N/A | Prevents Steam dependency leakage and avoids losing offline achievement unlocks |

### State Management

**Approach:** Explicit game mode state machine with plain C# serializable state models owned by one service each.

Core contracts:

- `GameModeId`
- `IGameModeState`
- `GameModeController`
- `IGameStateOrchestrator`

The `GameStateOrchestrator` controls high-level transitions only:

`Boot -> MainMenu -> RebirthHub -> RunExploration -> Dialogue/Event -> Combat -> Reward -> FloorTransition -> Death/Rebirth -> RebirthHub`

State ownership:

- `RunState`: current run, floor, room, modifiers, temporary resources
- `CombatState`: board, hands, placed Souvenirs, effects, turn order, result
- `MetaState` / `PlayerProfile`: persistent upgrades, unlocked Souvenirs, skills, memories, currencies
- `NarrativeState`: typed flags, clue discovery, memory unlocks, bounded choice consequences
- `WorldSeedState`: run seed, content version, generator inputs

Ownership rules:

- One service owns each state model.
- State models do not mutate each other directly.
- Systems request state changes through explicit APIs, commands, or events.
- Unity scene scripts must not directly mutate save, progression, combat, or narrative internals.

### Event Boundaries

The project uses three event layers:

- **Domain events:** gameplay facts such as `CombatWon`, `ClueDiscovered`, `SouvenirUnlocked`, `HeroDied`.
- **Application events:** orchestration requests such as `EnterCombatRequested`, `LoadRoomRequested`, `ReturnToHubRequested`.
- **UI events:** presentation/input events such as `CardSelected`, `HotspotClicked`, `RewardHovered`.

Rule: domain systems emit facts, application services handle flow, and UI only collects player intent or renders state.

### Data Persistence

**Save System:** Local versioned JSON snapshots with safe write and recovery.

Save rules:

- Every save includes `SaveVersion`.
- Write to temp file first.
- Flush/write complete before replacing primary save.
- Retain last-known-good backup.
- Validate after read.
- If primary save is corrupt, load backup.
- If backup is also invalid, start clean with explicit error reporting.
- Migration hooks are required before any schema-breaking change.

Do not use `PlayerPrefs` for core progression. `PlayerPrefs` may be used only for simple local settings.

### Scene Structure

**Approach:** Boring bootstrap scene plus controlled content scenes.

The bootstrap scene owns only:

- composition root
- service registration
- config/content version loading
- save/profile initialization
- platform service initialization
- mode controller startup

It must not contain gameplay managers.

Scene categories:

- `Boot`
- `MainMenu`
- `RebirthHub`
- `RoomScenes`
- `CombatScene` or combat overlay
- `Reward/Transition` screens

Scene loading is an application service triggered by mode transitions, not by narrative scripts, content definitions, or UI widgets.

### Combat Architecture

**Approach:** Unity-free deterministic C# combat assembly with Unity presentation adapter.

Combat core must have no `UnityEngine` dependency. Avoid `MonoBehaviour`, `ScriptableObject`, coroutines, `Time`, `UnityEngine.Random`, and Unity-specific structs in the rules layer.

Combat engine owns:

- 4x4 board state
- 8-card combat hands
- placement validation
- command application
- trigger/effect resolution
- hero/enemy life
- board-life tiebreakers
- deterministic AI hooks
- action log
- final state hash

Suggested command boundary:

- UI produces player intent.
- Adapter converts intent into combat commands.
- Tests drive `CombatEngine.Apply(command)` directly.
- Engine returns state deltas/events.
- Unity presentation renders deltas/events.

Determinism rule: same content version + same seed + same command list must produce the same final state hash.

### Content Data

**Approach:** ScriptableObject assets are editor/content inputs only; runtime systems consume validated immutable DTOs.

Use ScriptableObjects for authored definitions:

- Souvenirs
- enemies
- floors/rooms
- encounters
- rewards
- divine favors
- narrative events
- clues/memories
- achievements mapping

Before runtime use, content is converted into immutable DTOs and validated.

Validation must catch:

- duplicate IDs
- missing references
- invalid board/effect definitions
- invalid room exits
- missing clue/memory references
- invalid reward tables
- achievement mapping gaps
- missing asset provenance for release-candidate builds

### RNG and Determinism

**Approach:** Injectable seeded PRNG service.

All procedural choices must flow through the RNG abstraction:

- room encounter selection
- reward generation
- modifiers
- enemy hand/deck setup if applicable
- deterministic AI tie-breaking

Rule: core gameplay must not call `UnityEngine.Random` directly.

### UI Framework

**Approach:** uGUI for MVP runtime UI.

uGUI is selected for MVP because Tower Oblivion needs fast iteration on:

- combat board interaction
- mouse hover and selection
- gamepad navigation
- HUD status icons
- inventory/memory/skill screens
- modal reward choices
- high-contrast combat feedback

UI Toolkit may be used later for editor tooling or non-critical screens, but it is not the MVP runtime baseline.

### Asset Management

**Approach:** Direct references and scene-based loading for MVP.

The MVP should not start with Addressables unless content scale forces it.

Asset rules:

- use Sprite Atlases where beneficial
- keep room asset contracts explicit
- track commercial provenance for every generated/purchased asset
- avoid runtime string-path loading except controlled debug/development utilities
- reuse should mean reusable domain assemblies and data contracts, not a generalized content pipeline yet

### Narrative System

**Approach:** Typed flag registry and narrative event log API.

The MVP supports bounded choice consequences:

- unlock/block small dialogue variations
- unlock clues and memory fragments
- alter reward category bias in limited cases
- set one or two medium-term payoff flags
- persist narrative flags across death/rebirth

Narrative rules:

- Avoid raw string flags in gameplay code.
- Use typed keys, generated constants, or a registry.
- Narrative flags mutate narrative state only.
- Narrative scripts/events do not directly load scenes, unlock achievements, or alter UI.

### Enemy AI

**Approach:** Deterministic heuristic board AI with stable tie-breaking.

Enemy AI evaluates legal placements using weighted heuristics:

- damage hero
- preserve own hero life
- destroy or weaken high-value Souvenirs
- exploit adjacency effects
- block obvious player combos
- prefer strong board-life outcome when hero life is close

Tie-breaking must be deterministic from current state and seed.

### Audio Architecture

**Approach:** Unity Audio with mixer groups.

Mixer groups:

- Master
- Music
- SFX
- UI
- Ambience

FMOD/Wwise are deferred unless adaptive music or large-scale audio complexity becomes necessary.

### Platform Services

**Approach:** Steam features behind `IPlatformServices`.

Initial platform interface responsibilities:

- unlock achievement
- query achievement state if needed
- store local achievement intent when Steam is unavailable
- replay pending achievement unlocks when Steam becomes available
- provide local no-op/test implementation

Steam Cloud is not part of the MVP baseline.

### Test Acceptance Criteria

Architecture is considered implementable only if these tests are possible:

- Combat simulation runs 1,000 seeded games without Unity scene loading.
- Same combat seed + command list produces the same final state hash.
- Save/load roundtrip preserves run, board, flags, meta, and version fields.
- Corrupt primary save loads backup or starts clean with explicit error reporting.
- Content validation catches duplicate IDs, missing references, invalid board definitions, and achievement mapping gaps.
- Platform service mock proves Steam calls are not required for local play.

### Deferred Decisions

| Topic | Deferred Until | Reason |
|---|---|---|
| Addressables | Content scale or DLC/update need | Not required for compact MVP |
| Steam Cloud | Post-local-save stability | Cloud conflict handling adds risk |
| Full dialogue middleware | Narrative complexity proves need | Custom flags/events are enough for first chapter |
| Analytics | Playtest/Steam demo phase | Should be added behind an interface, not core dependency |
| FMOD/Wwise | Audio complexity proves need | Unity Audio is enough for MVP |

## Cross-cutting Concerns

These patterns apply to all systems and must be followed by every implementation.

### Error Handling

**Strategy:** Result objects for expected domain failures, exceptions for programmer/configuration faults, centralized fatal handling for unrecoverable runtime errors.

Error categories:

| Category | Use For | Handling |
|---|---|---|
| Recoverable domain failure | Invalid player action, blocked room, invalid combat placement | Return `Result.Failure(...)` |
| Content/configuration fault | Missing Souvenir ID, invalid room exit, duplicate flag key | Fail validation; block play/build when critical |
| Runtime infrastructure failure | Save write failed, Steam unavailable, asset load failed | Log warning/error, recover if possible |
| Fatal failure | Save system unusable, required content catalog missing | Show safe error screen and stop current flow |

Example:

```csharp
public readonly record struct Result(bool Success, string ErrorCode, string Message)
{
    public static Result Ok() => new(true, "", "");
    public static Result Failure(string code, string message) => new(false, code, message);
}

public Result PlaceCard(CombatCommand command)
{
    if (!_state.Board.IsEmpty(command.TargetCell))
        return Result.Failure("combat.cell_occupied", "Target cell is already occupied.");

    _resolver.Apply(command);
    return Result.Ok();
}
```

Rules:

- Domain systems return `Result` for expected failures.
- Do not use exceptions for normal gameplay decisions.
- Content validation errors must include stable error codes.
- Player-facing error text must not expose technical details.
- Fatal errors go through one application-level handler.

### Logging

**Format:** Structured plain-text logs through an `ILogger` abstraction.

**Destination:**

- Unity Console in editor.
- Local log file in development builds.
- Release builds keep `ERROR` and critical `WARN` logs only unless debug mode is enabled.

Log levels:

| Level | Use |
|---|---|
| `ERROR` | Broken flow, failed save/load, missing required content |
| `WARN` | Unexpected but recovered condition |
| `INFO` | Major flow milestones: run started, combat ended, save completed |
| `DEBUG` | Diagnostic state, development builds only |
| `TRACE` | Very verbose replay/debug logs, never default in release |

Required log fields:

- level
- category
- event code
- message
- run seed when available
- content version when available
- save version when relevant

Example:

```csharp
public interface ILogger
{
    void Info(string category, string code, string message, LogContext context = default);
    void Warn(string category, string code, string message, LogContext context = default);
    void Error(string category, string code, string message, LogContext context = default);
}

public readonly record struct LogContext(
    int? RunSeed = null,
    string ContentVersion = "",
    int? SaveVersion = null
);
```

Rules:

- No direct `Debug.Log` calls in domain assemblies.
- Performance-critical loops must not allocate log strings unless the level is enabled.
- Combat replay logs use command/action records, not free-form text.

### Configuration

**Approach:** Separate authored content, runtime config snapshots, and player settings.

Configuration types:

| Type | Storage | Notes |
|---|---|---|
| Authored game content | ScriptableObject assets | Souvenirs, enemies, rooms, rewards, narrative events |
| Runtime config snapshot | Plain C# immutable DTOs | Built from validated content before play |
| Player settings | Local settings file or `PlayerPrefs` for simple preferences | Audio volume, display, controls |
| Build/platform config | ScriptableObject or JSON config | Steam app ID, build channel, debug flags |

Rules:

- Runtime systems consume validated DTOs, not mutable ScriptableObject assets.
- Config/content version must be available to save, run generation, combat logs, and validation reports.
- Player settings are separate from progression saves.
- Balance values must live in authored data/config, not hardcoded inside systems unless truly constant.

Example:

```csharp
public sealed record CombatRulesConfig(
    int BoardWidth,
    int BoardHeight,
    int CardsPerSide,
    int StartingHeroLife,
    string ContentVersion
);
```

### Event System

**Pattern:** Typed synchronous events separated into domain, application, and UI layers.

Event layers:

| Layer | Purpose | Example |
|---|---|---|
| Domain events | Gameplay facts | `CombatWon`, `HeroDied`, `ClueDiscovered` |
| Application events | Flow/orchestration requests | `EnterCombatRequested`, `LoadRoomRequested` |
| UI events | Player intent/presentation | `CardSelected`, `HotspotClicked` |

Rules:

- No string event names.
- Events are typed C# records.
- Domain events describe facts, not instructions.
- Application events may request transitions.
- UI events must not mutate domain state directly.
- Event handlers must be deterministic when they affect gameplay state.
- Event history for combat/run flow must be available in development builds.

Example:

```csharp
public interface IEventBus
{
    void Publish<TEvent>(TEvent evt) where TEvent : IGameEvent;
    IDisposable Subscribe<TEvent>(Action<TEvent> handler) where TEvent : IGameEvent;
}

public interface IGameEvent { }

public sealed record CombatWon(string EncounterId, int RemainingHeroLife) : IGameEvent;
public sealed record EnterCombatRequested(string EncounterId) : IGameEvent;
public sealed record CardSelected(string CardInstanceId) : IGameEvent;
```

### Debug and Development Tools

**Available Tools:**

- Run seed viewer/copy button
- Combat command log export
- Combat state dump
- Final combat state hash display
- Save export/reset
- Save validation report
- Narrative flag inspector/reset
- Content validation report
- Achievement mock status
- FPS/performance overlay
- Force room/combat/reward test commands

**Activation:**

- Enabled in editor automatically.
- Enabled in development builds through a debug flag.
- Disabled in release builds unless explicitly compiled with a debug/dev symbol.
- Debug commands must not be reachable from normal player UI in release builds.

Example:

```csharp
public interface IDebugTool
{
    string Name { get; }
    bool IsAvailable(DebugBuildContext context);
    void Execute(DebugCommand command);
}
```

Rules:

- Debug tools can inspect state but must use official APIs to mutate state.
- Any debug mutation must be logged.
- Debug tools must not create save states impossible through normal play unless clearly marked as test-only.
- Save export must redact platform/user-specific information if needed.

### Determinism and Replay

Because Tower Oblivion depends on seeded runs and deterministic combat, all relevant systems must support replay/debug inspection.

Mandatory deterministic inputs:

- content version
- run seed
- combat seed if separate
- command/action list
- difficulty/floor modifiers
- deck/hand source data

Mandatory deterministic outputs:

- combat result
- reward table result
- narrative flag changes
- final state hash

Rule: if a gameplay result cannot be reproduced from seed + content version + command log, it is not acceptable for core MVP systems.

### Implementation Rules Summary

- Domain logic must not depend on `UnityEngine`.
- Expected gameplay failures return `Result`, not exceptions.
- Runtime systems consume immutable DTOs, not mutable ScriptableObjects.
- Events are typed records, not strings.
- Save, combat, run generation, and narrative flags must be inspectable.
- Debug tools are dev-only and must use official APIs.
- All core randomness uses injectable seeded RNG.

## Project Structure

### Organization Pattern

**Pattern:** Hybrid Unity structure with mandatory assembly boundaries.

**Rationale:** The project keeps a familiar Unity folder layout while using asmdefs to enforce architecture. This avoids an abstract folder tree, but still prevents `Core`, `Gameplay`, and Unity-facing code from collapsing into mixed responsibilities.

### Directory Structure

```text
TowerOblivion/
├── Assets/
│   ├── Art/
│   │   ├── Characters/
│   │   ├── Rooms/
│   │   ├── Souvenirs/
│   │   ├── UI/
│   │   └── VFX/
│   ├── Audio/
│   │   ├── Ambience/
│   │   ├── Music/
│   │   ├── SFX/
│   │   └── UI/
│   ├── Data/
│   │   ├── Authoring/
│   │   │   ├── Achievements/
│   │   │   ├── Combat/
│   │   │   ├── Enemies/
│   │   │   ├── Narrative/
│   │   │   ├── Progression/
│   │   │   ├── Rewards/
│   │   │   ├── Rooms/
│   │   │   └── Souvenirs/
│   │   ├── Generated/
│   │   └── Validation/
│   ├── Prefabs/
│   │   ├── Combat/
│   │   ├── Rooms/
│   │   ├── UI/
│   │   └── VFX/
│   ├── Scenes/
│   │   ├── Boot/
│   │   ├── MainMenu/
│   │   ├── Hub/
│   │   ├── Rooms/
│   │   ├── Combat/
│   │   └── TestHarness/
│   ├── Scripts/
│   │   ├── Core/
│   │   ├── Gameplay/
│   │   │   ├── Combat/
│   │   │   ├── Exploration/
│   │   │   ├── Narrative/
│   │   │   ├── Progression/
│   │   │   └── RunGeneration/
│   │   ├── Presentation/
│   │   │   ├── CombatView/
│   │   │   ├── RoomView/
│   │   │   ├── SceneFlow/
│   │   │   └── UI/
│   │   ├── Infrastructure/
│   │   │   ├── Content/
│   │   │   ├── Persistence/
│   │   │   ├── Platform/
│   │   │   └── Logging/
│   │   ├── Input/
│   │   └── Editor/
│   ├── Settings/
│   ├── Tests/
│   │   ├── EditMode/
│   │   └── PlayMode/
│   └── UI/
│       ├── Fonts/
│       ├── Icons/
│       └── Sprites/
├── Packages/
├── ProjectSettings/
├── Docs/
└── _bmad-output/
```

### Mandatory Assembly Boundaries

| Assembly | Location | Responsibility | May Reference |
|---|---|---|---|
| `TowerOblivion.Core` | `Assets/Scripts/Core` | Tiny shared primitives only: `Result`, IDs, deterministic RNG interfaces, event abstractions, logging interfaces | None |
| `TowerOblivion.Gameplay` | `Assets/Scripts/Gameplay` | Unity-free game rules: combat, progression, narrative flags, exploration state, run generation | `Core` |
| `TowerOblivion.Presentation` | `Assets/Scripts/Presentation` | uGUI views, presenters, visual feedback, scene-facing adapters | `Core`, `Gameplay`, `Input` |
| `TowerOblivion.Infrastructure` | `Assets/Scripts/Infrastructure` | JSON persistence, content loading/conversion, platform services, logging implementation | `Core`, `Gameplay` |
| `TowerOblivion.Input` | `Assets/Scripts/Input` | Unity Input System adapters that emit typed commands/events | `Core`, `Gameplay` |
| `TowerOblivion.Editor` | `Assets/Scripts/Editor` | content validation, import tools, editor utilities | Runtime assemblies as needed |
| `TowerOblivion.Tests.EditMode` | `Assets/Tests/EditMode` | deterministic/domain/infrastructure tests | Runtime assemblies |
| `TowerOblivion.Tests.PlayMode` | `Assets/Tests/PlayMode` | scene wiring, prefab, UI, and smoke tests | Runtime assemblies |

### System Location Mapping

| System | Location | Responsibility |
|---|---|---|
| `Result`, IDs, event interfaces | `Assets/Scripts/Core` | Shared primitives only |
| Seeded RNG interfaces | `Assets/Scripts/Core` | Abstractions for deterministic systems |
| Combat rules | `Assets/Scripts/Gameplay/Combat` | 4x4 board, commands, effects, state hashes, AI heuristics |
| Exploration rules | `Assets/Scripts/Gameplay/Exploration` | Room state, hotspot state, interaction rules |
| Narrative flags | `Assets/Scripts/Gameplay/Narrative` | Typed flags, narrative event log, clue/memory unlock rules |
| Progression | `Assets/Scripts/Gameplay/Progression` | XP, meta-currency, skill upgrades, Souvenir unlocks |
| Run generation | `Assets/Scripts/Gameplay/RunGeneration` | Fixed floor sequence with randomized encounters/rewards/modifiers |
| Combat visuals | `Assets/Scripts/Presentation/CombatView` | Board UI, card views, highlights, animations |
| Room visuals | `Assets/Scripts/Presentation/RoomView` | Hotspots, room image presentation, inspection UI |
| Scene flow | `Assets/Scripts/Presentation/SceneFlow` | Boot/menu/hub/room/combat scene presentation |
| HUD and menus | `Assets/Scripts/Presentation/UI` | Inventory, memories, skills, rewards, settings |
| Input adapters | `Assets/Scripts/Input` | Mouse/keyboard/gamepad input mapping to typed commands |
| Save implementation | `Assets/Scripts/Infrastructure/Persistence` | JSON serialization, temp-write, backup, validation, migration |
| Content conversion | `Assets/Scripts/Infrastructure/Content` | ScriptableObject-to-DTO conversion and runtime catalog loading |
| Steam/platform | `Assets/Scripts/Infrastructure/Platform` | `IPlatformServices` implementation and achievement intent replay |
| Logging | `Assets/Scripts/Infrastructure/Logging` | `ILogger` implementation and file/console routing |
| Content validation | `Assets/Scripts/Editor` | duplicate ID checks, missing refs, provenance checks, build gates |

### Naming Conventions

#### Files

| File Type | Convention | Example |
|---|---|---|
| C# scripts | PascalCase, one public type per file | `CombatEngine.cs` |
| Interfaces | PascalCase with `I` prefix | `IPlatformServices.cs` |
| Records/DTOs | PascalCase with explicit purpose | `CombatRulesConfig.cs` |
| Unity scenes | PascalCase by flow or room ID | `Boot.unity`, `Room_Floor01_Cell.unity` |
| Prefabs | PascalCase with category prefix when useful | `CardView.prefab`, `RoomHotspot.prefab` |
| ScriptableObjects | Type prefix + stable ID/name | `Souvenir_BrokenLaurel.asset` |
| Tests | Type or behavior + `Tests` suffix | `CombatEngineDeterminismTests.cs` |

#### Code Elements

| Element | Convention | Example |
|---|---|---|
| Classes / records / structs | PascalCase | `CombatState` |
| Interfaces | PascalCase with `I` prefix | `ISeededRandom` |
| Methods | PascalCase | `ApplyCommand()` |
| Properties | PascalCase | `CurrentRunSeed` |
| Private fields | `_camelCase` | `_combatEngine` |
| Local variables / parameters | camelCase | `targetCell` |
| Constants | PascalCase or UPPER_SNAKE for true constants | `MaxBoardSize` |
| Event records | Past-tense facts or explicit requests | `CombatWon`, `EnterCombatRequested` |
| Error codes | lowercase dot notation | `combat.cell_occupied` |
| Content IDs | lowercase snake/dot notation | `souvenir.broken_laurel` |

### Asset Naming Rules

- Souvenirs: `Souvenir_<Name>`
- Enemies: `Enemy_<Name>`
- Rooms: `Room_Floor##_Name`
- Rewards: `Reward_<Name>`
- Narrative events: `Narrative_<FlagOrEventName>`
- Icons: `Icon_<Purpose>`
- SFX: `SFX_<Category>_<Name>`
- Music: `Music_<Context>_<Name>`

### Architectural Boundaries

- `Core` must stay tiny. If a file is not a primitive, interface, ID, result type, or cross-cutting abstraction, it does not belong in `Core`.
- `Gameplay` must not depend on `UnityEngine`, scenes, prefabs, ScriptableObjects, PlayerPrefs, Steam, or uGUI.
- `Presentation` may depend on Unity and uGUI, but it must not own gameplay rules.
- `Infrastructure` may perform IO, serialization, content conversion, platform calls, and logging implementation.
- `Input` maps Unity Input System actions to typed commands/events only.
- `Editor` code must never be referenced by runtime assemblies.
- ScriptableObjects are authored content inputs only; runtime systems consume validated DTOs.
- Tests must mirror the architecture: deterministic systems in EditMode, Unity scene/prefab integration in PlayMode.

### Test Structure

```text
Assets/Tests/
├── EditMode/
│   ├── Core/
│   ├── Combat/
│   ├── Narrative/
│   ├── Progression/
│   ├── RunGeneration/
│   ├── Persistence/
│   └── ContentValidation/
└── PlayMode/
    ├── SceneFlow/
    ├── CombatView/
    ├── RoomView/
    ├── UI/
    └── Smoke/
```

## Implementation Patterns

These are implementation contracts. They exist to prevent different agents from solving the same problem in incompatible ways.

### Assembly Dependency Contract

Mandatory dependency direction:

```text
TowerOblivion.Core
  <- TowerOblivion.Gameplay
  <- TowerOblivion.Presentation

TowerOblivion.Core
  <- TowerOblivion.Infrastructure

TowerOblivion.Core
  <- TowerOblivion.Input

TowerOblivion.Editor
  -> runtime assemblies as needed

TowerOblivion.Tests
  -> runtime assemblies
```

Rules:

- `Core` references nothing.
- `Gameplay` references `Core` only.
- `Presentation` may reference `Core`, `Gameplay`, and `Input`.
- `Infrastructure` may reference `Core` and `Gameplay`.
- `Input` may reference `Core` and `Gameplay`.
- `Gameplay` must never reference `UnityEngine`, uGUI, scenes, prefabs, ScriptableObjects, Steam, PlayerPrefs, or Addressables.

### Contract 1: Gameplay Is Unity-Free

Gameplay owns rules, state, commands, deterministic simulation, and domain events.

Gameplay includes:

- combat board rules
- Souvenir command resolution
- enemy combat AI heuristics
- progression rules
- narrative flag rules
- exploration/interaction rules
- run generation rules

Gameplay does not include:

- MonoBehaviours
- prefabs
- scene loading
- ScriptableObject assets
- Unity Input System callbacks
- UI rendering
- Steam APIs
- file IO

Example:

```csharp
public interface IRng
{
    int NextInt(int minInclusive, int maxExclusive);
}

public sealed record CombatApplyResult(
    bool Success,
    IReadOnlyList<CombatEvent> Events,
    CombatStateDelta StateDelta,
    RngTrace RngTrace,
    string? FailureReason,
    string StateHash
);

public interface ICombatEngine
{
    CombatApplyResult Apply(CombatCommand command);
}
```

Rules:

- Combat commands are used only for deterministic/replayable gameplay decisions.
- Do not use commands for ordinary UI clicks, one-off animations, menu navigation, or simple view state.
- Same initial state + same command list + same content version + same RNG seed must produce the same final state hash.

### Contract 2: Presentation Adapts Unity Objects To Gameplay

Presentation owns uGUI views, scene views, visual feedback, and Unity-facing presenters.

Presentation may:

- render combat state
- highlight board cells
- collect pointer/gamepad input
- trigger animations and SFX
- call gameplay/application services
- subscribe to state/events for rendering

Presentation must not:

- own authoritative gameplay state
- decide combat outcomes
- mutate save snapshots directly
- modify narrative flags directly
- load scenes directly outside scene-flow services

Example:

```csharp
public sealed class CardViewPresenter : MonoBehaviour
{
    [SerializeField] private CardView _view;
    private ICombatInputService _combatInput = default!;

    public void Initialize(ICombatInputService combatInput)
    {
        _combatInput = combatInput;
    }

    public void OnClicked()
    {
        _combatInput.SelectCard(_view.CardInstanceId);
    }

    public void Render(CardViewModel model)
    {
        _view.SetName(model.DisplayName);
        _view.SetLife(model.CurrentLife);
        _view.SetPlayable(model.IsPlayable);
    }
}
```

Rules:

- MonoBehaviours are adapters only.
- Presenters are disposable by scene or mode lifetime.
- Presenters must unsubscribe from events on disable/destroy or mode exit.
- Presentation factories create Unity views only, not gameplay services.

### Contract 3: Infrastructure Owns Persistence, Content, Platform, and Logging Implementations

Infrastructure owns external boundaries.

Infrastructure includes:

- JSON save repository
- save migration pipeline
- ScriptableObject-to-runtime catalog conversion
- content validation runtime support
- Steam achievement adapter
- local/null platform adapter
- logging implementation
- file IO

Important interfaces:

```csharp
public interface ISaveRepository
{
    Result Save(SaveSlot slot, SaveSnapshotV1 snapshot);
    Result<SaveSnapshotV1> Load(SaveSlot slot);
}

public interface ISaveMigrator
{
    bool CanMigrate(int fromVersion, int toVersion);
    Result<SaveSnapshotV1> Migrate(string rawSaveJson);
}

public interface IContentCatalog<TId, TDto>
{
    bool TryGet(TId id, out TDto dto);
}

public interface IAchievementService
{
    Result Unlock(AchievementId id);
}
```

Rules:

- Infrastructure implements interfaces; gameplay consumes abstractions.
- Gameplay does not know whether persistence is JSON, Steam, local file, or mock.
- Steam-specific code stays behind `IAchievementService` or `IPlatformServices`.
- Use `NullAchievementService` or local mock for editor/testing.

### Contract 4: Modes Orchestrate Flow And Lifetime

Modes coordinate high-level flow. They do not own gameplay rules.

MVP mode states:

- `Boot`
- `MainMenu`
- `RunSetup`
- `Exploration`
- `Combat`
- `Reward`
- `GameOver`
- `RebirthHub`

Example:

```csharp
public interface IModeStateMachine
{
    GameModeId Current { get; }
    Result TransitionTo(GameModeId nextMode, GameModeTransitionContext context);
}

public interface IGameModeState
{
    GameModeId Id { get; }
    void Enter(GameModeContext context);
    void Tick(GameModeContext context);
    void Exit(GameModeContext context);
}
```

Rules:

- Use explicit states, not a generic hierarchical state machine for MVP.
- Modes own subscription lifetime for mode-specific presenters/services.
- Scene loading happens through scene-flow services.
- UI and narrative systems request mode changes; they do not load scenes directly.
- Mode transitions must be logged in development builds.

### Runtime Catalog Pattern

**Purpose:** Runtime systems consume read-only validated definitions, not mutable ScriptableObjects.

Data flow:

```text
ScriptableObject authoring assets
-> editor/build validation
-> immutable runtime DTOs
-> runtime catalogs
-> gameplay systems
```

Example:

```csharp
public sealed record SouvenirDefinition(
    SouvenirId Id,
    string DisplayNameKey,
    int BaseLife,
    IReadOnlyList<SouvenirEffectDefinition> Effects
);
```

Rules:

- DTOs are data-only records.
- DTOs are read-only definitions.
- Mutable run state belongs in domain state and save snapshots.
- Content validation must catch duplicate IDs, missing references, invalid numeric ranges, invalid board definitions, and missing localization keys.

### Narrative Flag/Event Pattern

**Purpose:** Support bounded choices and future-run consequences without raw string flags or hidden branching.

Example:

```csharp
public readonly record struct NarrativeFlagId(string Value);

public interface INarrativeService
{
    Result SetFlag(NarrativeFlagId flagId, bool value);
    bool HasFlag(NarrativeFlagId flagId);
}
```

Rules:

- No raw narrative flag strings in gameplay code.
- Use typed IDs or a generated registry.
- Narrative events are facts, not direct scene/UI instructions.
- Persistent narrative changes must be saveable and inspectable.

### Event Contract

Events are typed notifications, not hidden control flow.

Rules:

- Gameplay emits typed domain events.
- Presentation subscribes and adapts to uGUI.
- Infrastructure listens only where necessary, such as achievements or save telemetry.
- Events must not directly mutate domain state.
- Critical progression remains explicit in services or mode state machines.
- Subscribers must be disposable and unsubscribed by mode/scene lifetime.
- Avoid bidirectional event spaghetti.

Example:

```csharp
public interface IEventBus
{
    void Publish<TEvent>(TEvent evt) where TEvent : IGameEvent;
    IDisposable Subscribe<TEvent>(Action<TEvent> handler) where TEvent : IGameEvent;
}

public interface IGameEvent { }

public sealed record CombatWon(EncounterId EncounterId, int RemainingHeroLife) : IGameEvent;
```

### Save Compatibility Contract

Save files are compatibility contracts, not direct domain object dumps.

Required save types:

- `SaveSnapshotV1`
- `SaveMetadata`
- `SaveSlot`
- `SaveMigrationPipeline`

Rules:

- Every save has a schema version.
- Every save has a content version.
- Save snapshots store primitives, IDs, flags, inventory, run state, and RNG seed/state.
- Do not serialize domain service objects directly.
- Migrations are explicit and tested.
- Unknown future fields should not crash loading.
- Failed load returns a safe `Result`, not an exception through UI flow.

Example:

```csharp
public sealed record SaveSnapshotV1(
    SaveMetadata Metadata,
    PlayerProfileSnapshot PlayerProfile,
    RunSnapshot? ActiveRun,
    NarrativeSnapshot Narrative
);

public sealed record SaveMetadata(
    int SaveVersion,
    string ContentVersion,
    DateTime SavedAtUtc
);
```

### Ownership And Lifetime Table

| Object | Created By | Owned By | Disposed By |
|---|---|---|---|
| Runtime catalogs | Infrastructure bootstrap | Application lifetime | Application shutdown |
| Mode state machine | Bootstrap/composition root | Application lifetime | Application shutdown |
| Save repository | Bootstrap/composition root | Application lifetime | Application shutdown |
| Event bus | Bootstrap/composition root | Application lifetime | Application shutdown |
| Mode-specific subscriptions | Mode `Enter` | Current mode | Mode `Exit` |
| Presenters | Scene/prefab factories | Scene or mode | Scene unload / mode exit |
| Combat state | Combat setup service | Combat mode / run state | Combat end or save snapshot |
| Runtime DTOs | Content conversion | Runtime catalogs | Application shutdown |
| Save snapshots | Save service | Persistence flow | After save/load operation |

### Boundary Interfaces

Use interfaces only at real boundaries, test seams, or platform/external dependencies.

Required boundary interfaces:

- `IRng`
- `IClock`
- `IEventBus`
- `IContentCatalog<TId, TDto>`
- `ISaveRepository`
- `ISaveMigrator`
- `IAchievementService`
- `IModeStateMachine`
- `IPresenterFactory`

Do not create interfaces for every internal class. For MVP, prefer concrete domain services unless an interface protects a boundary or enables necessary tests.

### Testing Contract

Every gameplay system must be runnable in EditMode tests without:

- Unity scene loading
- prefabs
- MonoBehaviours
- Addressables
- Steam
- PlayerPrefs
- `UnityEngine.Random`
- `Time.time`

Every gameplay test must inject:

- `IRng`
- content catalog or test catalog
- initial state/save snapshot where relevant
- fake event sink
- fake clock if time is involved

Required tests:

- combat determinism from seed + command list
- combat failure cases for invalid commands
- run generation determinism
- narrative flag persistence
- save/load roundtrip
- save migration
- content validation
- achievement service mock behavior

### Consistency Rules

| Area | Convention | Enforcement |
|---|---|---|
| Gameplay | Unity-free, deterministic, injected dependencies | asmdefs + EditMode tests |
| Presentation | Thin MonoBehaviour adapters | code review + PlayMode tests |
| Infrastructure | Owns IO/platform/content conversion | asmdefs + integration tests |
| Modes | Explicit states and lifetime control | PlayMode scene-flow tests |
| Commands | Used for replayable deterministic gameplay decisions only | combat tests |
| Events | Typed notifications, not hidden control flow | code review + disposable subscriptions |
| Saves | Versioned snapshots, not domain object dumps | migration/roundtrip tests |
| Content | Runtime read-only catalogs | validation tests |
| RNG | Injected `IRng` only | determinism tests |

## Architecture Validation

### Validation Summary

| Check | Result | Notes |
|---|---|---|
| Decision Compatibility | PASS | Unity 6.3 LTS, uGUI, bootstrap scenes, deterministic gameplay, local saves, and direct asset references are coherent |
| GDD Coverage | PASS | Combat, exploration, narrative flags, progression, save, Steam achievements, asset provenance, performance, and reusable engine needs are covered |
| Pattern Completeness | PASS | Communication, state, data access, entity/view creation, save, events, testing, and debug patterns are defined |
| Epic Mapping | PASS | All 8 epics map to architecture locations and implementation contracts |
| Document Completeness | PASS | Required sections are present; stale placeholder/conflict text removed |

### Coverage Report

| Area | Coverage |
|---|---|
| Core systems covered | 14/14 |
| Epics mapped | 8/8 |
| Major decisions documented | 12 |
| Implementation contracts defined | 9 |
| Validation issues found | 3 |
| Validation issues resolved | 3 |

### Issues Resolved

- Removed stale workflow placeholder text.
- Updated engine table to lock `uGUI` instead of leaving UI framework open.
- Updated scene management table to lock boring bootstrap scene + controlled content scenes.
- Added concise architecture summary near the top of the document.

### Validation Date

2026-04-25
