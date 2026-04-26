# Story 1.6: Define Placeholder Combat Encounter Payload So That Exploration Can Enter Combat

Status: done

## Story

As a developer,
I want to define a placeholder combat encounter payload so that exploration can enter combat,
so that the transition from room investigation to tactical board play is established.

## Acceptance Criteria

1. A `CombatStateOrchestrator` exists in `Gameplay/Combat` (Unity-free) to initialize a new `CombatState` from an `EncounterContentDefinition`.
2. The orchestrator populates the `CombatState` initial life values:
   - `HeroLife` from a default value or `PlayerProfileState` (if available).
   - `EnemyLife` from a default placeholder value (e.g. 20) for now.
3. The orchestrator populates the initial hands:
   - `HeroHand` from the current `RunState.RunSouvenirIds`.
   - `EnemyHand` from the `EncounterContentDefinition.SouvenirIds`.
4. The orchestrator assigns a unique `CombatId` and uses the provided `RunSeed` for determinism.
5. The orchestrator emits a `CombatStarted` domain event via `IEventBus`.
6. EditMode tests verify that `CombatState` is correctly initialized with expected hands and life values.
7. Implementation follows the Unity-free Gameplay rule (no `UnityEngine` references).

## Tasks / Subtasks

- [x] Define `CombatStarted` domain event in `Gameplay/Combat`. (AC: 5)
  - [x] Add `CombatStarted` record event containing the initialized `CombatState`.
- [x] Implement `CombatStateOrchestrator` in `Gameplay/Combat`. (AC: 1, 2, 3, 4)
  - [x] Implement `StartCombat(EncounterContentDefinition, RunState, RunSeed)` logic.
  - [x] Initialize `CombatState` with Hero/Enemy life and hands.
  - [x] Publish `CombatStarted` event.
- [x] Add EditMode Tests for Combat initialization. (AC: 6)
  - [x] Test successful `CombatState` setup from an encounter definition.
  - [x] Test that events are published correctly.
  - [x] Verify determinism of the initial state.

### Review Findings

- [x] [Review][Decision] Mutable State Leakage in Events â€” Resolved: Option A (Keep it simple). Passed mutable state directly to subscribers.
- [x] [Review][Patch] Non-deterministic `CombatId` Generation [CombatStateOrchestrator.cs:25] â€” Resolved: CombatId is now derived from encounter ID and seed.
- [x] [Review][Patch] Incomplete `HeroLife` Initialization [CombatStateOrchestrator.cs:30] â€” Resolved: Added support for `PlayerProfileState` parameter.
- [x] [Review][Patch] Missing Determinism Verification in Tests [CombatOrchestrationTests.cs] â€” Resolved: Added test confirming same seed produces same IDs.
- [x] [Review][Patch] Brittle `FakeEventBus` Mocking [CombatOrchestrationTests.cs] â€” Resolved: FakeEventBus now returns a valid `IDisposable`.
- [x] [Review][Defer] Missing `CombatState` in Save Model [SaveSnapshotV1.cs] â€” deferred, pre-existing

## Dev Notes

- **Architecture Compliance:** Keep all combat orchestration in `TowerOblivion.Gameplay` (Unity-free).
- **Event System:** Use the `IEventBus` established in Story 1.4.
- **State Ownership:** `CombatStateOrchestrator` should create the `CombatState` but not necessarily own the long-term reference (which belongs in a `SaveSnapshot`).
- **Placeholder Values:** Use 20 as default `HeroLife` and `EnemyLife` if not otherwise specified in profile/encounter.

### Project Structure Notes

- `Assets/Scripts/Gameplay/Combat/CombatStateOrchestrator.cs`
- `Assets/Scripts/Gameplay/Combat/CombatEvents.cs`
- `Assets/Tests/EditMode/Combat/CombatOrchestrationTests.cs`

### Project Context Rules

- **Unity-Free Gameplay:** `TowerOblivion.Gameplay` must not reference `UnityEngine`. (Source: project-context.md > Engine-Specific Rules)
- **Deterministic RNG:** (Seeds are passed, but not used for randomization in this specific story's logic).
- **Records for Events:** Use C# records for domain events. (Source: project-context.md > Code Organization Rules)

### References

- [Source: _bmad-output/epics.md#Epic 1: Minimal Reusable Game Foundation]
- [Source: _bmad-output/implementation-artifacts/1-1-define-core-state-domains.md]
- [Source: _bmad-output/implementation-artifacts/1-3-define-placeholder-content-in-data.md]
- [Source: _bmad-output/implementation-artifacts/1-4-load-placeholder-room-from-reusable-room-definition.md]

## Dev Agent Record

### Agent Model Used

GPT-5 Codex (via Gemini CLI)

### Debug Log References

- EditMode Test Run: `job_id: 258a29ad2ee84032b0f18965146848b2`, 34/34 Passed.
- Post-Patch EditMode Test Run: `job_id: ac9d8dc1fafe49a2bfaa51a0d09ca755`, 34/34 Passed.

### Completion Notes List

- Defined `CombatStarted` domain event as a C# record in `Gameplay/Combat`.
- Implemented `CombatStateOrchestrator` with deterministic ID generation and profile integration.
- Integrated with `IEventBus` for domain event publication.
- Added comprehensive EditMode tests verifying deterministic combat initialization.
- Resolved all review findings and patches.

### File List

- Assets/Scripts/Gameplay/Combat/CombatEvents.cs
- Assets/Scripts/Gameplay/Combat/CombatStateOrchestrator.cs
- Assets/Tests/EditMode/Combat/CombatOrchestrationTests.cs

### Change Log

- 2026-04-26: Initial implementation of Story 1.6: Combat encounter payload and orchestration.
- 2026-04-26: Applied code review patches: fixed determinism, integrated profile for life, and hardened tests.
