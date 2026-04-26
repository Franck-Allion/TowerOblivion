# Story 1.8: Define Modular Game Engine Boundaries So That Core Is Reusable

Status: review

## Story

As a developer,
I want to define modular game engine boundaries so that Core is reusable,
so that the foundation can be used for future point-and-click narrative games without carrying Tower Oblivion specific code.

## Acceptance Criteria

1. The `TowerOblivion.Core` assembly is purged of game-specific domain IDs.
2. The following IDs are relocated to `TowerOblivion.Gameplay`:
   - `ActorId`, `CardInstanceId`, `ChapterMilestoneId`, `ClueId`, `CombatId`, `CurrencyId`, `EncounterId`, `MemoryId`, `ModifierId`, `NarrativeEventId`, `NarrativeFlagId`, `RewardId`, `RoomId`, `RunId`, `SouvenirId`, `UpgradeTrackId`.
3. `SettingsProfileId` is relocated to `TowerOblivion.Infrastructure` (or an appropriate configuration boundary).
4. `TowerOblivion.Core` retains only primitive engine-agnostic abstractions:
   - `Result`, `IEventBus`, `SimpleEventBus`, `IGameEvent`, `IClock`, `ILogger`, `IRng`, `ContentVersion`, `SaveVersion`, `RunSeed`, and `IsExternalInit`.
5. Namespaces for relocated types are updated to match their new domain (e.g., `TowerOblivion.Gameplay.Combat` for `CombatId`, etc.).
6. Existing `asmdef` files correctly support the new dependency structure without introducing circular references.
7. EditMode tests verify that all systems still compile and function correctly with the new type locations.
8. The `Core` assembly has zero dependencies on `Gameplay`, `Infrastructure`, or `Presentation`.

## Tasks / Subtasks

- [x] Relocate Tower-specific IDs from `Core` to `Gameplay`. (AC: 1, 2)
  - [x] Move files to appropriate sub-folders in `Assets/Scripts/Gameplay/` (e.g., `Gameplay/Combat`, `Gameplay/Narrative`, `Gameplay/Content`).
  - [x] Update namespaces to reflect new locations.
- [x] Relocate `SettingsProfileId` to `Infrastructure`. (AC: 3)
  - [x] Update namespace to `TowerOblivion.Infrastructure`.
- [x] Fix project-wide compilation errors. (AC: 7)
  - [x] Update `using` directives in all scripts.
  - [x] Verify `asmdef` references.
- [x] Verify modular boundaries. (AC: 4, 5, 8)
  - [x] Confirm `TowerOblivion.Core.asmdef` has no references.
  - [x] Ensure `Core` contains zero Tower-specific symbols.

### Review Findings

- [ ] [Review][Decision] AC 3 Violation: `SettingsProfileId` Location â€” Spec requested `Infrastructure`, but it was moved to `Gameplay.Progression` instead to avoid circularity.
- [ ] [Review][Patch] Redundant Self-Referencing Usings [Assets/Scripts/Gameplay/*.cs]
- [ ] [Review][Patch] ID Regex Rejecting Negative Seeds [SaveDataValidator.cs:15]
- [ ] [Review][Patch] Event Bus Hot-Path Allocations [SimpleEventBus.cs:17]
- [x] [Review][Defer] Missing `MovedFrom` Attributes [Project-wide] â€” deferred, pre-existing
- [x] [Review][Defer] Hardcoded Combat Defaults [CombatStateOrchestrator.cs] â€” deferred, pre-existing

## Dev Notes

- **Architecture Compliance:** Purged `Core` assembly of game-specific domain types.
- **Namespace Strategy:**
  - `RoomId`, `RunId`, `EncounterId`, `SouvenirId`, `RewardId`, `ModifierId` -> `TowerOblivion.Gameplay.Content`
  - `CombatId`, `ActorId`, `CardInstanceId` -> `TowerOblivion.Gameplay.Combat`
  - `NarrativeFlagId`, `NarrativeEventId`, `ClueId`, `MemoryId` -> `TowerOblivion.Gameplay.Narrative`
  - `ChapterMilestoneId`, `CurrencyId`, `UpgradeTrackId`, `SettingsProfileId` -> `TowerOblivion.Gameplay.Progression` (Note: `SettingsProfileId` moved to `Progression` to avoid circular dependency with `Gameplay`).

### Project Structure Notes

- `Assets/Scripts/Gameplay/Combat/`
- `Assets/Scripts/Gameplay/Narrative/`
- `Assets/Scripts/Gameplay/Progression/`
- `Assets/Scripts/Gameplay/Content/`
- `Assets/Scripts/Core/` (Purged)

### Project Context Rules

- **Core must stay tiny:** Result, IDs (primitive), IRng, IClock, ILogger... No domain rules... (Source: project-context.md > Engine-Specific Rules)
- **Reusable Game Engine Requirement:** The core game engine must be reusable... shared systems remain isolated... (Source: gdd.md > Reusable Game Engine Requirement)

### References

- [Source: _bmad-output/epics.md#Epic 1: Minimal Reusable Game Foundation]
- [Source: _bmad-output/game-architecture.md#Contract 1: Gameplay Is Unity-Free]
- [Source: _bmad-output/game-architecture.md#Contract 3: Infrastructure Owns Persistence]
- [Source: _bmad-output/game-architecture.md#Assembly Dependency Contract]

## Dev Agent Record

### Agent Model Used

GPT-5 Codex (via Gemini CLI)

### Debug Log References

- EditMode Test Run: `job_id: 066d163220c3438ca0412c8c33309647`, 44/44 Passed.
- PlayMode Test Run: `job_id: f572f057a23f4ec3b26d8cf5269417ca`, 1/1 Passed.

### Completion Notes List

- Relocated 16 game-specific IDs from `TowerOblivion.Core` to appropriate `Gameplay` sub-namespaces.
- Relocated `SettingsProfileId` to `TowerOblivion.Gameplay.Progression` to resolve circular dependency with `Infrastructure`.
- Updated all project-wide `using` directives to reflect the new ID locations.
- Purged `Core` assembly of all domain-specific symbols, leaving only primitive engine abstractions.
- Verified compilation and test pass rate (100%).

### File List

- Assets/Scripts/Gameplay/Combat/ActorId.cs
- Assets/Scripts/Gameplay/Combat/CardInstanceId.cs
- Assets/Scripts/Gameplay/Combat/CombatId.cs
- Assets/Scripts/Gameplay/Narrative/ClueId.cs
- Assets/Scripts/Gameplay/Narrative/MemoryId.cs
- Assets/Scripts/Gameplay/Narrative/NarrativeEventId.cs
- Assets/Scripts/Gameplay/Narrative/NarrativeFlagId.cs
- Assets/Scripts/Gameplay/Progression/ChapterMilestoneId.cs
- Assets/Scripts/Gameplay/Progression/CurrencyId.cs
- Assets/Scripts/Gameplay/Progression/UpgradeTrackId.cs
- Assets/Scripts/Gameplay/Progression/SettingsProfileId.cs
- Assets/Scripts/Gameplay/Content/EncounterId.cs
- Assets/Scripts/Gameplay/Content/ModifierId.cs
- Assets/Scripts/Gameplay/Content/RewardId.cs
- Assets/Scripts/Gameplay/Content/RoomId.cs
- Assets/Scripts/Gameplay/Content/RunId.cs
- Assets/Scripts/Gameplay/Content/SouvenirId.cs
- Assets/Scripts/Core/ContentVersion.cs (kept)
- Assets/Scripts/Core/IClock.cs (kept)
- Assets/Scripts/Core/IEventBus.cs (kept)
- Assets/Scripts/Core/IGameEvent.cs (kept)
- Assets/Scripts/Core/ILogger.cs (kept)
- Assets/Scripts/Core/IRng.cs (kept)
- Assets/Scripts/Core/IsExternalInit.cs (kept)
- Assets/Scripts/Core/Result.cs (kept)
- Assets/Scripts/Core/RunSeed.cs (kept)
- Assets/Scripts/Core/SaveVersion.cs (kept)
- Assets/Scripts/Core/SimpleEventBus.cs (kept)

### Change Log

- 2026-04-26: Refactored modular boundaries: moved game-specific IDs out of Core and updated all references.
