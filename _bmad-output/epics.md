# Tower Oblivion - Development Epics

## Epic Overview

| # | Epic Name | Deliverable |
|---|---|---|
| 1 | Minimal Reusable Game Foundation | Thin reusable framework primitives for the first playable slice |
| 2 | Playable Identity Vertical Slice | One end-to-end identity proof with a thin combat stub |
| 3 | Souvenir Board Combat MVP | Complete MVP 4x4 Souvenir combat system |
| 4 | Exploration and Narrative Rooms | Functional point-and-click narrative room framework |
| 5 | Progression, Economy, and Rebirth Hub | Persistent progression and hub upgrade loop |
| 6 | Chapter Run Content and Guardian | MVP chapter structure, pacing, and guardian goal |
| 7 | Art, Audio, UX, and Production Pipeline | Readable presentation pipeline and asset workflow |
| 8 | Steam MVP Polish and Release Readiness | Stable Steam-ready MVP build |

---

## Epic 1: Minimal Reusable Game Foundation

### Goal

Create only the reusable primitives required to support Tower Oblivion's first playable slice, while keeping extension seams for future point-and-click narrative games with turn-based board combat.

### Scope

**Includes:**
- State boundaries for `RunState`, `CombatState`, `MetaState`, and `WorldSeedState`
- Save/load foundation
- Input and UI shell
- Data-driven content definitions
- Minimal scene/room module seam
- Minimal narrative flag/event seam
- Minimal combat module seam
- Content registry structure

**Excludes:**
- Full gameplay systems
- Full combat rules
- Chapter content
- Generic multi-game editor
- Broad engine abstraction
- Mod support

### Dependencies

None.

### Deliverable

A lean Unity framework substrate capable of loading placeholder content, storing state, saving/loading progression, and connecting room, narrative, combat, and progression modules without hard Tower-specific dependencies.

### Stories

- As a developer, I can define core state domains so that run, combat, meta, and seed data stay separated.
- As a developer, I can create and load a local save so that progression can persist.
- As a developer, I can define placeholder content in data so that systems are not hardcoded.
- As a developer, I can load a placeholder room from a reusable room definition.
- As a developer, I can define a placeholder narrative flag so that consequences can persist.
- As a developer, I can define a placeholder combat encounter payload so that exploration can enter combat.
- As a developer, I can validate basic save data so that corrupt data is detected early.
- As a developer, I can reuse the core primitives for future similar games.

---

## Epic 2: Playable Identity Vertical Slice

### Goal

Validate the unique player-facing identity early: mythological room discovery, Souvenir reward, thin 4x4 board use, consequence, death/rebirth, and retained progression.

### Scope

**Includes:**
- Placeholder rebirth hub
- One mythological illustrated room
- One meaningful discovery interaction
- One Souvenir reward
- Thin 4x4 combat stub
- One consequence flag
- Death/rebirth or run-resolution flow
- Saved progression display
- Essential room and combat readability

**Excludes:**
- Full combat system
- Full enemy AI
- Full progression tree
- Chapter content volume
- Final art/audio polish

### Dependencies

Epic 1.

### Deliverable

A short playable validation slice where the player starts in the hub, enters a room, discovers something, receives or selects a Souvenir, uses it in a thin 4x4 combat stub, sees a consequence, returns to the hub, and starts again with visible retained progression.

### Stories

- As a player, I can start from the rebirth hub so that the loop has a clear origin.
- As a player, I can enter one mythological room so that the tower fantasy is visible.
- As a player, I can inspect one meaningful discovery so that investigation matters.
- As a player, I can receive or select one Souvenir so that discovery connects to build identity.
- As a player, I can place one Souvenir in a thin 4x4 combat stub so that the board link is felt.
- As a player, I can see basic combat feedback so that the result is understandable.
- As a player, I can trigger one persistent consequence so that choices carry forward.
- As a player, I can die or resolve the run and return to the hub so that rebirth is clear.
- As a player, I can see retained progression so that failure feels meaningful.

---

## Epic 3: Souvenir Board Combat MVP

### Goal

Implement the complete MVP 4x4 Souvenir board combat system as a reusable, deterministic, readable combat module.

### Scope

**Includes:**
- 4x4 board state
- 8-card combat hand per side
- Card/Souvenir placement
- Trigger effects
- Adjacency effects
- Hero life and board-life resolution
- Deterministic win/loss rules
- Enemy AI heuristic
- Placement preview/highlighting
- Reusable combat feedback tokens
- Bonus-based Souvenir requirement support

**Excludes:**
- Large card pool
- Complex status stacks
- Multiplayer
- Advanced bespoke animations
- Room/narrative content

### Dependencies

Epics 1-2.

### Deliverable

A complete deterministic board-combat encounter that supports MVP Souvenir effects, enemy turns, clear previews, and fair win/loss resolution.

### Stories

- As a player, I can place Souvenirs on a 4x4 board so that spatial decisions matter.
- As a player, I can preview placement effects so that outcomes are readable.
- As a player, I can trigger adjacency effects so that sequencing and placement matter.
- As a player, I can track hero life and board life so that victory conditions are clear.
- As a player, I can win or lose by deterministic rules so that combat feels fair.
- As an enemy, AI can place cards using simple heuristics so that battles have opposition.
- As a designer, I can define Souvenir effects in data so that content expands safely.
- As a player, I can understand damage, shield, status, and divine triggers through reusable feedback.
- As a player, I can unlock bonus Souvenir effects through upgrade tiers so that progression matters.
- As a developer, I can run deterministic combat tests so that bugs are reproducible.

---

## Epic 4: Exploration and Narrative Rooms

### Goal

Build the point-and-click room framework and bounded narrative consequence model without turning the epic into full chapter content production.

### Scope

**Includes:**
- Fixed-room scene presentation
- Hotspots
- Object inspection
- Clues and memory unlocks
- Locked/secret room framework
- Skill-gated interactions
- Dialogue/event variants
- Persistent narrative flags
- Room readability rules

**Excludes:**
- Full chapter room count
- Chapter guardian
- Deep branching story trees
- Complex god relationship matrix
- Final art/audio polish

### Dependencies

Epics 1-2.

### Deliverable

Reusable room and narrative systems with a small set of proof rooms demonstrating clues, flags, gates, and replay value.

### Stories

- As a player, I can inspect room hotspots so that I discover clues.
- As a player, I can unlock a memory from exploration so that narrative progresses.
- As a player, I can encounter a locked room so that meta skills create anticipation.
- As a player, I can use a skill gate so that old rooms become valuable later.
- As a player, I can make a bounded narrative choice so that future runs change.
- As a designer, I can define room events in data so that content is reusable.
- As a player, I can recognize interactable objects so that rooms are readable.
- As a player, I can discover a secret room proof so that replay is rewarded.
- As a developer, I can track narrative flags without broad world simulation.

---

## Epic 5: Progression, Economy, and Rebirth Hub

### Goal

Implement persistent progression, hub upgrades, death conversion, and post-run clarity.

### Scope

**Includes:**
- XP progression
- Life/Strength/Psyche/Mana upgrade tiers
- Meta-currency
- Exploration/narrative skills
- Souvenir unlock pool
- Post-run summary
- Death conversion rules
- Hub upgrade UI
- Kept/lost/converted feedback

**Excludes:**
- In-run shop
- Complex crafting
- Large skill tree
- Full ascension arc
- Steam save hardening

### Dependencies

Epics 1-2.

### Deliverable

A persistent progression loop where failed runs produce visible combat and exploration growth and give the player a clear reason to retry.

### Stories

- As a player, I can earn XP so that combat options improve.
- As a player, I can upgrade Life, Strength, Psyche, or Mana so that builds differ.
- As a player, I can unlock Souvenir bonuses through upgrade tiers so that progression affects combat.
- As a player, I can earn meta-currency so that exploration options expand.
- As a player, I can unlock skills like lockpicking or trap detection so that new rooms open.
- As a player, I can see Combat Growth, Tower Knowledge, and Next Opportunity after a run.
- As a player, I can understand what was kept, lost, and converted after death.
- As a developer, I can cap progression values so that balance remains controlled.
- As a player, I receive at least one visible progression opportunity after early failure.

---

## Epic 6: Chapter Run Content and Guardian

### Goal

Create the MVP chapter content that proves 25-35 minute run pacing, build variety, narrative anchors, and the chapter final guardian.

### Scope

**Includes:**
- Fixed floor sequence
- Room pools
- Encounter pools
- Reward pools
- Elite modifiers
- Anchor narrative rooms
- Guided onboarding micro-loop
- Chapter final guardian
- Zeus foreshadowing
- Run pacing tuning

**Excludes:**
- Full game chapter count
- Zeus confrontation
- Large enemy roster
- Many biomes
- New core systems

### Dependencies

Epics 3-5.

### Deliverable

A playable MVP chapter with a beginning, progression arc, semi-procedural variation, and final guardian goal.

### Stories

- As a player, I can climb a fixed floor sequence so that the run has structure.
- As a player, I encounter randomized rooms so that runs vary.
- As a player, I can reach elite rooms so that risk/reward escalates.
- As a player, I can encounter anchor narrative rooms so that story pacing remains coherent.
- As a player, I can complete the onboarding micro-loop so that the game promise is clear.
- As a player, I can fight a chapter guardian so that the MVP has a strong goal.
- As a player, I can see Zeus foreshadowed so that the full-release arc is implied.
- As a designer, I can tune room pools and encounter weights so that pacing holds.
- As a player, I can complete a run in the 25-35 minute target band.
- As a developer, I can reproduce runs through internal seed support.

---

## Epic 7: Art, Audio, UX, and Production Pipeline

### Goal

Create the MVP production pipeline and full presentation pass after core readability has already been proven in earlier epics.

### Scope

**Includes:**
- Room asset contract
- Asset provenance register
- Combat board visual language
- Reusable feedback kit
- UI readability pass
- SFX categories
- Music integration
- Partial VO pipeline
- Accessibility presentation options
- Content authoring workflow

**Excludes:**
- First implementation of gameplay readability
- Full final asset set
- Full localization
- Extensive animation
- Cinematic cutscenes

### Dependencies

Epics 2-6.

### Deliverable

A visually readable and commercially traceable presentation pipeline for MVP content.

### Stories

- As a developer, I can import rooms using a consistent asset contract so that production remains stable.
- As a player, I can distinguish interactables from background art so that exploration is readable.
- As a player, I can understand board states through clear combat feedback so that combat feels fair.
- As a developer, I can track asset provenance so that release risk is controlled.
- As a player, I can hear distinct SFX for core actions so that feedback is clear.
- As a player, I can rely on text even when VO is absent so that comprehension is not voice-dependent.
- As a developer, I can use reusable feedback tokens so that combat polish scales.
- As a player, I can adjust readability/audio options so that the game remains accessible.

---

## Epic 8: Steam MVP Polish and Release Readiness

### Goal

Prepare the MVP for Steam-facing testing and release readiness.

### Scope

**Includes:**
- Steam achievements
- Settings menu
- Save validation/recovery
- Performance profiling
- Crash/error handling
- Build packaging
- QA checklist
- Controller support hardening
- Demo/release readiness support

**Excludes:**
- New gameplay scope
- Full launch marketing campaign
- Mobile release
- Mod support
- Online services

### Dependencies

Epics 1-7.

### Deliverable

A stable Steam-ready MVP build suitable for demo/testing and wishlist conversion validation.

### Stories

- As a player, I can unlock Steam achievements so that milestones are recognized.
- As a player, I can configure controls, audio, text size, and motion options so that the game is comfortable.
- As a player, my save data is validated so that progression is protected.
- As a developer, I can recover from save version changes so that patches are safer.
- As a developer, I can profile performance so that the 60 FPS target is protected.
- As a tester, I can run a QA checklist so that release risks are visible.
- As a developer, I can create a Steam build package so that external testing is possible.
- As a designer, I can validate first-session completion, return intent, and death-worth-it sentiment.

---

## Delivery Principles

- Each epic must produce either playable proof or reusable code directly needed by the MVP.
- Reusability is achieved through disciplined boundaries and data-driven definitions, not broad engine abstraction.
- Essential readability is required in early gameplay epics, not postponed to the polish epic.
- Content expansion waits until the core loop, combat, exploration, and progression are validated.
