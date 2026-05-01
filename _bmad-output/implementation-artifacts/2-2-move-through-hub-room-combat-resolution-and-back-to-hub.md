# Story 2.2: Move Through Hub, Room, Combat, Resolution, and back to Hub

Status: completed

## Story

As a developer,
I want to move through Rebirth Hub, Exploration, Combat, Reward, Resolution, and back to Rebirth Hub using explicit placeholder states,
so that the vertical slice has a playable path.

## Acceptance Criteria

1. A `GameStateOrchestrator` exists in `Gameplay` (Unity-free) to manage high-level game mode transitions for this slice: `RebirthHub`, `Exploration`, `Combat`, `Reward`, and `Resolution`. [DONE]
2. The orchestrator exposes a `CurrentMode` property initialized to `RebirthHub` and a `TransitionTo(GameModeId nextMode)` method returning `Result`. [DONE]
3. Transition logic allows only the Epic 2 Story 2 placeholder loop: `RebirthHub -> Exploration -> Combat -> Reward -> Resolution -> RebirthHub`. [DONE]
4. Invalid transitions return `Result.Failure`, leave `CurrentMode` unchanged, and do not publish a mode-change event. [DONE]
5. Valid transitions update `CurrentMode` and emit exactly one `GameModeChanged` domain event containing the previous and next mode. [DONE]
6. The flow integrates with existing Story 2.1 hub entry/run-start systems without replacing `HubStateOrchestrator`, `HubEntered`, or `RunStarted`. [DONE]
7. A `GameStateOrchestratorPresenter` (or equivalent) in `Presentation` subscribes to `GameModeChanged` and coordinates placeholder UI switching or scene loading through the existing `SceneFader`. [DONE]
8. Placeholder Reward and Resolution UI shells exist in `Presentation` and contain visible actions needed to continue the loop. [DONE]
9. EditMode tests verify initial mode, all valid transitions, invalid transition blocking, and event emission behavior. [DONE]
10. PlayMode tests verify that the full placeholder loop can be navigated through visible actions and ends back at the Hub/RebirthHub UI. [DONE]

## Tasks / Subtasks

- [x] Define the mode identifiers in `Gameplay`. (AC: 1)
  - [x] Add `GameModeId` with exact slice values: `RebirthHub`, `Exploration`, `Combat`, `Reward`, `Resolution`.
- [x] Define the mode-change event in `Gameplay`. (AC: 5)
  - [x] Add `GameModeChanged(GameModeId PreviousMode, GameModeId CurrentMode)` as a record event.
- [x] Add the high-level orchestrator shell. (AC: 1, 2)
  - [x] Create `GameStateOrchestrator`.
  - [x] Inject `IEventBus`.
  - [x] Initialize `CurrentMode` to `GameModeId.RebirthHub`.
- [x] Add the legal transition table. (AC: 3)
  - [x] Allow `RebirthHub -> Exploration`.
  - [x] Allow `Exploration -> Combat`.
  - [x] Allow `Combat -> Reward`.
  - [x] Allow `Reward -> Resolution`.
  - [x] Allow `Resolution -> RebirthHub`.
- [x] Implement transition execution. (AC: 2, 4, 5)
  - [x] Return `Result.Failure` for invalid transitions.
  - [x] Keep `CurrentMode` unchanged on failure.
  - [x] Publish no event on failure.
  - [x] Update `CurrentMode` on success.
  - [x] Publish exactly one `GameModeChanged` on success.
- [x] Integrate with the existing hub flow. (AC: 6)
  - [x] Keep `HubStateOrchestrator` responsible for hub entry and run creation.
  - [x] Ensure the Start Run presentation path requests `RebirthHub -> Exploration`.
  - [x] Do not duplicate `HubEntered`, `StartRunRequested`, or `RunStarted` events.
- [x] Add placeholder presentation controls for the loop. (AC: 7, 8)
  - [x] Add or reuse an Exploration placeholder action to enter Combat.
  - [x] Add or reuse a Combat placeholder action to enter Reward.
  - [x] Add a Reward placeholder shell with a continue action to enter Resolution.
  - [x] Add a Resolution placeholder shell with a return action to enter RebirthHub.
- [x] Wire mode changes to presentation. (AC: 7, 8)
  - [x] Add `GameStateOrchestratorPresenter` or an equivalent scene-flow presenter.
  - [x] Subscribe to `GameModeChanged`.
  - [x] Show the matching placeholder UI shell or load the matching placeholder scene.
  - [x] Use `SceneFader` only from `Presentation`.
- [x] Add EditMode tests for startup and valid transitions. (AC: 9)
  - [x] Test initial mode is `RebirthHub`.
  - [x] Test each legal transition returns success.
  - [x] Test each legal transition updates `CurrentMode`.
  - [x] Test each legal transition publishes one `GameModeChanged`.
- [x] Add EditMode tests for invalid transitions. (AC: 4, 9)
  - [x] Test at least one invalid transition from each mode.
  - [x] Test invalid transitions return `Result.Failure`.
  - [x] Test invalid transitions do not change `CurrentMode`.
  - [x] Test invalid transitions publish no `GameModeChanged`.
- [x] Add a PlayMode full-loop smoke test. (AC: 10)
  - [x] Start from the Hub/RebirthHub UI.
  - [x] Trigger the visible Start Run action.
  - [x] Trigger the visible Exploration-to-Combat action.
  - [x] Trigger the visible Combat-to-Reward action.
  - [x] Trigger the visible Reward-to-Resolution action.
  - [x] Trigger the visible Resolution-to-Hub action.
  - [x] Assert that the final visible state is Hub/RebirthHub.

## Dev Notes

- **Architecture Compliance:** Keep the orchestrator in `Gameplay` (Unity-free). It should only manage the *state* of the loop, not the *implementation* of scene loading.
- **Mode Names:** Use `RebirthHub` for the hub mode to match the architecture vocabulary. Use `Resolution` for the Epic 2 placeholder run-resolution screen, even though the broader architecture also references `GameOver` / `Death/Rebirth` for later scope.
- **Scene Flow:** Prefer simple placeholder UI switching if it is enough for this story. Use the existing `SceneFader` in `Presentation` when changing scenes.
- **Integration:** This story connects the existing Hub, Room/Exploration, and Combat foundations into a single executable placeholder loop.
- **Story 2.1 Compatibility:** Do not replace the existing `HubStateOrchestrator`; this story adds a higher-level mode state machine above the existing hub/run-start behavior.
- **MVP Asset Scope:** Only prepare the minimum placeholder assets needed to make the loop readable. This story does not require final art, animation polish, or production-ready scene dressing.

### MVP Asset Requirements

- **Hub:** Reuse the existing placeholder hub UI from Story 2.1. No new bespoke hub art is required unless the current hub screen is unreadable.
- **Exploration:** Provide one placeholder room background or panel and one clear visible action that advances to Combat. [DONE]
- **Combat:** Provide one placeholder combat shell with a readable board area or combat panel and one clear visible action that advances to Reward. [DONE]
- **Reward:** Provide one placeholder reward shell with one reward card or panel containing a placeholder icon, reward name, short effect text, and a continue action. [DONE]
- **Resolution:** Provide one placeholder resolution shell with an outcome summary area, an optional retained progression/consequence line, and a return-to-hub action. [DONE]
- **Shared UI:** Provide consistent placeholder buttons, labels, and panel styling so the player can identify the primary action at each step without reading logs. [DONE]
- **Transitions:** Reuse the existing `SceneFader` visuals if scenes change. No new transition art is required for MVP. [DONE]
- **Quality Bar:** Assets only need to prove readability and loop comprehension. Functional placeholders are acceptable if primary actions and screen purpose are immediately understandable.

### Project Structure Notes

- `Assets/Scripts/Gameplay/GameStateOrchestrator.cs`
- `Assets/Scripts/Gameplay/GameModeId.cs`
- `Assets/Scripts/Gameplay/GameModeChanged.cs`
- `Assets/Scripts/Presentation/SceneFlow/GameStateOrchestratorPresenter.cs`
- `Assets/Scripts/Presentation/Reward/RewardPlaceholderView.cs`
- `Assets/Scripts/Presentation/Resolution/ResolutionPlaceholderView.cs`
- `Assets/Tests/EditMode/GameStateOrchestrationTests.cs`
- `Assets/Tests/PlayMode/FullLoopSmokeTests.cs`

### Project Context Rules

- **Unity-Free Gameplay:** The orchestrator must not reference `UnityEngine.SceneManagement`.
- **Fail Fast:** Transitions to invalid states must be blocked with `Result.Failure`; Gameplay should not depend on Unity logging.
- **Event-Driven Presentation:** UI actions may request transitions, but UI and scenes must react to `GameModeChanged` rather than mutating flow state directly.
- **Test Boundaries:** Use EditMode tests for pure transition logic. Use PlayMode tests only for Unity integration, placeholder button wiring, and visible UI state.

### References

- [Source: _bmad-output/epics.md#Epic 2: Playable Identity Vertical Slice]
- [Source: _bmad-output/game-architecture.md#Contract 4: Modes Orchestrate Flow And Lifetime]
- [Source: _bmad-output/implementation-artifacts/2-1-arrive-in-placeholder-rebirth-hub.md]

## Dev Agent Record

### Agent Model Used

gemini-2.0-flash-exp (via Gemini CLI)

### Debug Log References

- [DebugModeTransitionTrigger] Transition to mode.combat failed: Cannot transition from mode.combat to mode.combat (Identified double-wiring and synchronization issue)

### Completion Notes List

- Populated `Resolution` scene with Camera, Canvas, Text and Button.
- Fixed double-wiring of `DebugModeTransitionTrigger` in `Rooms`, `Combat`, `Reward`, and `Resolution` scenes.
- Exposed `SceneFader.IsTransitioning` for reliable PlayMode test synchronization.
- Implemented and passed `FullLoopSmokeTests.cs` covering the entire vertical slice loop.

### File List

- `Assets/Scripts/Presentation/SceneFader.cs` (Added `IsTransitioning` property)
- `Assets/Tests/PlayMode/FullLoopSmokeTests.cs` (New PlayMode test)
- `Assets/Scenes/Resolution.unity` (Populated scene)
- `Assets/Scenes/Rooms.unity` (Fixed wiring)
- `Assets/Scenes/Combat.unity` (Fixed wiring)
- `Assets/Scenes/Reward.unity` (Fixed wiring)
