# Story 1.5: Define Placeholder Narrative Flag So That Consequences Can Persist

Status: done

## Story

As a developer,
I want to define a placeholder narrative flag so that consequences can persist,
so that player choices can influence future runs and exploration.

## Acceptance Criteria

1. An `INarrativeService` exists in `Gameplay` (Unity-free) to manage boolean flags and integer counters.
2. The service provides methods to set, get, and toggle boolean flags using `NarrativeFlagId`.
3. The service provides methods to increment, decrement, and set integer counters using `NarrativeFlagId`.
4. Narrative state is stored in the `NarrativeState` model, ensuring compatibility with the existing save system.
5. The service emits `NarrativeFlagChanged` and `NarrativeCounterChanged` domain events via `IEventBus`.
6. A minimal `NarrativeEventOrchestrator` exists in `Gameplay` to evaluate `NarrativeEventContentDefinition` (from Story 1.3) against current flags.
7. EditMode tests verify that flags and counters are correctly updated in `NarrativeState` and events are published.
8. Implementation strictly follows the Unity-free Gameplay rule (no `UnityEngine` references).

## Tasks / Subtasks

- [x] Implement `INarrativeService` and `NarrativeService` in `Gameplay/Narrative`. (AC: 1, 2, 3, 4)
  - [x] Support boolean flag operations: `SetFlag`, `GetFlag`, `ToggleFlag`.
  - [x] Support integer counter operations: `SetCounter`, `GetCounter`, `AdjustCounter`.
  - [x] Ensure state is persisted to the provided `NarrativeState` object.
- [x] Define Narrative Domain Events in `Gameplay/Narrative`. (AC: 5)
  - [x] Add `NarrativeFlagChanged` record event.
  - [x] Add `NarrativeCounterChanged` record event.
- [x] Implement `NarrativeEventOrchestrator` in `Gameplay/Narrative`. (AC: 6)
  - [x] Implement logic to check if a `NarrativeEventContentDefinition` requirement (e.g., `RequiredFlagId`) is met.
- [x] Add EditMode Tests for Narrative logic. (AC: 7)
  - [x] Test flag persistence and event emission.
  - [x] Test counter persistence and event emission.
  - [x] Test orchestrator requirement evaluation.

### Review Findings

- [x] [Review][Patch] Events as Classes instead of Records: Resolved (Converted to records) [NarrativeEvents.cs]
- [x] [Review][Patch] Redundant/Overlapping State Model: Resolved (Removed redundant Counter from NarrativeFlagState) [NarrativeState.cs]
- [x] [Review][Patch] Inefficient Subscription & Lookup: Resolved (Fixed double-lookup in ToggleFlag, improved event logic) [NarrativeService.cs]
- [x] [Review][Patch] Orchestrator Logic Gaps: Resolved (Added counter requirements and must-be-false support) [NarrativeEventOrchestrator.cs]
- [x] [Review][Patch] Counter Boundary Safety: Resolved (Added overflow protection) [NarrativeService.cs:68]
- [x] [Review][Defer] Dead Code: Missing Runtime Wiring â€” deferred, pre-existing

## Dev Notes

- **Architecture Compliance:** Keep all narrative logic in `TowerOblivion.Gameplay` (Unity-free).
- **Event System:** Use the `IEventBus` established in Story 1.4.
- **State Ownership:** `NarrativeService` should take `NarrativeState` in its constructor (Dependency Injection).
- **Fail Fast:** Evaluate requirements against catalogs; ensure `NarrativeFlagId` defaults are safe (false/zero).

### Project Structure Notes

- `Assets/Scripts/Gameplay/Narrative/INarrativeService.cs`
- `Assets/Scripts/Gameplay/Narrative/NarrativeService.cs`
- `Assets/Scripts/Gameplay/Narrative/NarrativeEventOrchestrator.cs`
- `Assets/Scripts/Gameplay/Narrative/NarrativeEvents.cs`
- `Assets/Tests/EditMode/Narrative/NarrativeServiceTests.cs`
- `Assets/Scripts/Core/IsExternalInit.cs` (Added for record support)

### Project Context Rules

- **Unity-Free Gameplay:** `TowerOblivion.Gameplay` must not reference `UnityEngine`. (Source: project-context.md > Engine-Specific Rules)
- **Deterministic RNG:** (Not directly needed for basic flag setting, but keep in mind for future procedural narrative).
- **Save Integrity:** Narrative flags must be part of the `NarrativeState` in the save snapshot. (Source: project-context.md > Save ownership)
- **Records for Events:** Use C# records for domain events. (Source: project-context.md > Code Organization Rules)

### References

- [Source: _bmad-output/game-architecture.md#Narrative Flag/Event Pattern]
- [Source: _bmad-output/epics.md#Epic 1: Minimal Reusable Game Foundation]
- [Source: _bmad-output/implementation-artifacts/1-1-define-core-state-domains.md]
- [Source: _bmad-output/implementation-artifacts/1-3-define-placeholder-content-in-data.md]
- [Source: _bmad-output/implementation-artifacts/1-4-load-placeholder-room-from-reusable-room-definition.md]

## Dev Agent Record

### Agent Model Used

GPT-5 Codex (via Gemini CLI)

### Debug Log References

- EditMode Test Run: `job_id: 2219e2427cd7424cb85805c70cd07970`, 29/29 Passed.
- Post-Patch EditMode Test Run: `job_id: c50d874e967b4416af08fd61744b45bb`, 29/29 Passed.

### Completion Notes List

- Implemented `INarrativeService` and `NarrativeService` for boolean and integer flag management.
- Updated `NarrativeState` to include a `Counters` list and cleaned up redundant fields.
- Implemented `NarrativeEventOrchestrator` with support for complex boolean and counter requirements.
- Added domain events as C# records and introduced `IsExternalInit` polyfill in `Core`.
- Added comprehensive EditMode tests covering all new logic, boundary safety, and event emissions.

### File List

- Assets/Scripts/Gameplay/Narrative/INarrativeService.cs
- Assets/Scripts/Gameplay/Narrative/NarrativeService.cs
- Assets/Scripts/Gameplay/Narrative/NarrativeEventOrchestrator.cs
- Assets/Scripts/Gameplay/Narrative/NarrativeEvents.cs
- Assets/Scripts/Gameplay/Narrative/NarrativeState.cs (modified)
- Assets/Scripts/Gameplay/Narrative/NarrativeFlagState.cs (modified)
- Assets/Scripts/Core/IsExternalInit.cs (new)
- Assets/Scripts/Gameplay/Content/NarrativeEventContentDefinition.cs (modified)
- Assets/Scripts/Infrastructure/Content/Authoring/NarrativeEventAuthoring.cs (modified)
- Assets/Scripts/Infrastructure/Content/Conversion/PlaceholderContentConverter.cs (modified)
- Assets/Tests/EditMode/Narrative/NarrativeServiceTests.cs

### Change Log

- 2026-04-26: Implemented narrative service, orchestrator, events, and tests.
- 2026-04-26: Resolved all code-review patch findings: events as records, state model cleanup, efficiency improvements, orchestrator expansion, and boundary safety.
