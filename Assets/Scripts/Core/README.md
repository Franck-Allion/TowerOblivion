# TowerOblivion.Core

Purpose: tiny Unity-free primitives shared by inner gameplay code.

Allowed:
- Result/error primitives.
- Stable ID value types.
- Deterministic seed and RNG-facing abstractions.
- Content/save version value types.

Forbidden:
- UnityEngine, MonoBehaviours, ScriptableObjects, scenes, prefabs, uGUI, PlayerPrefs, Steam, Addressables.
- Domain rules, save file IO, content loading, presentation logic, or gameplay services.

Examples:
- `Result`
- `RunId`
- `RunSeed`
- `ContentVersion`
