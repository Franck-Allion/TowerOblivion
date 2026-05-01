---
project_name: 'IA'
game_name: 'Tower Oblivion'
user_name: 'Franck'
date: '2026-04-28'
sections_completed: ['technology_stack', 'engine_rules', 'performance_rules', 'code_organization_rules', 'testing_rules', 'platform_build_rules', 'critical_dont_miss_rules']
status: 'complete'
rule_count: 113
section_count: 7
optimized_for_llm: true
existing_patterns_found: 0
source_documents:
  architecture: 'D:\Dev\Unity\TowerOblivion\_bmad-output\game-architecture.md'
  gdd: 'D:\Dev\Unity\TowerOblivion\_bmad-output\gdd.md'
  epics: 'D:\Dev\Unity\TowerOblivion\_bmad-output\epics.md'
---

# Project Context for AI Agents

This file contains critical rules and patterns that AI agents must follow when implementing game code in this project. Focus on unobvious details that agents might otherwise miss.

---

## Technology Stack & Versions

**Current repository state:** planning artifacts exist, but the Unity project has not been scaffolded yet. No `ProjectSettings/ProjectVersion.txt`, `Packages/manifest.json`, C# scripts, asmdefs, Unity scenes, or Unity assets were found during discovery.

**Planned stack from architecture:**

- Engine: Unity 6.3 LTS
- Platform: PC / Steam
- Rendering: Unity 2D + URP 2D
- Default Font: `Assets/Art/Fonts/Alata-Regular SDF.asset`
- Runtime UI: uGUI
- Input: Unity Input System
- Tests: Unity Test Framework, EditMode and PlayMode
- Save: local versioned JSON snapshots with validation, backup, and migration hooks
- Steam: achievements behind platform interfaces only
- AI tooling: MCP Unity and Context7 are optional dev-only tools

**Agent rules:**

- Exact Unity patch version is not known yet because the Unity project has not been scaffolded. Once Unity is installed, `ProjectSettings/ProjectVersion.txt` becomes the source of truth.
- Use uGUI for runtime UI. Do not switch MVP runtime UI to UI Toolkit unless the architecture is explicitly changed.
- Use Unity Input System. Do not use the legacy Input Manager.
- Steam achievements are MVP scope, but only behind interfaces. Gameplay code must not call Steam APIs directly.
- MCP Unity and Context7 are optional dev-only tools. They must not become runtime, build, CI, or gameplay dependencies.

## Critical Implementation Rules

### Engine-Specific Rules

- Keep `TowerOblivion.Gameplay` Unity-free. It must not reference `UnityEngine`, MonoBehaviours, ScriptableObjects, scenes, prefabs, uGUI, PlayerPrefs, Addressables, Steam, Unity random/time APIs, or Unity serialization types.
- Forbid Unity types in Gameplay models: no `Vector2`, `Vector3`, `Color`, `AnimationCurve`, `Sprite`, `AudioClip`, `GameObject`, `SerializeField`, or Unity-specific serialization assumptions. Use primitive/domain value objects or stable IDs.
- Enforce dependency direction with asmdefs: `Core` references nothing, `Gameplay` references `Core` only, `Presentation/Input/Infrastructure` may reference `Gameplay` and `Core`, never the reverse.
- MonoBehaviours may orchestrate Unity lifecycle, bind serialized references, collect input, render state, play feedback, and call services. They must not contain game rules or persistent state authority.
- ScriptableObjects are authoring inputs only. They may be read only by infrastructure/editor/bootstrap conversion code. Runtime systems consume validated immutable DTO catalogs.
- Invalid authoring data must fail fast during editor/build validation or runtime bootstrap, before gameplay starts.
- Gameplay catalogs reference assets by stable IDs, not direct Unity asset references. Presentation/infrastructure resolves IDs to prefabs, sprites, audio, and VFX.
- `Core` must stay tiny: `Result`, IDs, `IRng`, `IClock`, `ILogger`, event abstractions, and similar primitive abstractions only. No domain rules, Unity adapters, save logic, or content loading.
- Use Unity lifecycle methods only in Presentation/Input/Infrastructure adapters. Gameplay code must be plain C# and testable without scene loading.
- No coroutines, Unity async operations, or frame-dependent waits in Gameplay. Timed behavior uses injected clocks/ticks or explicit simulation steps.
- Gameplay systems receive `IRng` and `IClock` through constructors or method parameters. No static random, no `UnityEngine.Random`, no `Time.time`, and no service-locator fallback.
- Save ownership is split: Gameplay defines save-state DTOs/snapshots; Infrastructure owns JSON file IO, versioning, migration, path selection, validation, and corruption fallback.
- Gameplay emits domain events/results. Presentation decides how to animate, show UI, play sounds, or trigger platform-facing feedback.
- Input System belongs at the edge. Gameplay receives intent commands, not `InputAction`, devices, bindings, or Unity input events.
- No `Debug.Log` from gameplay assemblies. Use a tiny logger abstraction from `Core`; logging must be optional/no-op safe.
- Commit `.meta` files. Ignore generated/cache folders only.
- Start with minimum asmdefs: `Core`, `Gameplay`, `Infrastructure`, `Presentation`, `Input`, `Editor`, `Tests`. Avoid over-splitting until coupling or compile pressure appears.
- Do not add Addressables, ECS, custom DI containers, or build-time catalog generation unless the MVP has a demonstrated need.

### Performance Rules

- Target: 60 FPS at 1920x1080 (16:9) on mid-tier PC hardware; minimum acceptable is 30 FPS.
- Frame-time target is ~16.7 ms at 60 FPS. Prioritize input responsiveness and readability over decorative effects.
- Combat board feedback must stay readable under worst-case effect stacking. Difficulty should increase decision pressure, not visual confusion.
- Avoid allocations in gameplay hot paths: combat resolution, card preview, board highlighting, input polling, reward generation, and room interaction scanning.
- Do not use LINQ, string concatenation, reflection, or repeated collection allocations in hot paths unless profiling proves it is harmless.
- Pool frequently spawned presentation objects: damage numbers, status icons, tile highlights, VFX tokens, card views if recreated often.
- Keep combat rules deterministic and cheap enough to simulate many encounters in EditMode tests.
- Room scenes should load one room context at a time for MVP. Do not introduce streaming/Addressables unless content scale proves the need.
- Large room illustrations must follow memory budgets and import presets once the Unity project exists.
- Use reusable feedback tokens instead of bespoke animation/VFX per card or enemy.
- Audio/visual spectacle must not mask gameplay feedback. Threat signals, damage/heal feedback, and board previews have priority.
- Any new visual effect must be tested against combat readability, not only against visual appeal.
- Profile worst-case screens: combat with stacked effects, room transition, inventory/memory/skills UI, save/load, and post-run summary.

### Code Organization Rules

- Use the hybrid Unity structure from the architecture. Do not invent new top-level script folders without updating the architecture.
- Required script areas: `Assets/Scripts/Core`, `Gameplay`, `Presentation`, `Infrastructure`, `Input`, `Editor`.
- Required test areas: `Assets/Tests/EditMode` and `Assets/Tests/PlayMode`.
- Major script folders should have a small `README.md` explaining purpose, allowed dependencies, forbidden contents, and example classes.
- Assembly definitions must enforce folder intent: `Core` depends on nothing, `Gameplay` depends on `Core`, `Presentation/Input/Infrastructure` depend inward, and `Editor` is never referenced by runtime assemblies.
- `Core` is not a utilities drawer. Allowed: primitive abstractions, IDs, result/error types, logger/event/RNG/clock interfaces. Forbidden: domain rules, Unity helpers, save logic, content loading.
- Ports/interfaces live near the caller that needs them; adapters live in `Infrastructure`. Example: gameplay-facing save port outside infrastructure, JSON implementation inside `Infrastructure/Persistence`.
- `Infrastructure` contains technical integrations only: persistence, platform services, logging implementation, content loading/conversion, serialization, external adapters. If a class decides game behavior, it does not belong there.
- Put combat rules in `Assets/Scripts/Gameplay/Combat`.
- Put exploration room rules in `Assets/Scripts/Gameplay/Exploration`.
- Put narrative flags and memory/clue rules in `Assets/Scripts/Gameplay/Narrative`.
- Put XP, meta-currency, skills, and unlock rules in `Assets/Scripts/Gameplay/Progression`.
- Put run generation in `Assets/Scripts/Gameplay/RunGeneration`.
- Avoid vague gameplay folders such as `Managers`, `Helpers`, `Common`, or generic `Systems`. Put shared code in the smallest owning domain first; promote only after repeated use.
- Avoid `Manager` naming unless it names a lifecycle boundary or external service orchestration. Prefer specific names like `CombatResolver`, `TurnScheduler`, `RunStateStore`, `EncounterGenerator`, `HudPresenter`.
- If orchestration logic needs a home, put it under `Gameplay/Application` or a clearly named orchestration folder, not in `Core`, presenters, or infrastructure.
- Put uGUI views, presenters, scene visual adapters, HUD, inventory, memory, reward, and combat board UI in `Assets/Scripts/Presentation`.
- Presenters are passive. They translate view input/output and bind state to uGUI. They do not decide gameplay outcomes, mutate saves directly, perform content rule lookup, or call Steam/persistence APIs except through application services.
- Put Unity Input System adapters in `Assets/Scripts/Input`. Command types live where they are consumed, usually `Gameplay` or orchestration, not in `Input`.
- Put JSON persistence, content conversion/loading, Steam/platform adapters, and logging implementation in `Assets/Scripts/Infrastructure`.
- Put content validation and import/editor utilities in `Assets/Scripts/Editor`.
- ScriptableObject assets live under `Assets/Data/Authoring`. They may contain authored data and editor-time validation only; no mutable run state, procedural decisions, combat resolution, progression logic, or player-specific state.
- Generated assets under `Assets/Data/Generated` are disposable/rebuildable outputs. Do not hand-edit generated content.
- Do not place gameplay rules inside prefabs, scenes, presenters, or ScriptableObject assets.
- Prefabs/scenes may reference views, adapters, installers, and serialized config references. Important runtime wiring should be in explicit composition roots, not hidden only in scenes.
- Tests mirror ownership: EditMode for pure gameplay, content validation, converters, save serialization; PlayMode for Unity integration, scene wiring, UI flows, input adapters, prefab behavior.
- Naming: C# files/classes use PascalCase; interfaces use `I` prefix; private fields use `_camelCase`; content IDs use lowercase dot/snake notation such as `souvenir.broken_laurel`.
- Localization: All displayed text must use `LocalizedString`. UI translations are stored in the `UI` table under `Assets/Localization/UI` (Default Locales: en, fr, es).
- Localization Naming Convention: Keys use `PascalCase` with dot notation for grouping, e.g., `Category.SubCategory.KeyName` (Example: `Hub.StartButton`, `Menu.Settings.Title`).
- Technical Wiring for Localization: When using `LocalizeStringEvent` in Unity:
    - Ensure `m_StringReference` points to the correct **String Table Collection** GUID (verify in Inspector).
    - The `OnUpdateString` callback MUST be serialized in **Dynamic mode** (`m_Mode: 0`).
    - Bind it to the `set_text` method of the `TextMeshProUGUI` component to ensure the translated string is passed correctly at runtime.
    - Avoid "Static String" mode (`m_Mode: 5`) for these callbacks as it will overwrite the label with an empty string or a hardcoded value.
- Use one public type per C# file unless there is a strong reason not to.
- Event records should be plain C# records, not `UnityEvent`, and should be past-tense facts or explicit requests such as `CombatWon` or `EnterCombatRequested`.

### Unity Naming Conventions (Mandatory for AI Agents)

To ensure consistency and maintainability across AI-generated content, all Unity objects must follow these naming rules:

#### 1. GameObjects & Hierarchy
- **Scene Objects**: Use `PascalCase` (e.g., `MainCamera`, `EnvironmentContainer`, `PlayerSpawnPoint`).
- **UI Components**: Use `PascalCase` with a functional suffix for primary interactive elements:
    - Buttons: `[Name]Button` (e.g., `StartRunButton`, `CloseWindowButton`).
    - Labels/Texts: `[Name]Label` or `[Name]Text` (e.g., `HealthLabel`, `DescriptionText`).
    - Panels/Windows: `[Name]Panel` (e.g., `InventoryPanel`, `RewardOverlayPanel`).
    - Input Fields: `[Name]Input` (e.g., `NameInput`).
- **Standardized Child Text**: For buttons and simple containers, name the child TextMeshPro object simply `Text` (not `Text (TMP)` or `Label`).

#### 2. Folders & Assets
- **Folders**: Use `PascalCase` (e.g., `Art/Sprites/Environment`, `Scripts/Core`).
- **Prefabs**: Use `PascalCase` matching the root GameObject name.
- **Materials/Shaders**: Use `PascalCase` describing the intent (e.g., `UnlitTransparentGlow`).
- **Textures/Sprites**: Use `snake_case` or `PascalCase` based on existing patterns, but prioritize descriptive names (e.g., `bg_hub_main`, `Icon_Sword_Gold`).

#### 3. Localization Keys
- **Dot Notation**: Use `PascalCase` with dots for grouping: `Category.SubCategory.KeyName`.
- **Consistency**: The key should describe the *purpose*, not the *content* (e.g., `Menu.Button.Quit` instead of `Menu.Button.ExitGame`).

#### 4. Serialization
- **Private Fields**: Use `_camelCase` with the `[SerializeField]` attribute (e.g., `[SerializeField] private Button _confirmButton;`).
- **Public Properties**: Use `PascalCase`.

### Testing Rules

- Gameplay tests must run without Unity scenes, prefabs, MonoBehaviours, Steam, PlayerPrefs, Addressables, `UnityEngine.Random`, or `Time.time`.
- Prefer EditMode tests for pure logic: combat rules, run generation, narrative flags, progression, save snapshot conversion, content validation, and deterministic RNG behavior.
- Use PlayMode tests only for Unity integration: scene flow, prefab wiring, uGUI presenters, input adapters, visual feedback smoke tests, and boot/hub/combat integration.
- Combat determinism is mandatory: same content version + same initial state + same command list + same RNG seed must produce the same result and final state hash.
- Run generation determinism is mandatory: same content version + same seed + same generation config must produce the same floor/room/encounter/reward structure.
- Save tests must cover roundtrip, schema version, migration, corrupt primary save, backup fallback, invalid content version, and interrupted write behavior.
- Content validation tests must catch duplicate IDs, missing references, invalid numeric ranges, invalid board/effect definitions, missing localization keys, and missing achievement mappings.
- Narrative tests must prove flags, clue/memory unlocks, and choice consequences persist through save/load and death/rebirth.
- Platform tests must use `NullAchievementService` or a fake achievement service. Tests must not require Steam runtime.
- Input tests should validate intent mapping, not gameplay rules. Gameplay rule tests should call services/commands directly.
- Every gameplay test should inject `IRng`, `IClock` when needed, test content catalogs, initial state/snapshot, and fake event sinks.
- Do not rely on scene object names, frame timing, animation completion, or real file paths in pure gameplay tests.
- If a gameplay system cannot be tested without Unity scene setup, its boundaries are wrong.

### Platform & Build Rules

- Primary platform is PC / Steam only for MVP. Do not add mobile, console, web, online services, or cloud-save assumptions unless scope changes.
- Steam achievements are required for MVP, but gameplay must emit achievement-relevant facts through domain/application events. Steam-specific unlock code belongs behind `IAchievementService` or `IPlatformServices`.
- Provide a local/null platform implementation so the game runs in editor and local builds without Steam.
- If Steam is unavailable, achievement intent should be stored locally and replayed when Steam becomes available.
- Save system is local-first. Do not implement Steam Cloud in MVP unless explicitly requested later.
- Builds must preserve local save compatibility. Any save schema change requires migration or explicit invalidation handling.
- Mouse + keyboard and gamepad are both MVP targets. Input actions must be device-agnostic at gameplay level.
- Do not let platform code leak into gameplay assemblies. No Steam IDs, Steamworks types, platform paths, or platform conditionals in Gameplay.
- Release-candidate builds must fail or be blocked if critical content validation fails.
- Release-candidate builds must fail or be blocked if shipping assets lack provenance/commercial-use status.
- Debug tools are editor/development-build only unless explicitly compiled into a special build. They must not be reachable through normal player UI in release builds.
- Exact Unity version must be locked by `ProjectSettings/ProjectVersion.txt` after project creation.

### Critical Don't-Miss Rules

- Do not implement the full combat system in Epic 2. Epic 2 may include deterministic board primitives, validation, previews, fixtures, and tests, but not complete enemy AI, progression rewards, balancing, or full combat UX unless re-scoped.
- Do not build a generic reusable engine platform. Reusability means clean module boundaries, data-driven content, and Unity-free gameplay where practical.
- Do not add in-run shops, Steam Cloud, mobile support, online services, mod support, ECS, Addressables, custom DI containers, FMOD/Wwise, dialogue middleware, or a second UI paradigm unless explicitly re-scoped.
- Do not add meta-progression economy depth before the room -> combat -> reward -> save -> run-end loop is executable and fun.
- Do not add content volume before content schemas, validation rules, and balancing fields are stable.
- If a feature requires lots of content before it becomes fun, it is not MVP-safe.
- Keep MVP run length and content count constrained. Replayability should come from variation and consequence, not volume.
- Use one combat ruleset for MVP. Enemy variety should come from stats, intents, Souvenir interactions, and board layouts, not bespoke encounter-specific rule exceptions.
- Every combat result must be reproducible from saved combat state, player action/command list, content version, and deterministic RNG seed. No `Time`, frame order, Unity physics, animation events, collection iteration ambiguity, or floating-point randomness may affect outcomes.
- Combat preview logic must be pure/read-only. Hover/preview must never mutate board state, RNG, buffs, counters, cooldowns, or tracking flags.
- Reward text/effects must stay readable. Every reward should be understandable in one short tooltip and resolve deterministically. Avoid nested triggers, ambiguous timing, hidden counters, or "when X unless Y before Z" effects unless re-scoped.
- Souvenirs may unlock optional advantages, routes, flavor, alternate rewards, or tactical benefits, but core run completion must not depend on owning a specific Souvenir unless explicitly scoped.
- Do not store derived combat stats as authoritative save data. Saves may cache derived values only for display/performance if they are recomputed and validated on load.
- Meta upgrades should apply at run start or explicit transition points, not retroactively inside active combat/rooms unless explicitly designed.
- Do not mutate ScriptableObject assets at runtime.
- Do not let narrative choices become deep branching trees, procedural dependency chains, soft-lock-prone prerequisite graphs, or hidden systemic modifiers.
- Do not let combat difficulty increase by adding too many simultaneous board-rule exceptions. Increase pressure through floor index, numbers, enemy rulesets, elite modifiers, and boss thresholds.
- Do not sacrifice board readability for VFX, animation, or "juice." Every placement effect must be previewable and understandable.
- Do not use raw string narrative flags, content IDs, room IDs, enemy IDs, Souvenir IDs, or achievement IDs in gameplay code. Use stable typed IDs or registries so renaming display text or moving files does not break saves.
- Events are for notification and integration boundaries only. Core gameplay decisions must remain traceable through direct application/domain services.
- Achievement logic must observe domain/progression events, not UI events, button clicks, scene loads, panels opening, or animation callbacks.
- Do not place persistent state authority in UI, scenes, prefabs, presenters, ScriptableObjects, or MonoBehaviours. "Temporary" MonoBehaviour authority is a hard violation.
- Do not bypass save validation/recovery because "it is only MVP." Once saves exist, schema version and migration behavior are in scope.
- "Delete old saves" is acceptable only before public playtests, not after Steam-facing builds.
- Content data must fail closed. Missing/invalid content, unknown IDs, duplicate IDs, invalid board coordinates, invalid rewards, or broken prerequisites must produce clear validation errors and safe fallback behavior.
- Do not add broad loot taxonomy. MVP itemization stays compact: Souvenirs, temporary divine favors/modifiers, and very few consumables/quest objects.
- Test the full loop before tuning values. Avoid balancing Souvenirs, enemies, room text volume, or rewards until room -> combat -> reward -> save -> run-end works and has smoke coverage.
- Do not use AI/purchased/generated assets in a shipping build without documented commercial-use rights and provenance.
- Do not prompt asset generation using living artists, named franchises, copyrighted characters, or "in the style of X."
- Do not make VO mandatory for comprehension. All important information must exist in text or clear gameplay feedback.
- Achievements should reflect existing MVP milestones. They must not introduce extra systems, hidden tracking complexity, or content requirements.
- Avoid cinematic production commitments: cutscenes, complex camera staging, animated dialogue portraits, bespoke room transitions, and VO-dependent pacing.

---

## Usage Guidelines

**For AI Agents:**

- Read this file before implementing any game code.
- Follow all rules exactly as documented.
- When in doubt, prefer the more restrictive option.
- If a requested implementation conflicts with this file, surface the conflict before coding.
- Update this file only when project architecture, scope, or implementation patterns intentionally change.

## Unity MCP Usage Policy (Mandatory)

When Unity MCP is available, agents MUST use it to take full ownership of the Unity-side implementation. Manual instructions for the user are a last-resort fallback.

Mandatory uses for Unity MCP:
- **Full Scene Setup**: Create GameObjects, hierarchy, and lighting for new features.
- **Component Wiring**: Assign all serialized references, events, and Unity Event listeners.
- **UI Polishing**: Use project-specific UI assets (e.g., Layer Lab) to create attractive, high-quality interfaces rather than bare placeholders.
- **Runtime Verification**: Enter Play Mode to debug and verify logic, state persistence, and UI responsiveness.
- **Asset Configuration**: Set up materials, fonts, and localization assets.

Execution rules:
- Prefer local file analysis first.
- If Unity state is required, call Unity MCP and report exact checks performed.
- Never claim Unity validation passed without evidence from Unity MCP or Unity Test Runner output.
- Always separate `Verified` (observed via MCP/tests) from `Assumed` (not directly verified).

## Context7 MCP Usage Policy (Mandatory When Relevant)

Context7 is available in Codex as the `context7` MCP server. Use it whenever current or version-specific external documentation would improve implementation accuracy.

Mandatory uses for Context7:
- Library, framework, SDK, API, Unity package, or tool usage questions.
- Setup, configuration, migration, or upgrade steps.
- Code generation that depends on third-party APIs or current package behavior.
- Debugging errors where upstream docs, package behavior, examples, or configuration options may have changed.

Execution rules:
- Resolve the package or library name to a Context7 library ID first.
- Fetch relevant documentation before giving implementation guidance or editing code that depends on that external API.
- Prefer Context7 documentation over memory for modern dependencies.
- If Context7 is unavailable or does not contain the needed library, state that briefly and use the next best source.
- Do not use Context7 for purely local codebase questions, general C# syntax, or project-specific behavior that can be answered from repository files.

## Nano Banana MCP Asset Generation Policy (Use When Assets Are Needed)

Nano Banana image generation is available in Codex through the `mcp-image` MCP server. Use it to create prototype or production-candidate visual assets when generated imagery would unblock implementation, UX polish, concept validation, or placeholder replacement.

Mandatory uses for `mcp-image`:
- Generate temporary or prototype UI art, icons, illustrations, room art, combat concepts, or mood references when a feature needs visual assets to be usable.
- Create consistent visual variants for game-specific assets after the desired style, aspect ratio, and in-game purpose are known.
- Edit or refine existing generated/reference images when an asset needs a targeted visual change.
- Produce assets directly into `Assets/Art/Generated` unless a more specific generated-art folder is intentionally created.

Execution rules:
- Use `mcp-image` for image generation/editing, then use Unity MCP for Unity import, assignment, scene wiring, and runtime verification when Unity state is involved.
- Prefer explicit prompts that include game context, asset purpose, style constraints, aspect ratio, background/transparency needs, and whether text should be avoided.
- For UI icons and gameplay-readable assets, request clean silhouettes, high contrast, no embedded text, and simple shapes that remain readable at small sizes.
- Store generated outputs only under generated/prototype asset folders, not final curated art folders, until human review accepts them.
- Record provenance for any generated asset that may ship: source tool/model if available, prompt summary, generation date, and intended usage/license review status.
- Do not use AI-generated or purchased/generated assets in a shipping build without documented commercial-use rights and provenance.
- Do not generate assets using living artists, named franchises, copyrighted characters, or "in the style of X."
- If `mcp-image` is unavailable, state that clearly and proceed with placeholders or request human-provided art instead of blocking implementation.

**For Humans:**

- Keep this file focused on implementation-critical rules for agents.
- Update it when Unity version, package choices, architecture boundaries, or MVP scope changes.
- Remove or simplify rules only when they become obsolete or are enforced elsewhere.
- Treat `_bmad-output/game-architecture.md` as the detailed source and this file as the compact implementation guide.

Last Updated: 2026-04-28
