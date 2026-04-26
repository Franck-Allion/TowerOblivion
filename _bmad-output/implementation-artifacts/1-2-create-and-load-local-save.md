# Story 1.2: Create And Load Local Save

Status: ready-for-dev

## Story

As a developer,  
I want to create and load a local save,  
so that progression can persist between sessions.

## Acceptance Criteria

1. A versioned save snapshot shape exists and is reusable, with explicit metadata including save schema version and content version.
2. Save-facing state is separated from runtime services and Unity objects; snapshots use primitive values, typed IDs, and plain serializable data only.
3. A gameplay-facing persistence port exists (for example `ISaveRepository`) and the local JSON implementation is in `Infrastructure`.
4. Saving and loading persistent progression data (profile + narrative at minimum) works through the port without requiring scene loading.
5. Save/load implementation does not use `PlayerPrefs` for progression data.
6. EditMode tests cover save roundtrip for representative profile/narrative data and verify no Unity scene/prefab dependency.
7. Story scope excludes migration pipeline, backup fallback strategy, corruption recovery flow, Steam Cloud, and UI wiring.

## Tasks / Subtasks

- [ ] Define save snapshot contracts in Unity-free code. (AC: 1, 2)
  - [ ] Add `SaveMetadata` with `SaveVersion`, `ContentVersion`, and `SavedAtUtc`.
  - [ ] Add `SaveSnapshotV1` that stores persistent progression-focused state for MVP (`PlayerProfileState`, `NarrativeState`, and optional `RunState` reference if present).
  - [ ] Keep snapshot contracts free of `UnityEngine`, MonoBehaviours, ScriptableObjects, and scene references.

- [ ] Introduce persistence ports at the gameplay boundary. (AC: 3, 4)
  - [ ] Add minimal interfaces such as `ISaveRepository` and slot/value types in Unity-free code.
  - [ ] Ensure gameplay/application callers depend on ports only, not concrete file IO classes.

- [ ] Implement local JSON save repository in infrastructure. (AC: 3, 4, 5)
  - [ ] Create `Infrastructure/Persistence` implementation for `Save` and `Load`.
  - [ ] Use file-based JSON storage for local saves (not `PlayerPrefs`).
  - [ ] Keep implementation small and MVP-safe: no migration/backups/corruption flow in this story.

- [ ] Wire minimal composition path for save create/load usage without UI coupling. (AC: 4, 7)
  - [ ] Provide a simple callable path (service or bootstrap-safe entry) to create a default snapshot and load an existing snapshot.
  - [ ] Keep this independent of menu/hub UI and scene flow details.

- [ ] Add EditMode tests for save create/load baseline. (AC: 4, 6)
  - [ ] Verify snapshot roundtrip with representative `PlayerProfileState` + `NarrativeState` values.
  - [ ] Verify load behavior for a missing file path returns a safe, explicit result (not crash).
  - [ ] Verify tests run without scenes/prefabs/MonoBehaviours/Steam.

## Dev Notes

Story 1.2 builds directly on Story 1.1 state domains. Keep the implementation focused on **minimal save create/load foundation** only.

### Source Requirements

- Epic 1 Story 2: "As a developer, I can create and load a local save so that progression can persist." [Source: `_bmad-output/epics.md` > Epic 1 > Stories]
- Epic 1 scope includes "Save/load foundation." [Source: `_bmad-output/epics.md` > Epic 1 > Scope]
- Save validation/corruption detection is a separate story concern and should not be over-implemented here. [Source: `_bmad-output/epics.md` > Epic 1 > Stories]

### Architecture Compliance

Mandatory rules for this story:

- `TowerOblivion.Gameplay` remains Unity-free.
- Save ownership split: Gameplay defines save DTO/snapshots and persistence ports; Infrastructure owns JSON file IO.
- Do not use `PlayerPrefs` for progression saves.
- Keep dependencies directional: `Gameplay` -> `Core`, `Infrastructure` -> (`Gameplay`, `Core`), never reverse.
- Do not add Steam/Cloud save coupling in MVP foundation.

[Source: `_bmad-output/game-architecture.md` > Data Persistence]  
[Source: `_bmad-output/game-architecture.md` > Contract 3: Infrastructure Owns Persistence]  
[Source: `_bmad-output/project-context.md` > Engine-Specific Rules, Platform & Build Rules]

### Project Structure Requirements

Expected locations for this story:

```text
Assets/
+-- Scripts/
|   +-- Gameplay/
|   |   +-- Persistence/
|   |       +-- SaveMetadata.cs
|   |       +-- SaveSnapshotV1.cs
|   |       +-- SaveSlot.cs
|   |       +-- ISaveRepository.cs
|   +-- Infrastructure/
|       +-- Persistence/
|           +-- JsonSaveRepository.cs
+-- Tests/
    +-- EditMode/
        +-- Persistence/
            +-- JsonSaveRepositoryTests.cs
```

Use existing asmdef boundaries. If `Infrastructure` asmdef does not exist yet, create the minimal asmdef required and keep references compliant with architecture.

### Previous Story Intelligence

From Story 1.1 (`1-1-define-core-state-domains.md`):

- State models and typed IDs already exist in Unity-free code; reuse them instead of introducing parallel save-specific ID models.
- Keep schema canonical and avoid alias fields that create serialized-contract ambiguity.
- Existing tests already establish Unity-free EditMode patterns; follow the same approach for persistence tests.

### Git Intelligence Summary

Recent commits indicate baseline architecture/docs landed before Story 1.1 implementation:

- `9e51bdd` feat: initial project settings and core state domains
- `44cae28` docs: project context
- `1b8b9ee` docs: epics + architecture + gdd

Use these conventions and avoid introducing divergent folder or naming patterns.

### Testing Requirements

- Prefer EditMode tests for save contracts and repository behavior.
- Include roundtrip coverage for representative persistent data.
- Include missing-save-file behavior coverage with explicit result handling.
- Keep tests deterministic and scene-free.

[Source: `_bmad-output/project-context.md` > Testing Rules]  
[Source: `_bmad-output/game-architecture.md` > Save Compatibility Contract]

### Explicit Non-Goals

- No save migration pipeline implementation.
- No backup/rollback recovery strategy implementation.
- No corruption-repair flow.
- No Steam Cloud.
- No save UI/menu wiring.
- No progression economy or reward-loop changes.

## Project Context Rules

The developer must follow `_bmad-output/project-context.md` before coding. The most relevant rules for this story:

- Keep Gameplay Unity-free and keep `Core` tiny.
- Put persistence adapters in `Assets/Scripts/Infrastructure/Persistence`.
- Do not place persistent state authority in UI/scenes/prefabs/MonoBehaviours.
- Use typed IDs and explicit snapshots/DTOs for save-facing state.
- Use Unity MCP when Unity editor/runtime state must be verified beyond file analysis; mark manual editor work as `Human Action Required`.

## References

- `_bmad-output/epics.md` > Epic 1: Minimal Reusable Game Foundation
- `_bmad-output/game-architecture.md` > Data Persistence
- `_bmad-output/game-architecture.md` > Contract 3: Infrastructure Owns Persistence
- `_bmad-output/game-architecture.md` > Save Compatibility Contract
- `_bmad-output/project-context.md` > Engine-Specific Rules
- `_bmad-output/project-context.md` > Testing Rules
- `_bmad-output/project-context.md` > Unity MCP Usage Policy (Mandatory)
- `_bmad-output/implementation-artifacts/1-1-define-core-state-domains.md`

## Dev Agent Record

### Agent Model Used

To be filled during implementation.

### Debug Log References

### Completion Notes List

### File List

### Change Log

- 2026-04-26: Story created and context-completed for Epic 1 Story 2.
