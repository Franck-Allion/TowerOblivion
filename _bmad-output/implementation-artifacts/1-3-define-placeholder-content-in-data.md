# Story 1.3: Define Placeholder Content In Data

Status: done

## Story

As a developer,  
I want to define placeholder content in data,  
so that systems are not hardcoded.

## Acceptance Criteria

1. Unity-free runtime DTO definitions exist for placeholder content domains required by Epic 1 (`Room`, `Encounter`, `Souvenir`, `Reward`, `NarrativeEvent`, `Modifier`) and use typed IDs/value objects instead of raw runtime string identifiers.
2. Placeholder content DTOs are immutable/read-only at runtime and contain only data needed for MVP seams; no gameplay rule execution logic is embedded in DTOs.
3. Authoring-side placeholder content structures exist in `Infrastructure` as ScriptableObject inputs only (or equivalent authoring inputs), with stable ID fields and no mutable runtime state authority.
4. A minimal conversion seam from authoring inputs to runtime DTO catalogs exists in `Infrastructure/Content` and keeps `Gameplay` independent from Unity types.
5. A minimal catalog abstraction exists for content lookup (for example `IContentCatalog<TId, TDto>` or story-scoped equivalent) and supports deterministic, read-only lookup behavior.
6. Baseline validation for placeholder content catches at least duplicate IDs and missing required references before gameplay consumption.
7. EditMode tests cover representative conversion, validation, and catalog lookup paths without scenes, prefabs, MonoBehaviours, Steam, or `PlayerPrefs`.
8. Story scope excludes full production content authoring workflow, full balancing toolchain, large content volume, advanced import pipeline automation, and full combat/narrative/progression implementation.

## Tasks / Subtasks

- [x] Define Unity-free placeholder DTO contracts in gameplay-facing code. (AC: 1, 2)
  - [x] Add minimal DTOs for placeholder room, encounter, souvenir, reward, narrative event, and modifier content.
  - [x] Use existing typed IDs from `TowerOblivion.Core` instead of introducing parallel ID systems.
  - [x] Keep DTOs read-only/value-focused and free of Unity runtime types.

- [x] Add placeholder catalog interfaces and baseline read-only implementations. (AC: 4, 5)
  - [x] Add a catalog seam in gameplay-facing code (`TryGet`, optional `GetAll`/enumeration as needed).
  - [x] Add a minimal infrastructure-side in-memory catalog implementation for converted placeholder data.
  - [x] Keep dependency direction compliant: `Gameplay` -> `Core`; `Infrastructure` may reference `Gameplay` + `Core`.

- [x] Define authoring input placeholders under infrastructure/editor-facing boundaries. (AC: 3)
  - [x] Add minimal ScriptableObject authoring classes for the placeholder domains in `Infrastructure/Content/Authoring` (or nearest compliant folder).
  - [x] Ensure authoring classes contain stable IDs and basic authored fields only.
  - [x] Do not place mutable run state or gameplay logic in authoring assets.

- [x] Implement placeholder conversion pipeline from authoring inputs to runtime DTO catalogs. (AC: 3, 4, 5)
  - [x] Add converter classes in `Assets/Scripts/Infrastructure/Content` to map authoring data to runtime DTOs.
  - [x] Keep conversion explicit and deterministic.
  - [x] Ensure Gameplay code consumes only converted DTOs/catalog abstractions.

- [x] Implement baseline placeholder content validation. (AC: 6)
  - [x] Validate duplicate IDs per content domain.
  - [x] Validate required references for representative placeholder links (for example encounter -> reward/modifier IDs where defined).
  - [x] Return explicit `Result`/error details on invalid content.

- [x] Add EditMode tests for content placeholder foundations. (AC: 7)
  - [x] Test conversion of representative authoring inputs to runtime DTOs.
  - [x] Test duplicate-ID and missing-reference validation failure paths.
  - [x] Test catalog lookup determinism and not-found behavior.
  - [x] Keep tests scene-free and Unity-runtime-light (no scene loading/prefab dependency).

### Review Findings

- [x] [Review][Patch] Mutable ID Types: Refactored 18 ID structs in `Core` to use a public field `Value` for `JsonUtility` compatibility without `UnityEngine` dependency.
- [x] [Review][Patch] Silent Duplicate Handling in Catalogs: Updated `InMemoryContentCatalog` to throw `ArgumentException` on duplicate IDs.
- [x] [Review][Patch] Null/Missing Reference Handling in Converter: `PlaceholderContentConverter.AddIds` now filters out null or empty ID strings.
- [x] [Review][Patch] Validation Gap for Empty IDs: Added `HasInvalidIds` check to `PlaceholderContentValidator`.
- [x] [Review][Patch] Missing Validation for Reward Currency: Added `CurrencyId` validation to `PlaceholderContentValidator`.
- [x] [Review][Patch] Public Fields in Authoring Classes: Encapsulated authoring fields using `[SerializeField] internal` and public properties; updated tests accordingly.
- [x] [Review][Patch] Test Fix: Switched `EditMode` tests from `XmlSerializer` to `JsonUtility` to support IDs with private fields and align with runtime persistence.
- [x] [Review][Defer] Persistence Atomicity & Safety [`Assets/Scripts/Infrastructure/Persistence/JsonSaveRepository.cs`] — deferred, pre-existing.

## Dev Notes

Story 1.3 is the data-definition seam for future room/combat/narrative/progression implementation. Keep it intentionally small and focused on contracts, conversion seams, and validation guardrails.

### Source Requirements

- Epic 1 Story 3: "As a developer, I can define placeholder content in data so that systems are not hardcoded." [Source: `_bmad-output/epics.md` > Epic 1 > Stories]
- Epic 1 includes "Data-driven content definitions" and "Content registry structure" in scope. [Source: `_bmad-output/epics.md` > Epic 1 > Scope]
- Epic 1 excludes full gameplay systems and chapter content; keep this story to placeholders and seams only. [Source: `_bmad-output/epics.md` > Epic 1 > Excludes]

### Architecture Compliance

Mandatory rules for this story:

- `TowerOblivion.Gameplay` remains Unity-free; no `UnityEngine`, MonoBehaviours, ScriptableObjects, scenes, prefabs, uGUI, Steam, or file IO in gameplay contracts.
- ScriptableObjects are authoring inputs only; runtime systems consume validated immutable DTO catalogs.
- Runtime catalogs must be read-only definitions consumed by gameplay systems through abstractions.
- Validation must fail fast on invalid content before gameplay consumption.
- Keep asmdef dependency direction intact (`Core` <- `Gameplay`; `Infrastructure` may depend on `Gameplay` and `Core`).

[Source: `_bmad-output/game-architecture.md` > Runtime Catalog Pattern]  
[Source: `_bmad-output/game-architecture.md` > Contract 3: Infrastructure Owns Persistence, Content, Platform, and Logging Implementations]  
[Source: `_bmad-output/game-architecture.md` > Assembly Dependency Contract]  
[Source: `_bmad-output/project-context.md` > Engine-Specific Rules]

### Project Structure Requirements

Expected locations for this story:

```text
Assets/
+-- Scripts/
|   +-- Gameplay/
|   |   +-- Content/
|   |       +-- ... placeholder DTOs and catalog interfaces
|   +-- Infrastructure/
|       +-- Content/
|           +-- Authoring/
|           |   +-- ... ScriptableObject placeholder definitions
|           +-- Conversion/
|           |   +-- ... authoring-to-DTO converters
|           +-- Validation/
|               +-- ... content validators
+-- Tests/
    +-- EditMode/
        +-- Content/
            +-- ... conversion/validation/catalog tests
```

Prefer extending existing assemblies and folders rather than introducing new top-level modules.

### Previous Story Intelligence

From Story 1.2 (`1-2-create-and-load-local-save.md`) and Story 1.1 (`1-1-define-core-state-domains.md`):

- Reuse existing `TowerOblivion.Core` typed IDs and version types; do not duplicate ID families.
- Keep contracts deterministic and explicit (`Result`-based failures, no silent fallback behavior).
- Maintain strict boundary ownership (Gameplay contracts + Infrastructure adapters).
- Keep test style aligned with existing EditMode patterns and deterministic fixture construction.

### Git Intelligence Summary

Recent repository history relevant to this story:

- `8706c7b` feat: story 1.2 - create and load local save
- `30edc8d` feat: story 1.1 - define core state domain
- `9e51bdd` feat: add initial project settings and core state domain definitions

Use established naming and assembly conventions from those commits; avoid introducing parallel patterns.

### Testing Requirements

- Prefer EditMode tests for DTO contracts, conversion, validation, and catalog lookup behavior.
- Verify invalid content fails with explicit errors (duplicate IDs, missing required references).
- Verify representative valid content converts successfully and is retrievable through catalogs.
- Do not require scenes, prefabs, MonoBehaviours, Steam, `PlayerPrefs`, `UnityEngine.Random`, or `Time.time` for these tests.

[Source: `_bmad-output/project-context.md` > Testing Rules]  
[Source: `_bmad-output/game-architecture.md` > Testing Contract]

### Explicit Non-Goals

- No full content authoring workflow tooling.
- No large production content drop.
- No build-time codegen pipeline for catalogs.
- No full combat effect system, full room generation, full narrative event engine, or progression economy logic.
- No Addressables/ECS/custom DI container introduction.
- No UI/content editor tooling beyond minimal placeholder authoring classes needed for this seam.

## Project Context Rules

The developer must follow `_bmad-output/project-context.md` before coding. Most relevant rules for this story:

- Keep Gameplay Unity-free and deterministic.
- ScriptableObject assets are authoring-only; runtime consumes validated immutable DTOs.
- Gameplay catalogs reference stable IDs, not direct asset references.
- Invalid authoring data must fail fast before gameplay start.
- Do not use raw string IDs in gameplay code; use typed IDs/registries.
- Keep folder/assembly boundaries aligned with architecture and avoid generic `Managers`/`Helpers` dumping grounds.
- Use Unity MCP when Unity editor/runtime state must be verified beyond source inspection.

## References

- `_bmad-output/epics.md` > Epic 1: Minimal Reusable Game Foundation
- `_bmad-output/game-architecture.md` > Runtime Catalog Pattern
- `_bmad-output/game-architecture.md` > Contract 3: Infrastructure Owns Persistence, Content, Platform, and Logging Implementations
- `_bmad-output/game-architecture.md` > Assembly Dependency Contract
- `_bmad-output/project-context.md` > Engine-Specific Rules
- `_bmad-output/project-context.md` > Testing Rules
- `_bmad-output/implementation-artifacts/1-1-define-core-state-domains.md`
- `_bmad-output/implementation-artifacts/1-2-create-and-load-local-save.md`

## Dev Agent Record

### Agent Model Used

GPT-5 Codex

### Debug Log References

- Unity MCP EditMode test run passed after implementation:
  `job_id=63c5883bc521407889d7368a3d8af72b`, `total=21`, `passed=21`, `failed=0`, `skipped=0`.
- Unity MCP console check after run returned no active errors/warnings tied to the implemented content pipeline code.

### Completion Notes List

- Added new typed IDs in Core for placeholder content domains: `RewardId`, `NarrativeEventId`.
- Implemented read-only gameplay content contracts in `Assets/Scripts/Gameplay/Content` for room, encounter, souvenir, reward, narrative event, and modifier definitions.
- Added generic gameplay-facing content catalog interface and infrastructure in-memory catalog implementation for deterministic lookup.
- Added ScriptableObject authoring placeholders for all six content domains in `Infrastructure/Content/Authoring`.
- Implemented authoring-to-runtime conversion seam in `Infrastructure/Content/Conversion/PlaceholderContentConverter`.
- Implemented baseline validation for duplicate IDs and missing references in `Infrastructure/Content/Validation/PlaceholderContentValidator`.
- Added EditMode tests covering conversion success, duplicate-ID failure, missing-reference failure, and not-found lookup behavior.
- Generated and included Unity `.meta` files for all new folders/files via Unity asset refresh.
- Resolved all code-review patch findings (ID immutability, duplicate handling, validation gaps, and test serialization).

### File List

- Assets/Scripts/Core/NarrativeEventId.cs
- Assets/Scripts/Core/NarrativeEventId.cs.meta
- Assets/Scripts/Core/RewardId.cs
- Assets/Scripts/Core/RewardId.cs.meta
- Assets/Scripts/Gameplay/Content.meta
- Assets/Scripts/Gameplay/Content/EncounterContentDefinition.cs
- Assets/Scripts/Gameplay/Content/EncounterContentDefinition.cs.meta
- Assets/Scripts/Gameplay/Content/IContentCatalog.cs
- Assets/Scripts/Gameplay/Content/IContentCatalog.cs.meta
- Assets/Scripts/Gameplay/Content/ModifierContentDefinition.cs
- Assets/Scripts/Gameplay/Content/ModifierContentDefinition.cs.meta
- Assets/Scripts/Gameplay/Content/NarrativeEventContentDefinition.cs
- Assets/Scripts/Gameplay/Content/NarrativeEventContentDefinition.cs.meta
- Assets/Scripts/Gameplay/Content/PlaceholderContentCatalogs.cs
- Assets/Scripts/Gameplay/Content/PlaceholderContentCatalogs.cs.meta
- Assets/Scripts/Gameplay/Content/RewardContentDefinition.cs
- Assets/Scripts/Gameplay/Content/RewardContentDefinition.cs.meta
- Assets/Scripts/Gameplay/Content/RoomContentDefinition.cs
- Assets/Scripts/Gameplay/Content/RoomContentDefinition.cs.meta
- Assets/Scripts/Gameplay/Content/SouvenirContentDefinition.cs
- Assets/Scripts/Gameplay/Content/SouvenirContentDefinition.cs.meta
- Assets/Scripts/Infrastructure/TowerOblivion.Infrastructure.asmdef
- Assets/Scripts/Infrastructure/Content.meta
- Assets/Scripts/Infrastructure/Content/Authoring.meta
- Assets/Scripts/Infrastructure/Content/Authoring/EncounterAuthoring.cs
- Assets/Scripts/Infrastructure/Content/Authoring/EncounterAuthoring.cs.meta
- Assets/Scripts/Infrastructure/Content/Authoring/ModifierAuthoring.cs
- Assets/Scripts/Infrastructure/Content/Authoring/ModifierAuthoring.cs.meta
- Assets/Scripts/Infrastructure/Content/Authoring/NarrativeEventAuthoring.cs
- Assets/Scripts/Infrastructure/Content/Authoring/NarrativeEventAuthoring.cs.meta
- Assets/Scripts/Infrastructure/Content/Authoring/RewardAuthoring.cs
- Assets/Scripts/Infrastructure/Content/Authoring/RewardAuthoring.cs.meta
- Assets/Scripts/Infrastructure/Content/Authoring/RoomAuthoring.cs
- Assets/Scripts/Infrastructure/Content/Authoring/RoomAuthoring.cs.meta
- Assets/Scripts/Infrastructure/Content/Authoring/SouvenirAuthoring.cs
- Assets/Scripts/Infrastructure/Content/Authoring/SouvenirAuthoring.cs.meta
- Assets/Scripts/Infrastructure/Content/Conversion.meta
- Assets/Scripts/Infrastructure/Content/Conversion/PlaceholderContentAuthoringSet.cs
- Assets/Scripts/Infrastructure/Content/Conversion/PlaceholderContentAuthoringSet.cs.meta
- Assets/Scripts/Infrastructure/Content/Conversion/PlaceholderContentConverter.cs
- Assets/Scripts/Infrastructure/Content/Conversion/PlaceholderContentConverter.cs.meta
- Assets/Scripts/Infrastructure/Content/InMemoryContentCatalog.cs
- Assets/Scripts/Infrastructure/Content/InMemoryContentCatalog.cs.meta
- Assets/Scripts/Infrastructure/Content/Validation.meta
- Assets/Scripts/Infrastructure/Content/Validation/PlaceholderContentValidator.cs
- Assets/Scripts/Infrastructure/Content/Validation/PlaceholderContentValidator.cs.meta
- Assets/Tests/EditMode/Content.meta
- Assets/Tests/EditMode/Content/PlaceholderContentPipelineTests.cs
- Assets/Tests/EditMode/Content/PlaceholderContentPipelineTests.cs.meta
- _bmad-output/implementation-artifacts/1-3-define-placeholder-content-in-data.md

### Change Log

- 2026-04-26: Story created and context-completed for Epic 1 Story 3.
- 2026-04-26: Implemented placeholder content DTOs, authoring assets, conversion/validation pipeline, and EditMode coverage; set story status to review.
- 2026-04-26: Resolved all code-review patch findings and set story status to done.
