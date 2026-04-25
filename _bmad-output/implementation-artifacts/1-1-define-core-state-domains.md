# Story 1.1: Define Core State Domains

Status: ready-for-dev

## Story

As a developer,  
I want to define the core state domains for run, combat, meta progression, narrative, and world seed data,  
so that future systems keep persistent, transient, combat, narrative, and deterministic generation state separated from the start.

## Acceptance Criteria

1. `RunState`, `CombatState`, `MetaState` or `PlayerProfileState`, `NarrativeState`, and `WorldSeedState` exist as plain C# serializable state models in the Unity-free gameplay layer.
2. Each state model has a clear responsibility matching the architecture ownership boundaries and does not directly mutate another state model.
3. State models use stable IDs, primitives, collections, or project-owned value objects only; they do not reference `UnityEngine`, MonoBehaviours, ScriptableObjects, scenes, prefabs, uGUI, Steam, PlayerPrefs, or Unity serialization types.
4. Minimal shared primitives required by these models exist in `Core`, including `Result`, stable ID value types or a generic ID pattern, deterministic seed/RNG-facing value types where needed, and content/save version fields.
5. Assembly definitions enforce dependency direction: `TowerOblivion.Core` references nothing project-specific, and `TowerOblivion.Gameplay` references `TowerOblivion.Core` only.
6. EditMode tests prove the gameplay state models can be constructed, serialized-compatible data can roundtrip through simple in-memory JSON or equivalent test serialization, and the `Gameplay` assembly does not require scene loading or Unity object instances.
7. The implementation creates or updates minimal folder README files for `Assets/Scripts/Core` and `Assets/Scripts/Gameplay`, documenting purpose, allowed dependencies, forbidden contents, and examples.
8. No save file IO, full save repository, full combat rules, full progression economy, room loading, Unity UI, Steam integration, or content authoring workflow is implemented in this story.

## Tasks / Subtasks

- [ ] Create the minimal Unity project structure if it does not already exist. (AC: 5, 7)
  - [ ] Create `Assets/Scripts/Core`.
  - [ ] Create `Assets/Scripts/Gameplay`.
  - [ ] Create `Assets/Tests/EditMode`.
  - [ ] Create `TowerOblivion.Core.asmdef`.
  - [ ] Create `TowerOblivion.Gameplay.asmdef` referencing `TowerOblivion.Core`.
  - [ ] Create `TowerOblivion.Tests.EditMode.asmdef` referencing `TowerOblivion.Core` and `TowerOblivion.Gameplay`.

- [ ] Add small folder README files. (AC: 7)
  - [ ] `Assets/Scripts/Core/README.md`: primitive abstractions only; no Unity, domain rules, save logic, or content loading.
  - [ ] `Assets/Scripts/Gameplay/README.md`: Unity-free gameplay rules/state only; no MonoBehaviours, ScriptableObjects, scenes, prefabs, Steam, PlayerPrefs, Addressables, or Unity time/random.

- [ ] Implement core primitives needed by state models. (AC: 3, 4)
  - [ ] Add `Result` or equivalent minimal success/failure type.
  - [ ] Add stable ID value types or a generic strongly typed ID pattern for run, room, encounter, Souvenir, narrative flag, and content references.
  - [ ] Add version/seed value objects such as `ContentVersion`, `SaveVersion`, and `RunSeed` if useful for clean state definitions.
  - [ ] Keep all primitives Unity-free.

- [ ] Implement `RunState`. (AC: 1, 2, 3)
  - [ ] Include current run ID or seed reference.
  - [ ] Include current floor/room context using stable IDs.
  - [ ] Include active modifiers and transient run values in simple serializable collections.
  - [ ] Do not include combat board internals or permanent meta progression.

- [ ] Implement `CombatState`. (AC: 1, 2, 3)
  - [ ] Include minimal placeholders for combat instance ID, encounter ID, turn/phase marker, hero/enemy life values, and board/hand references as IDs or primitive structures.
  - [ ] Keep this as state only; do not implement full 4x4 combat rules, effects, AI, balancing, or full UX.

- [ ] Implement `MetaState` or `PlayerProfileState`. (AC: 1, 2, 3)
  - [ ] Include persistent unlocks, upgrade tiers, currencies, settings references, unlocked Souvenirs, memory progress, and chapter milestones as data fields or minimal collections.
  - [ ] Do not implement progression economy logic beyond shape/ownership.

- [ ] Implement `NarrativeState`. (AC: 1, 2, 3)
  - [ ] Include typed narrative flags, clue/memory unlocks, and bounded consequence values.
  - [ ] Avoid raw strings in gameplay-facing code; use typed IDs or registries.
  - [ ] Do not implement deep branching, dialogue middleware, or narrative graph logic.

- [ ] Implement `WorldSeedState`. (AC: 1, 2, 3)
  - [ ] Include deterministic run seed, content version, and generator input/version fields.
  - [ ] Do not implement full run generation yet.

- [ ] Add EditMode tests. (AC: 3, 5, 6)
  - [ ] Verify state models can be constructed with test IDs/seeds.
  - [ ] Verify state models do not require Unity object instances or scene loading.
  - [ ] Verify simple serialization-compatible roundtrip for representative state data.
  - [ ] Verify derived/computed values are not required as authoritative state for these models.

## Dev Notes

This is the foundation story for Epic 1. Keep the implementation deliberately small: state shape, ownership boundaries, core primitives, asmdefs, and tests only. Do not start full save/load, combat, progression, narrative, room, UI, or Steam implementation here.

### Source Requirements

- Epic 1 Story 1: "As a developer, I can define core state domains so that run, combat, meta, and seed data stay separated." [Source: `_bmad-output/epics.md` > Epic 1 > Stories]
- Epic 1 includes state boundaries for `RunState`, `CombatState`, `MetaState`, and `WorldSeedState`, but excludes full gameplay systems, full combat rules, chapter content, and broad engine abstraction. [Source: `_bmad-output/epics.md` > Epic 1 > Scope]
- Architecture adds `NarrativeState` ownership for flags and narrative variables. Include it in this story because later narrative and save work depend on it. [Source: `_bmad-output/game-architecture.md` > Ownership and State Boundaries]

### Architecture Compliance

Mandatory rules for this story:

- `TowerOblivion.Gameplay` must be Unity-free.
- `TowerOblivion.Core` must stay tiny.
- `Gameplay` references `Core` only.
- State models do not directly mutate each other.
- Runtime state must not live inside ScriptableObject assets.
- No `UnityEngine.Random`, `Time.time`, MonoBehaviours, ScriptableObjects, PlayerPrefs, Steam, Addressables, uGUI, scene references, prefabs, `Vector2`, `Vector3`, `Color`, `AnimationCurve`, `Sprite`, `AudioClip`, `GameObject`, or `SerializeField` in gameplay state models.

[Source: `_bmad-output/game-architecture.md` > Architectural Decisions, Project Structure, Implementation Patterns]  
[Source: `_bmad-output/project-context.md` > Engine-Specific Rules, Code Organization Rules]

### Project Structure Requirements

Expected files/folders for this story:

```text
Assets/
+-- Scripts/
|   +-- Core/
|   |   +-- README.md
|   |   +-- TowerOblivion.Core.asmdef
|   |   +-- ... core primitive C# files
|   +-- Gameplay/
|       +-- README.md
|       +-- TowerOblivion.Gameplay.asmdef
|       +-- RunGeneration/
|       +-- Combat/
|       +-- Narrative/
|       +-- Progression/
|       +-- ... state model C# files
+-- Tests/
    +-- EditMode/
        +-- TowerOblivion.Tests.EditMode.asmdef
        +-- ... state model tests
```

Use the architecture's hybrid Unity structure. Do not create new top-level script folders unless the architecture is updated. [Source: `_bmad-output/game-architecture.md` > Project Structure]

### Suggested Model Ownership

- `RunState`: active run context, current floor/room, active modifiers, transient run values.
- `CombatState`: isolated combat instance state, board/hand/life placeholders, turn/phase marker.
- `MetaState` / `PlayerProfileState`: persistent unlocks, upgrade tiers, Souvenir ownership, memory progress, settings references, currencies.
- `NarrativeState`: typed flags, clue discovery, memory unlocks, bounded consequences.
- `WorldSeedState`: deterministic seed/config inputs for run generation, encounters, rewards, and narrative variation.

Do not add behavior-heavy methods to these models. Keep logic in future services/systems.

### Testing Requirements

- Prefer EditMode tests.
- Tests must not require Unity scenes, prefabs, MonoBehaviours, Steam, PlayerPrefs, Addressables, `UnityEngine.Random`, or `Time.time`.
- Use simple representative fixtures for IDs, versions, seeds, and collections.
- If using JSON roundtrip tests, keep them in-memory only; do not implement file IO in this story.

[Source: `_bmad-output/project-context.md` > Testing Rules]

### Explicit Non-Goals

- No full local save repository.
- No atomic file writing.
- No save migration pipeline.
- No content validation pipeline.
- No room loading.
- No combat rules/effects/AI.
- No progression economy logic.
- No narrative dialogue system.
- No UI.
- No Steam integration.
- No Addressables, ECS, DI container, FMOD/Wwise, or dialogue middleware.

## Project Context Rules

The developer must follow `_bmad-output/project-context.md` before coding. The most relevant rules for this story:

- Keep Gameplay Unity-free.
- Core stays tiny.
- Ports/interfaces live near the caller; adapters live in Infrastructure.
- Do not create vague `Managers`, `Helpers`, `Common`, or generic `Systems` dumping grounds.
- Use typed IDs/registries instead of raw strings for gameplay-facing IDs.
- Do not place persistent state authority in UI, scenes, prefabs, presenters, ScriptableObjects, or MonoBehaviours.
- Save-facing state must use explicit snapshots/DTOs, not live service objects.

## References

- `_bmad-output/epics.md` > Epic 1: Minimal Reusable Game Foundation
- `_bmad-output/game-architecture.md` > Ownership and State Boundaries
- `_bmad-output/game-architecture.md` > Mandatory Assembly Boundaries
- `_bmad-output/game-architecture.md` > Implementation Patterns
- `_bmad-output/project-context.md` > Critical Implementation Rules

## Dev Agent Record

### Agent Model Used

To be filled during implementation.

### Debug Log References

### Completion Notes List

### File List
