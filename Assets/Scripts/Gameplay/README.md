# TowerOblivion.Gameplay

Purpose: Unity-free gameplay state, rules, and application-facing domain logic.

Allowed:
- Plain C# state models.
- Gameplay rules and deterministic services.
- Typed IDs and primitives from `TowerOblivion.Core`.
- Collections of primitive or project-owned value objects.

Forbidden:
- UnityEngine, MonoBehaviours, ScriptableObjects, scenes, prefabs, uGUI, PlayerPrefs, Steam, Addressables.
- Unity time/random APIs, Unity serialization assumptions, direct asset references, save file IO, or presentation code.

Examples:
- `RunState`
- `CombatState`
- `PlayerProfileState`
- `NarrativeState`
- `WorldSeedState`
