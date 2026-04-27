# Story 2.1: Arrive In Placeholder Rebirth Hub

Status: done

## Story

As a player,
I want to launch the game and arrive in a placeholder rebirth hub,
so that the playable slice has a clear entry point.

## Acceptance Criteria

1. Launching the game (or entering the "Hub" state) results in the display of a placeholder Hub UI.
2. The Hub UI shell contains a "Start Run" button (or equivalent action).
3. The Hub UI shell displays a "Retained Progression Summary" (e.g., current XP, meta-currency, or basic stats from `PlayerProfileState`).
4. The Hub UI shell shows "Load/Save Status Feedback" (e.g., "Ready", "Saving...", or "Last saved at...").
5. The Hub acts as a safe space (logic-wise) where no combat or expiration occurs; it uses the `HubStateOrchestrator` to manage entry/exit.
6. "Start Run" triggers a transition to the first exploration room (Room 1) and initializes a new `RunState`.
7. EditMode tests verify that the orchestrator correctly updates state and publishes a `HubEntered` event.
8. PlayMode tests verify that the Hub UI is visible and the "Start Run" button publishes a `StartRunRequested` event.

## Tasks / Subtasks

- [x] Define Hub Domain Events in `Gameplay`. (AC: 1, 6, 8)
  - [x] Add `HubEntered` record event.
  - [x] Add `StartRunRequested` record event.
- [x] Implement `HubStateOrchestrator` in `Gameplay/Application` (or `Gameplay`). (AC: 1, 5, 6)
  - [x] Implement `EnterHub()` logic.
  - [x] Implement `RequestStartRun()` logic that transitions state from Hub to Run.
  - [x] Ensure orchestrator is Unity-free.
- [x] Implement `HubView` and `HubPresenter` in `Presentation/Hub`. (AC: 1, 2, 3, 4)
  - [x] Create `HubView : MonoBehaviour` with references to uGUI components (Button, TextMeshPro labels).
  - [x] Create `HubPresenter : MonoBehaviour` to bind `PlayerProfileState` and `SaveMetadata` to the view.
  - [x] Wire "Start Run" button to publish `StartRunRequested`.
- [x] Add EditMode Tests for `HubStateOrchestrator`. (AC: 7)
  - [x] Test `EnterHub` publishes event.
  - [x] Test `RequestStartRun` initializes `RunState` and transitions.
- [x] Add PlayMode Test for Hub UI. (AC: 8)
  - [x] Test that the Hub UI is visible on start.
  - [x] Test that clicking "Start Run" publishes the correct event.

## Dev Notes

- **Architecture Compliance:** Keep `HubStateOrchestrator` in `TowerOblivion.Gameplay` (Unity-free).
- **UI Framework:** Use uGUI as per `project-context.md`.
- **Localization:** Use `LocalizedString` for "Start Run" and "Progression".
- **Dependencies:** Relies on Story 1.1 (State Domains) and Story 1.2 (Save Foundation).
- **Save Feedback:** Use `SaveMetadata.SavedAtUtc` to show the "Last saved" feedback.
- **Wiring Fix:** LocalizeStringEvent must use Dynamic mode (m_Mode: 0) and bind to set_text to avoid empty labels at runtime.

### Project Structure Notes

- `Assets/Scripts/Gameplay/HubStateOrchestrator.cs`
- `Assets/Scripts/Gameplay/HubEvents.cs`
- `Assets/Scripts/Presentation/Hub/HubView.cs`
- `Assets/Scripts/Presentation/Hub/HubPresenter.cs`
- `Assets/Tests/EditMode/HubOrchestrationTests.cs`
- `Assets/Tests/PlayMode/Hub/HubViewTests.cs`

### Project Context Rules

- **Unity-Free Gameplay:** `TowerOblivion.Gameplay` must not reference `UnityEngine`. (Source: project-context.md > Engine-Specific Rules)
- **uGUI baseline:** Use uGUI for runtime UI. (Source: project-context.md > Technology Stack & Versions)
- **Records for Events:** Use C# records for domain events. (Source: project-context.md > Code Organization Rules)
- **Save integrity:** Progression summary must come from `PlayerProfileState`. (Source: project-context.md > Save integrity)

### References

- [Source: _bmad-output/epics.md#Epic 2: Playable Identity Vertical Slice]
- [Source: _bmad-output/game-architecture.md#Contract 1: Gameplay Is Unity-Free]
- [Source: _bmad-output/game-architecture.md#Contract 2: Presentation Adapts Unity Objects To Gameplay]
- [Source: _bmad-output/implementation-artifacts/1-1-define-core-state-domains.md]
- [Source: _bmad-output/implementation-artifacts/1-2-create-and-load-local-save.md]

## Dev Agent Record

### Agent Model Used

GPT-5 Codex (via Gemini CLI)

### Debug Log References

- EditMode Tests: `job_id: e879cdda934a4c38a3bbf82bdbeb19fd`, 46/46 Passed.
- PlayMode Tests: `job_id: 0e9ee425077d4d98b2c056b508fc6028`, 3/3 Passed.

### Completion Notes List

- Implemented `HubStateOrchestrator` in `Gameplay` (Unity-free) to manage hub lifecycle.
- Implemented `HubView` and `HubPresenter` using uGUI.
- Defined `HubEntered` and `StartRunRequested` as domain events.
- Added comprehensive EditMode and PlayMode tests.
- Resolved compilation issues by adding missing using directives for relocated IDs and events.
- Wired Hub UI in Unity, optimized for 1920x1080 resolution, and applied corporate font.
- Finalized localization wiring using GUID references and dynamic mode serialization fix.

### File List

- Assets/Scripts/Gameplay/HubEvents.cs
- Assets/Scripts/Gameplay/HubStateOrchestrator.cs
- Assets/Scripts/Presentation/Hub/HubView.cs
- Assets/Scripts/Presentation/Hub/HubPresenter.cs
- Assets/Tests/EditMode/HubOrchestrationTests.cs
- Assets/Tests/PlayMode/Hub/HubViewTests.cs

### Change Log

- 2026-04-26: Initial implementation of Story 2.1: Arrive in rebirth hub.
- 2026-04-26: Finalized Unity wiring, resolution scaling, and localization.
