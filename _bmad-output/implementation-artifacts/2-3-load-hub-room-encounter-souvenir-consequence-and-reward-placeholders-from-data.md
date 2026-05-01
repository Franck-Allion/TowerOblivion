# Story 2.3: Load Hub, Room, Encounter, Souvenir, Consequence, and Reward Placeholders from Data

Status: ready-for-dev

## Story

As a developer,
I want to load the hub, room, encounter, Souvenir, consequence, and reward placeholders from data,
so that the playable identity slice is not hardcoded.

## Acceptance Criteria

1. The Epic 2 placeholder slice has a single validated data set for hub, room, encounter, Souvenir, consequence/narrative event, reward, and optional modifier content.
2. Placeholder content is authored at the Unity edge only, then converted into immutable/plain gameplay DTOs before gameplay systems consume it.
3. Gameplay systems use typed IDs and catalogs for placeholder content lookup; gameplay code does not use raw string IDs except inside typed ID construction, tests, or authoring conversion boundaries.
4. The existing Story 2.2 loop remains intact: `RebirthHub -> Exploration -> Combat -> Reward -> Resolution -> RebirthHub`.
5. Starting the slice loads the configured placeholder room from data and publishes/uses the existing room-load pathway instead of hardcoding room state in presentation.
6. The configured room references the configured encounter and consequence/narrative event by ID, and validation fails if either reference is missing.
7. The configured encounter references the configured Souvenir and reward by ID, and validation fails if either reference is missing.
8. The configured reward references a stable currency/progression ID and non-negative amount, and validation fails for missing currency ID or invalid amount.
9. The configured consequence/narrative event uses a stable typed flag/counter ID and can be evaluated by the narrative service/orchestrator without scene or UI authority.
10. The combat stub can initialize from the configured encounter/Souvenir data without implementing the full Epic 3 combat system.
11. The Reward and Resolution placeholder screens display data-derived labels/values where those labels already exist in the content definitions; no final localization/art pipeline is required in this story.
12. Missing, duplicate, empty, or broken placeholder content fails closed with `Result.Failure` or validation failure before the playable loop proceeds.
13. EditMode tests cover conversion, catalog lookup, duplicate IDs, missing references, invalid reward data, and deterministic lookup order.
14. PlayMode or integration tests verify the visible slice still moves through the Story 2.2 loop using data-backed placeholder content.
15. No new top-level systems, generic engine framework, Addressables pipeline, full combat AI, full reward economy, or final art/audio polish are introduced.

## Tasks / Subtasks

- [ ] Confirm and preserve existing content foundations. (AC: 1, 2, 3)
  - [ ] Reuse existing content DTOs under `Assets/Scripts/Gameplay/Content` where present.
  - [ ] Reuse or extend existing authoring/conversion/validation code under `Assets/Scripts/Infrastructure/Content`.
  - [ ] Keep all Unity `ScriptableObject` usage in Infrastructure/Authoring or composition/bootstrap code, not Gameplay.

- [ ] Define the required placeholder fixture IDs and relationships. (AC: 1, 6, 7, 8, 9)
  - [ ] Include one hub/default run entry reference if the current hub bootstrap needs it.
  - [ ] Include one room, for example `room.entry`.
  - [ ] Include one encounter, for example `encounter.entry`.
  - [ ] Include one Souvenir, for example `souvenir.broken_laurel`.
  - [ ] Include one consequence/narrative event, for example `narrative.prometheus_whisper`.
  - [ ] Include one reward, for example `reward.memory_embers`.
  - [ ] Include stable currency/flag IDs, for example `currency.memory_embers` and `flag.prometheus_contacted`.

- [ ] Create or wire the placeholder data set. (AC: 1, 2, 5, 6, 7, 8)
  - [ ] If using ScriptableObjects, place authored assets under `Assets/Data/Authoring`.
  - [ ] If using an in-memory fixture for this slice, keep it in Infrastructure or bootstrap composition and make it replaceable by authoring assets later.
  - [ ] Ensure converted runtime catalogs are immutable/read-only from Gameplay's perspective.
  - [ ] Do not mutate ScriptableObject assets at runtime.

- [ ] Validate content before gameplay flow begins. (AC: 6, 7, 8, 12)
  - [ ] Check empty IDs, duplicate IDs, missing room -> encounter links, missing room -> narrative event links, missing encounter -> Souvenir links, missing encounter -> reward links, and invalid reward currency/amount.
  - [ ] Return stable validation error codes such as `content.duplicate_id`, `content.missing_reference`, or `content.invalid_reward`.
  - [ ] Block or visibly fail the slice startup if critical placeholder content is invalid.

- [ ] Wire data-backed loading into the existing loop. (AC: 4, 5, 10, 11)
  - [ ] Keep `GameStateOrchestrator` as the mode-transition owner from Story 2.2.
  - [ ] Keep `HubStateOrchestrator` responsible for hub entry/run-start behavior from Story 2.1.
  - [ ] Load the configured room through `RoomStateOrchestrator` / `IRoomLoader` / `RoomLoaded` instead of presentation-only hardcoding.
  - [ ] Start the combat stub from the configured encounter/Souvenir data, but keep combat rules minimal.
  - [ ] Populate reward/resolution placeholder labels from data where practical; use readable fallback text only at the presentation edge.

- [ ] Preserve architecture boundaries. (AC: 2, 3, 9, 10, 15)
  - [ ] `Gameplay` may define DTOs, typed IDs, catalogs, orchestrators, and events.
  - [ ] `Infrastructure` owns authoring conversion, validation, catalog construction, and persistence-facing loading.
  - [ ] `Presentation` owns scene/UI binding only and reacts to gameplay state/events.
  - [ ] Do not add UnityEngine references to Gameplay.
  - [ ] Do not add Steam, Addressables, custom DI, full localization migration, or a generic content pipeline.

- [ ] Add tests. (AC: 12, 13, 14)
  - [ ] Add/extend EditMode tests for valid conversion and deterministic catalog lookup.
  - [ ] Add/extend EditMode tests for duplicate IDs.
  - [ ] Add/extend EditMode tests for missing references.
  - [ ] Add/extend EditMode tests for invalid reward currency/amount.
  - [ ] Add/extend an integration or PlayMode smoke test proving the Story 2.2 loop still completes with data-backed room/encounter/reward content.

## Dev Notes

### Current Repository Signals

- The repository already contains gameplay content DTOs and typed IDs under `Assets/Scripts/Gameplay/Content`.
- The repository already contains infrastructure authoring, conversion, validation, and in-memory catalog classes under `Assets/Scripts/Infrastructure/Content`.
- The repository already contains `RoomStateOrchestrator`, `IRoomLoader`, `RoomLoaded`, `CombatStateOrchestrator`, `NarrativeEventOrchestrator`, and `ContentService` foundations.
- Treat those as the intended path. Do not create a parallel content system in Presentation, scenes, or prefabs.

### Previous Story Intelligence

- Story 2.2 established the explicit placeholder loop and completed it through visible actions.
- Keep `GameStateOrchestrator`, `GameModeId`, and `GameModeChanged` as the high-level flow contract.
- Do not replace `HubStateOrchestrator`, `HubEntered`, `StartRunRequested`, or `RunStarted`.
- Story 2.2 found scene double-wiring risks in debug transition triggers. Check scene/button wiring carefully when connecting data-backed flow.
- `SceneFader.IsTransitioning` exists for PlayMode synchronization; reuse it for loop smoke tests if scene transitions are involved.

### Architecture Compliance

- Gameplay remains Unity-free. No `UnityEngine`, `MonoBehaviour`, `ScriptableObject`, scene, prefab, localization component, or file IO references in Gameplay.
- ScriptableObjects are authoring inputs only. Runtime systems consume validated DTO catalogs.
- Content IDs, room IDs, encounter IDs, Souvenir IDs, reward IDs, currency IDs, and flag IDs must be stable typed IDs.
- Events are typed records, not string event names.
- Expected content/gameplay failures return `Result.Failure`; do not use exceptions for normal missing-content or invalid-action paths.
- Combat in this story is still a stub. Do not implement full card placement rules, enemy AI, balancing, or Epic 3 combat resolution.

### Project Structure Notes

- Likely existing files to reuse or extend:
  - `Assets/Scripts/Gameplay/Content/*`
  - `Assets/Scripts/Gameplay/ContentService.cs`
  - `Assets/Scripts/Gameplay/RoomStateOrchestrator.cs`
  - `Assets/Scripts/Gameplay/IRoomLoader.cs`
  - `Assets/Scripts/Gameplay/Combat/CombatStateOrchestrator.cs`
  - `Assets/Scripts/Gameplay/Narrative/NarrativeEventOrchestrator.cs`
  - `Assets/Scripts/Infrastructure/Content/InMemoryContentCatalog.cs`
  - `Assets/Scripts/Infrastructure/Content/RoomLoader.cs`
  - `Assets/Scripts/Infrastructure/Content/Authoring/*`
  - `Assets/Scripts/Infrastructure/Content/Conversion/*`
  - `Assets/Scripts/Infrastructure/Content/Validation/*`
  - `Assets/Scripts/Presentation/GlobalBootstrapper.cs`
  - `Assets/Scripts/Presentation/SceneFlow/GameStateOrchestratorPresenter.cs`
  - `Assets/Scripts/Presentation/Reward/RewardPlaceholderView.cs`
  - `Assets/Scripts/Presentation/Resolution/ResolutionPlaceholderView.cs`
- Likely tests to reuse or extend:
  - `Assets/Tests/EditMode/Content/PlaceholderContentPipelineTests.cs`
  - `Assets/Tests/EditMode/Exploration/RoomOrchestrationTests.cs`
  - `Assets/Tests/EditMode/Combat/CombatOrchestrationTests.cs`
  - `Assets/Tests/EditMode/Narrative/NarrativeServiceTests.cs`
  - `Assets/Tests/PlayMode/FullLoopSmokeTests.cs`

### Project Context Rules

- Use uGUI for runtime UI. Do not switch this story to UI Toolkit.
- Use Unity Input System only if input binding changes are required; otherwise leave input unchanged.
- Follow the existing assembly boundaries: `Core` depends on nothing, `Gameplay` depends on `Core`, and `Infrastructure` / `Presentation` depend inward.
- Use `Assets/Data/Authoring` for ScriptableObject authored data if creating assets.
- Do not place gameplay rules inside prefabs, scenes, presenters, or ScriptableObjects.
- Localization is required for displayed text in the target architecture, but this story may use existing placeholder display keys/fallbacks if the localization table pipeline is not yet ready. Do not build the full localization pipeline here.
- Content data must fail closed. Missing/invalid content, unknown IDs, duplicate IDs, invalid references, or broken prerequisites must produce clear validation errors and safe fallback behavior.
- Unity MCP should be used by the implementing agent for Unity scene/assets wiring and PlayMode verification if available.
- Context7 should be used by the implementing agent before relying on current Unity package/API details.

### References

- [Source: _bmad-output/epics.md#Epic 2: Playable Identity Vertical Slice]
- [Source: _bmad-output/epics.md#Placeholder UI and Asset Scope]
- [Source: _bmad-output/game-architecture.md#Content Data]
- [Source: _bmad-output/game-architecture.md#Implementation Rules Summary]
- [Source: _bmad-output/project-context.md#Critical Implementation Rules]
- [Source: _bmad-output/implementation-artifacts/2-2-move-through-hub-room-combat-resolution-and-back-to-hub.md]

## Dev Agent Record

### Agent Model Used

TBD by dev agent

### Debug Log References

### Completion Notes List

### File List
