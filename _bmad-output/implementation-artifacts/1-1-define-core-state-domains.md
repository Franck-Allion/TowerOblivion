# Story 1.1: Define Core State Domains

Status: review

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

- [x] Create the minimal Unity project structure if it does not already exist. (AC: 5, 7)
  - [x] Create `Assets/Scripts/Core`.
  - [x] Create `Assets/Scripts/Gameplay`.
  - [x] Create `Assets/Tests/EditMode`.
  - [x] Create `TowerOblivion.Core.asmdef`.
  - [x] Create `TowerOblivion.Gameplay.asmdef` referencing `TowerOblivion.Core`.
  - [x] Create `TowerOblivion.Tests.EditMode.asmdef` referencing `TowerOblivion.Core` and `TowerOblivion.Gameplay`.

- [x] Add small folder README files. (AC: 7)
  - [x] `Assets/Scripts/Core/README.md`: primitive abstractions only; no Unity, domain rules, save logic, or content loading.
  - [x] `Assets/Scripts/Gameplay/README.md`: Unity-free gameplay rules/state only; no MonoBehaviours, ScriptableObjects, scenes, prefabs, Steam, PlayerPrefs, Addressables, or Unity time/random.

- [x] Implement core primitives needed by state models. (AC: 3, 4)
  - [x] Add `Result` or equivalent minimal success/failure type.
  - [x] Add stable ID value types or a generic strongly typed ID pattern for run, room, encounter, Souvenir, narrative flag, and content references.
  - [x] Add version/seed value objects such as `ContentVersion`, `SaveVersion`, and `RunSeed` if useful for clean state definitions.
  - [x] Keep all primitives Unity-free.

- [x] Implement `RunState`. (AC: 1, 2, 3)
  - [x] Include current run ID or seed reference.
  - [x] Include current floor/room context using stable IDs.
  - [x] Include active modifiers and transient run values in simple serializable collections.
  - [x] Do not include combat board internals or permanent meta progression.

- [x] Implement `CombatState`. (AC: 1, 2, 3)
  - [x] Include minimal placeholders for combat instance ID, encounter ID, turn/phase marker, hero/enemy life values, and board/hand references as IDs or primitive structures.
  - [x] Keep this as state only; do not implement full 4x4 combat rules, effects, AI, balancing, or full UX.

- [x] Implement `MetaState` or `PlayerProfileState`. (AC: 1, 2, 3)
  - [x] Include persistent unlocks, upgrade tiers, currencies, settings references, unlocked Souvenirs, memory progress, and chapter milestones as data fields or minimal collections.
  - [x] Do not implement progression economy logic beyond shape/ownership.

- [x] Implement `NarrativeState`. (AC: 1, 2, 3)
  - [x] Include typed narrative flags, clue/memory unlocks, and bounded consequence values.
  - [x] Avoid raw strings in gameplay-facing code; use typed IDs or registries.
  - [x] Do not implement deep branching, dialogue middleware, or narrative graph logic.

- [x] Implement `WorldSeedState`. (AC: 1, 2, 3)
  - [x] Include deterministic run seed, content version, and generator input/version fields.
  - [x] Do not implement full run generation yet.

- [x] Add EditMode tests. (AC: 3, 5, 6)
  - [x] Verify state models can be constructed with test IDs/seeds.
  - [x] Verify state models do not require Unity object instances or scene loading.
  - [x] Verify simple serialization-compatible roundtrip for representative state data.
  - [x] Verify derived/computed values are not required as authoritative state for these models.

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

GPT-5 Codex

### Debug Log References

- Unity EditMode test run attempted with `C:\Program Files\Unity\Hub\Editor\6000.3.14f1\Editor\Unity.exe`; blocked because another Unity instance already has `D:\Dev\Unity\TowerOblivion` open.
- Fallback pure C# runtime compile check passed with `dotnet build` against `Assets/Scripts/Core` and `Assets/Scripts/Gameplay`: 0 warnings, 0 errors.
- Fallback test-source compile check passed with `dotnet build` against Core, Gameplay, and EditMode test sources using NUnit: 0 warnings, 0 errors.
- User confirmed Unity validation passes on 2026-04-26.

### Completion Notes List

- Story marked ready for review after user-confirmed Unity validation pass.
- Core and Gameplay were consolidated around root `TowerOblivion.Core` typed IDs to avoid duplicate ID namespaces.
- Unity-free `Core` and `Gameplay` asmdefs were added with `noEngineReferences`.
- State models and representative EditMode tests were added for run, combat, profile/meta progression, narrative, and world seed domains.

### File List

- Assets/Scripts.meta
- Assets/Scripts/Core.meta
- Assets/Scripts/Core/ActorId.cs
- Assets/Scripts/Core/ActorId.cs.meta
- Assets/Scripts/Core/CardInstanceId.cs
- Assets/Scripts/Core/CardInstanceId.cs.meta
- Assets/Scripts/Core/ChapterMilestoneId.cs
- Assets/Scripts/Core/ChapterMilestoneId.cs.meta
- Assets/Scripts/Core/ClueId.cs
- Assets/Scripts/Core/ClueId.cs.meta
- Assets/Scripts/Core/CombatId.cs
- Assets/Scripts/Core/CombatId.cs.meta
- Assets/Scripts/Core/ContentVersion.cs
- Assets/Scripts/Core/ContentVersion.cs.meta
- Assets/Scripts/Core/CurrencyId.cs
- Assets/Scripts/Core/CurrencyId.cs.meta
- Assets/Scripts/Core/EncounterId.cs
- Assets/Scripts/Core/EncounterId.cs.meta
- Assets/Scripts/Core/IClock.cs
- Assets/Scripts/Core/IClock.cs.meta
- Assets/Scripts/Core/ILogger.cs
- Assets/Scripts/Core/ILogger.cs.meta
- Assets/Scripts/Core/IRng.cs
- Assets/Scripts/Core/IRng.cs.meta
- Assets/Scripts/Core/MemoryId.cs
- Assets/Scripts/Core/MemoryId.cs.meta
- Assets/Scripts/Core/ModifierId.cs
- Assets/Scripts/Core/ModifierId.cs.meta
- Assets/Scripts/Core/NarrativeFlagId.cs
- Assets/Scripts/Core/NarrativeFlagId.cs.meta
- Assets/Scripts/Core/README.md
- Assets/Scripts/Core/README.md.meta
- Assets/Scripts/Core/Result.cs
- Assets/Scripts/Core/Result.cs.meta
- Assets/Scripts/Core/RoomId.cs
- Assets/Scripts/Core/RoomId.cs.meta
- Assets/Scripts/Core/RunId.cs
- Assets/Scripts/Core/RunId.cs.meta
- Assets/Scripts/Core/RunSeed.cs
- Assets/Scripts/Core/RunSeed.cs.meta
- Assets/Scripts/Core/SaveVersion.cs
- Assets/Scripts/Core/SaveVersion.cs.meta
- Assets/Scripts/Core/SettingsProfileId.cs
- Assets/Scripts/Core/SettingsProfileId.cs.meta
- Assets/Scripts/Core/SouvenirId.cs
- Assets/Scripts/Core/SouvenirId.cs.meta
- Assets/Scripts/Core/TowerOblivion.Core.asmdef
- Assets/Scripts/Core/TowerOblivion.Core.asmdef.meta
- Assets/Scripts/Core/UpgradeTrackId.cs
- Assets/Scripts/Core/UpgradeTrackId.cs.meta
- Assets/Scripts/Gameplay.meta
- Assets/Scripts/Gameplay/Combat.meta
- Assets/Scripts/Gameplay/Combat/BoardCell.cs
- Assets/Scripts/Gameplay/Combat/BoardCell.cs.meta
- Assets/Scripts/Gameplay/Combat/CombatCardState.cs
- Assets/Scripts/Gameplay/Combat/CombatCardState.cs.meta
- Assets/Scripts/Gameplay/Combat/CombatState.cs
- Assets/Scripts/Gameplay/Combat/CombatState.cs.meta
- Assets/Scripts/Gameplay/Narrative.meta
- Assets/Scripts/Gameplay/Narrative/NarrativeCounterState.cs
- Assets/Scripts/Gameplay/Narrative/NarrativeCounterState.cs.meta
- Assets/Scripts/Gameplay/Narrative/NarrativeFlagState.cs
- Assets/Scripts/Gameplay/Narrative/NarrativeFlagState.cs.meta
- Assets/Scripts/Gameplay/Narrative/NarrativeState.cs
- Assets/Scripts/Gameplay/Narrative/NarrativeState.cs.meta
- Assets/Scripts/Gameplay/Progression.meta
- Assets/Scripts/Gameplay/Progression/CurrencyAmountState.cs
- Assets/Scripts/Gameplay/Progression/CurrencyAmountState.cs.meta
- Assets/Scripts/Gameplay/Progression/PlayerProfileState.cs
- Assets/Scripts/Gameplay/Progression/PlayerProfileState.cs.meta
- Assets/Scripts/Gameplay/Progression/UpgradeTierState.cs
- Assets/Scripts/Gameplay/Progression/UpgradeTierState.cs.meta
- Assets/Scripts/Gameplay/README.md
- Assets/Scripts/Gameplay/README.md.meta
- Assets/Scripts/Gameplay/RunGeneration.meta
- Assets/Scripts/Gameplay/RunGeneration/RunModifierState.cs
- Assets/Scripts/Gameplay/RunGeneration/RunModifierState.cs.meta
- Assets/Scripts/Gameplay/RunGeneration/RunResourceState.cs
- Assets/Scripts/Gameplay/RunGeneration/RunResourceState.cs.meta
- Assets/Scripts/Gameplay/RunGeneration/RunState.cs
- Assets/Scripts/Gameplay/RunGeneration/RunState.cs.meta
- Assets/Scripts/Gameplay/RunGeneration/WorldSeedState.cs
- Assets/Scripts/Gameplay/RunGeneration/WorldSeedState.cs.meta
- Assets/Scripts/Gameplay/TowerOblivion.Gameplay.asmdef
- Assets/Scripts/Gameplay/TowerOblivion.Gameplay.asmdef.meta
- Assets/Tests.meta
- Assets/Tests/EditMode.meta
- Assets/Tests/EditMode/AssemblyBoundaryTests.cs
- Assets/Tests/EditMode/AssemblyBoundaryTests.cs.meta
- Assets/Tests/EditMode/StateDomainSerializationTests.cs
- Assets/Tests/EditMode/StateDomainSerializationTests.cs.meta
- Assets/Tests/EditMode/StateModelTests.cs
- Assets/Tests/EditMode/StateModelTests.cs.meta
- Assets/Tests/EditMode/TowerOblivion.Tests.EditMode.asmdef
- Assets/Tests/EditMode/TowerOblivion.Tests.EditMode.asmdef.meta

### Change Log

- 2026-04-25: Added Core/Gameplay state-domain implementation and EditMode test sources; status remained in-progress pending Unity EditMode test execution.
- 2026-04-26: Marked all tasks complete and moved story to review after user-confirmed Unity validation pass.
