# Deferred Work

## Deferred from: code review 1-3-define-placeholder-content-in-data (2026-04-26)

- **Persistence Atomicity & Safety:** `JsonSaveRepository` performs direct file writes which can lead to corruption on failure; it lacks thread-safety and robust handling for future schema versions. (Source: Edge Case Hunter)

## Deferred from: code review 1-6-define-placeholder-combat-encounter-payload-so-that-exploration-can-enter-combat (2026-04-26)

- **Missing CombatState in Save Model:** `SaveSnapshotV1` does not yet include a field for `CombatState`. (Source: Edge Case Hunter)
