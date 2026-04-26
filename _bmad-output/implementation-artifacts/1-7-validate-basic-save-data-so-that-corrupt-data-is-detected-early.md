# Story 1.7: Validate Basic Save Data So That Corrupt Data Is Detected Early

Status: done

## Story

As a developer,
I want to validate basic save data so that corrupt data is detected early,
so that player progression remains reliable and crashes due to malformed state are prevented.

## Acceptance Criteria

1. A `SaveDataValidator` service exists in `Infrastructure/Persistence` to check for structural integrity and essential values in `SaveSnapshotV1` (or equivalent).
2. The validator verifies at minimum:
   - Required fields are present and not null (e.g. `RunState`, `PlayerProfileState`).
   - `ContentVersion` matches the current supported version or a known compatible version.
   - Numeric values are within sane ranges (e.g. floor index >= 0, life >= 0).
   - Typed IDs follow the expected naming convention (lowercase dot/snake notation).
3. The `SaveGameService` (or equivalent load path) uses the validator during the load process.
4. If validation fails, the system returns a detailed `Result.Failure` and prevents loading of corrupt data.
5. The system supports a "safe fallback" or "backup recovery" flow if the primary save is corrupt (AC: 8 in Epic 1 mentions reusability and primitive seams).
6. EditMode tests cover:
   - Successful validation of a perfect save snapshot.
   - Failure on missing mandatory fields.
   - Failure on incompatible content versions.
   - Failure on out-of-range numeric values.
7. Implementation follows the hybrid architecture: validation logic is in `Infrastructure`, results are returned to `Gameplay`/Orchestration.

## Tasks / Subtasks

- [x] Implement `SaveDataValidator` in `Infrastructure/Persistence`. (AC: 1, 2)
  - [x] Add structural checks (null/missing sections).
  - [x] Add value-range checks (floor, life, amount).
  - [x] Add version compatibility checks.
- [x] Integrate validation into `SaveGameService` (or `JsonSaveRepository`). (AC: 3, 4, 5)
  - [x] Update the load method to call the validator before returning data.
  - [x] Ensure `Result.Failure` is propagated with specific error codes (e.g. `save.corrupt`, `save.incompatible_version`).
- [x] Add EditMode Tests for Save Validation. (AC: 6)
  - [x] Test roundtrip with valid data.
  - [x] Test rejection of invalid floor index.
  - [x] Test rejection of missing profile state.
  - [x] Test rejection of mismatched content version.

### Review Findings

- [x] [Review][Decision] Rigid Version Check â€” Resolved: Option B (Soften check). Now allows major version matches while tolerating minor/patch differences.
- [x] [Review][Patch] Missing Backup/Fallback Logic: Resolved. Added `.bak` file rotation and fallback on load failure. [JsonSaveRepository.cs:80]
- [x] [Review][Patch] Incomplete Object Validation: Resolved. Added checks for `PlayerProfile` and `Narrative`. [SaveDataValidator.cs:22]
- [x] [Review][Patch] Missing Typed ID Convention Validation: Resolved. Added regex validation for IDs. [SaveDataValidator.cs]
- [x] [Review][Patch] Inconsistent Null Validation in Repo: Resolved. Made validator optional in constructor. [JsonSaveRepository.cs:20]
- [x] [Review][Patch] Non-Atomic File Writes: Resolved. Implemented write-to-temp-then-move pattern. [JsonSaveRepository.cs:51]
- [x] [Review][Defer] Hardcoded Error Strings [SaveDataValidator.cs] â€” deferred, pre-existing

## Dev Notes

- **Architecture Compliance:** Validation is a technical integration concern; place it in `TowerOblivion.Infrastructure`.
- **Error Codes:** Use standard codes like `save.corrupt`, `save.incompatible_version`, `save.invalid_field`.
- **Version Handling:** Compare `SaveSnapshot.ContentVersion` against the static `ContentVersion` defined in Story 1.3.
- **Fail Fast:** The goal is to detect issues *before* state is injected into gameplay systems.

### Project Structure Notes

- `Assets/Scripts/Infrastructure/Persistence/SaveDataValidator.cs`
- `Assets/Scripts/Infrastructure/Persistence/JsonSaveRepository.cs` (Updated load path)
- `Assets/Tests/EditMode/Persistence/SaveValidationTests.cs`

### Project Context Rules

- **Engine-Specific Rules:** Infrastructure owns JSON file IO, versioning, and validation. (Source: project-context.md > Engine-Specific Rules)
- **Engine-Specific Rules:** Invalid authoring data must fail fast... before gameplay starts. (Apply similar logic to save data). (Source: project-context.md > Engine-Specific Rules)
- **Testing Rules:** Save tests must cover roundtrip, schema version, migration, and corrupt primary save. (Source: project-context.md > Testing Rules)
- **Critical Don't-Miss Rules:** Do not bypass save validation/recovery because "it is only MVP." (Source: project-context.md > Critical Don't-Miss Rules)

### References

- [Source: _bmad-output/epics.md#Epic 1: Minimal Reusable Game Foundation]
- [Source: _bmad-output/implementation-artifacts/1-2-create-and-load-local-save.md]
- [Source: _bmad-output/implementation-artifacts/1-3-define-placeholder-content-in-data.md]
- [Source: _bmad-output/game-architecture.md#Contract 3: Infrastructure Owns Persistence]

## Dev Agent Record

### Agent Model Used

GPT-5 Codex (via Gemini CLI)

### Debug Log References

- EditMode Test Run: `job_id: faa28cb322924d17ab135dd4457e57b1`, 41/41 Passed.
- Post-Patch EditMode Test Run: `job_id: a1cfc18605344c4ab1e24a148de68ec6`, 41/41 Passed.

### Completion Notes List

- Created `SaveDataValidator` in `Infrastructure` to validate save snapshots against content version and sane value ranges.
- Integrated `SaveDataValidator` into `JsonSaveRepository` (both `Save` and `Load` paths).
- Implemented atomic save operations using temp files.
- Implemented backup fallback logic (loads `.bak` if primary is corrupt).
- Added regex-based ID convention validation.
- Softened version check to allow matching major versions.
- Updated `JsonSaveRepositoryTests` and `SaveValidationTests` to verify all new logic.

### File List

- Assets/Scripts/Infrastructure/Persistence/SaveDataValidator.cs
- Assets/Scripts/Infrastructure/Persistence/JsonSaveRepository.cs
- Assets/Tests/EditMode/Persistence/SaveValidationTests.cs
- Assets/Tests/EditMode/Persistence/JsonSaveRepositoryTests.cs

### Change Log

- 2026-04-26: Initial implementation of Story 1.7: Save data validation and repository integration.
- 2026-04-26: Applied code review patches: softened version check, backup fallback, atomic writes, ID validation, and exhaustive null checks.
