---
stepsCompleted: [1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14]
inputDocuments:
  - "D:\\Dev\\Unity\\TowerOblivion\\_bmad-output\\game-brief.md"
documentCounts:
  briefs: 1
  research: 0
  brainstorming: 0
  projectDocs: 0
workflowType: 'gdd'
lastStep: 14
project_name: 'IA'
user_name: 'Franck'
date: '2026-04-24T20:31:29+02:00'
game_type: 'roguelike'
game_name: 'Tower Oblivion'
---

# Tower Oblivion - Game Design Document

**Author:** Franck  
**Game Type:** Roguelike  
**Target Platform(s):** PC (Steam)

---

## Executive Summary

### Game Name

Tower Oblivion

### Core Concept

Tower Oblivion is a tactical heroic-fantasy roguelike built around a fixed floor sequence with randomized encounters, rewards, and modifiers. Each run follows: floor entry -> explore rooms -> find resources and Souvenirs (point-and-click) using skills -> solve puzzles -> fight guardians/monsters -> reward choice -> resource management -> floor ascent.

Combat is a 4x4 Souvenir board system where each side deploys from an 8-card combat hand from a deck per battle. Placement and timing on the 4x4 board are the primary skill tests, with immediate placement effects driving tactical outcomes and build expression.

The experience is split into two goals:
- **Run goal:** reach higher floors and survive each ascent.
- **Meta goal:** recover memories, expand Souvenir options, and progress toward demigod ascension.

Early runs teach core board logic before deeper synergies are introduced. For MVP scope, the chapter ends with a chapter boss and foreshadows Zeus for later chapters.

**Technical baseline for this GDD:** PC-first target at 60 FPS, 1080p, mid-tier hardware, mouse + keyboard + gamepad support. Difficulty scales by floor index, elite modifiers, and boss phase thresholds.

### Game Type

**Type:** Roguelike  
**Framework:** This GDD uses the `roguelike` template with type-specific sections for run structure, procedural generation, permadeath/progression, item-upgrade systems, character setup, and difficulty modifiers.

## Target Platform(s)

### Primary Platform

PC (Steam)

### Platform Considerations

- MVP is PC-first with no secondary platform commitment in current scope.
- Steam distribution and discoverability are core to launch strategy.
- Platform feature included for MVP: Steam achievements.
- Performance baseline remains aligned with brief constraints (60 FPS target at 1080p on mid-tier hardware).

### Control Scheme

- Mouse + keyboard
- Gamepad (parity support in MVP)

---

## Target Audience

### Demographics

- Age: 16+
- Player profile: midcore / casual-core

### Gaming Experience

Midcore/casual-core players who want accessible tactical depth without high mechanical execution barriers.

### Genre Familiarity

Mixed familiarity:
- players comfortable with roguelite progression loops
- players attracted by mystery narrative and tactical board decisions even if less experienced with card-combat hybrids

### Session Length

~30 minutes per run, with first meaningful build choice within <=10 minutes.

### Player Motivations

- Mystery and narrative discovery across repeated runs
- Build optimization through Souvenir/deck progression
- Meaningful long-term advancement through memory retention and consequence systems

## Goals and Context

### Project Goals

1. **Creative goal**  
Deliver a distinctive mythological roguelite/puzzle experience with a strong identity: a condemned warrior climbing a divine tower, rebuilding lost glory through Souvenirs, and gradually becoming a demi-god.

2. **Technical goal**  
Build a small, polished, stable MVP around the 4x4 Souvenir combat system, with short runs, high readability, reliable save/meta-progression integrity, and performance stability across repeated sessions.

3. **Personal/production goal**  
Finish a realistic solo part-time scope without uncontrolled feature creep, and reach a meaningful end-to-end playable milestone that proves the concept.

4. **Narrative systems goal**  
Introduce meaningful player choices whose consequences persist and influence future events, encounters, and narrative context across the adventure.

### Background and Rationale

Tower Oblivion is being developed now because AI-assisted production can accelerate visual asset creation and make a complete Unity MVP feasible under solo part-time constraints. The game also aims to fill a gap for players who want a coherent blend of RPG progression, adventure-style exploration, tactical decision-making, and roguelite replayability in one structure.

---

## Unique Selling Points (USPs)

1. **4x4 Souvenir capture/flip combat**  
Spatial puzzle decisions combined with roguelite run tension in a compact, readable battle format.

2. **Memory-driven narrative progression**  
Prometheus-retained memory makes failure meaningful and transforms repeated attempts into identity progression.

3. **Semi-procedural runs with fixed-arc pacing**  
Randomized variation inside a controlled narrative spine designed for coherent ~30-minute sessions.

4. **Persistent narrative choices and consequences**  
Player choices create lasting effects that alter future encounters, available options, and story context in later runs.

5. **Hybrid interaction model**  
Point-and-click exploration integrated with tactical board combat, framed by mythological ascension from condemned mortal to demi-god.

### Competitive Positioning

Tower Oblivion positions itself as a tactical narrative roguelite hybrid that avoids pure action-centric or pure text-heavy extremes. It differentiates through the combination of board-based tactical combat, persistent memory systems, and consequence-driven narrative choices that carry forward.

## Core Gameplay

### Core Player Promise (MVP)

In ~30 minutes, each run delivers tense survival plus at least one permanent breakthrough.

### Game Pillars

1. **Die, Remember, Return Stronger**  
Failure resets run gear and temporary resources, but permanent skills, discovered clues, and persistent progression are retained.

2. **Choices Shape Future Runs**  
Player decisions create bounded persistent effects (MVP: unlock flags, encounter/reward weight shifts, lore node reveals) that alter later runs.

3. **Investigate Through Replay**  
The tower is understood across attempts; each run adds evidence and interpretation through clues, events, and memory threads.

4. **Tense Room-by-Room Survival**  
Progression is deliberate and risky, with readable threat signals and meaningful tactical tradeoffs.

**Pillar Prioritization:**  
When pillars conflict, prioritize in this order:  
1) Die, Remember, Return Stronger -> 2) Choices Shape Future Runs -> 3) Investigate Through Replay -> 4) Tense Room-by-Room Survival

### Core Gameplay Loop

Each run starts in the rebirth hub, where the player prepares and re-enters the tower.  
Inside the tower, progression follows a fixed floor sequence with randomized encounters, rewards, and modifiers.

**Loop Diagram:**  
Rebirth Hub Setup -> Floor Entry -> Room Exploration (point-and-click, clues, interactions) -> Puzzle/Skill Check (light MVP scope) -> 4x4 Souvenir Combat -> Reward Choice -> Build/Resource Adjustment -> Floor Ascent -> (Death -> Rebirth Hub with permanent gains) -> Next Run

**Loop Timing:**  
- MVP run target: ~30 minutes (P50 target band: 25-35 minutes)  
- First meaningful build target: <=10 minutes (P75)  
- Encounter cycle target: ~2-5 minutes

**Onboarding Reassurance (MVP):**  
First 8-10 minutes use a guided micro-loop (one clue, one combat, one upgrade, one death/rebirth) to teach board logic and failure value before deeper synergies.

**Loop Variation:**  
- Randomized encounters, rewards, and floor modifiers inside fixed chapter pacing  
- Changing Souvenir hand/deck states and divine favor options  
- Bounded persistent consequences (no broad world simulation in MVP)

### Win/Loss Conditions

#### Victory Conditions

- **Battle victory:** enemy hero life reaches 0, or board resolves to victory by deterministic end-of-battle rule set:
1) higher remaining hero life wins  
2) if tied, higher total surviving card life on board wins  
3) if still tied, hero wins
- **Battle rewards:** XP, quest objects, and/or Souvenirs.
- **Run victory (MVP):** defeat the chapter final guardian (Zeus reserved for full-release roadmap).
- **Meta progression success (MVP):** achieve at least one concrete permanent milestone unlock per successful progression cycle.

#### Failure Conditions

- **Battle failure:** hero life reaches 0, or end-of-battle deterministic comparison favors the enemy.
- **Run failure:** hero dies during ascent and run ends immediately.

#### Failure Recovery

On death, the hero returns to the rebirth hub.

**Retention Matrix (MVP):**
- **Kept on death:** permanent skills, unlocked Souvenirs, memory/clue discoveries, unlocked narrative flags, chapter progression milestones
- **Lost on death:** run-limited gear, temporary resources, in-run consumables, current floor progress
- **Converted on death:** selected run earnings converted into meta currency/progression according to chapter rules

## Game Mechanics

### Primary Mechanics

1. **Room Exploration and Investigation (Point-and-Click)**
- **Player verb:** explore, inspect, collect clues, interact with room elements
- **Usage frequency:** constant between combats
- **Skill tested:** observation, risk evaluation, route planning
- **Feel target:** deliberate, readable, tense
- **Progression:** unlockable utility skills (lockpicking, trap detection, camouflage, keen hearing) expand reachable content
- **Pillar support:** Investigate Through Replay, Tense Room-by-Room Survival, Choices Shape Future Runs

2. **Souvenir Board Combat (4x4 Tactical Placement)**
- **Core combat fantasy (MVP):** compact deterministic board duel focused on adjacency effects, life preservation, and sequencing
- **Player verb:** place cards, resolve trigger effects, control board state
- **Usage frequency:** frequent and central
- **Skill tested:** spatial reasoning, sequencing, tactical tradeoffs
- **Feel target:** clear, compact, high-stakes, deterministic resolution
- **Progression:** larger Souvenir pool, stronger synergies, selective god-favor modifiers
- **Pillar support:** Die/Remember/Return Stronger, Tense Room-by-Room Survival, Choices Shape Future Runs

3. **Run Build Shaping (Rewards and Upgrades)**
- **Player verb:** choose rewards, optimize short-term survival vs long-term value
- **Usage frequency:** recurring after encounters
- **Skill tested:** build planning, risk/reward prioritization
- **Feel target:** meaningful and fast decisions (first real build impact <=10 minutes)
- **Progression:** broader option set via meta unlocks; higher-tier choices at higher floors
- **Pillar support:** Die/Remember/Return Stronger, Choices Shape Future Runs

4. **Narrative Choice and Consequence (Bounded MVP Scope)**
- **Player verb:** choose dialogue/actions that set persistent flags
- **Usage frequency:** situational at defined events
- **Skill tested:** inference, strategic commitment
- **Feel target:** weighty but understandable
- **MVP consequence scope:**  
  1) unlock/block small dialogue or event variants  
  2) slight reward-category bias shifts  
  3) one to two medium-term payoffs
- **Out of MVP:** deep branching trees, dense hidden systemic modifiers, narrative-exclusive board-rule layers, complex god-relationship matrices
- **Pillar support:** Investigate Through Replay, Choices Shape Future Runs

5. **Death-Rebirth Meta Progression**
- **Player verb:** return to hub, spend permanent gains, reconfigure approach
- **Usage frequency:** every failed run / loop transition
- **Skill tested:** long-arc planning and adaptation
- **Feel target:** loss with forward momentum
- **Progression:** permanent skill tree, Souvenir unlocks, memory thread advancement
- **Pillar support:** Die/Remember/Return Stronger, Investigate Through Replay

### Mechanic Interactions

- Exploration yields clues/resources that influence reward choices and narrative flags.
- Narrative choices affect flavor and run structure first, with only bounded systemic impact in MVP.
- Combat outcomes grant XP/items/Souvenirs, feeding both immediate run build and long-term meta growth.
- Death resolves run state into retained meta gains and unlocks, then rebirth hub decisions shape the next run.
- Utility skills from meta progression unlock new exploration options, creating replay-based discovery loops.

### Mechanic Progression

- **Early runs:** guided onboarding, simple Souvenir effects, low-complexity room interactions.
- **Mid progression:** expanded Souvenir pool, stronger adjacency/sequence combos, clearer risk/reward decisions.
- **Later MVP chapter:** higher pressure encounters, elite modifiers, chapter guardian test of build + board mastery.
- **Scope guard for MVP:** one persistent progression track is primary; other systems remain shallow/supportive.

---

## Controls and Input

### Control Scheme (PC/Steam)

**Mouse + Keyboard**
- Cursor movement / target selection: Mouse move
- Primary interact / confirm placement: Left Click
- Secondary info / inspect tooltip: Right Click
- Navigate UI / quick confirm-cancel: Enter / Esc
- Character panel (stats/skills): `C`
- Inventory / Souvenirs: `I`
- Memory/Codex: `J`
- Map/Floor overview: `M`
- Pause/System: `Esc`

**Gamepad**
- Cursor/selection focus: Left Stick
- Confirm interact/place: `A` / Cross
- Secondary inspect/details: `X` / Square
- Cancel/back: `B` / Circle
- Tab cycle (inventory/memory/stats): `LB` / `RB`
- Pause/System: `Start`
- Optional quick panel: `Y` / Triangle

### Input Feel

- Inputs should be responsive and deterministic, favoring readability over visual noise.
- Board-combat targeting must provide clear pre-placement preview/highlighting.
- Common actions require minimal input friction; no complex multi-button chords for core loop actions.
- Camera/UI transitions should be brief and functional, not decorative.

### Accessibility Controls

- Full key rebinding (keyboard and gamepad remap support where feasible).
- Adjustable text size and UI scale for clue readability.
- Colorblind-safe board and status icon palette with high-contrast mode.
- Optional reduced camera shake / reduced motion.
- Distinct audio cues for damage/heal/status events plus independent volume sliders (music/SFX/UI/voice).
- Persistent tooltip mode for Souvenir effects and status definitions.

## Roguelike Specific Design

### Run Structure

- **Target run length (MVP):** ~30 minutes (P50 target band: 25-35 minutes).
- **Run flow:** fixed floor sequence with randomized encounters, rewards, and modifiers.
- **Start conditions:** run begins in the rebirth hub with preparation/loadout decisions, then floor 1 entry.
- **Scaling model:** difficulty increases via floor index, elite modifiers, and boss phase thresholds.
- **Run victory (MVP):** defeat the chapter final guardian.
- **Run failure:** hero death immediately ends the run and returns the player to hub recovery.

### Procedural Generation

- **Generation model:** semi-procedural structure (fixed macro progression, randomized micro content).
- **Randomized elements:** encounter types, enemy pack composition, reward offerings, and room modifiers.
- **Narrative control:** handcrafted anchor rooms/events preserve coherent story beats.
- **Seed policy (MVP):** internal run seed supports reproducibility/debugging; no player-facing seed system required.

### Permadeath and Progression

- **Permadeath rule:** run-limited resources, consumables, temporary blessings, and current floor progress are lost on death.
- **Persistent progression:** permanent Souvenir unlocks, permanent skills, clue/memory discoveries, narrative flags, and chapter milestones are retained.
- **Conversion on death:** selected run earnings convert to meta progression currency/value under chapter rules.
- **Meta loop:** rebirth hub spend/upgrade cycle prepares the next run with stronger long-term options.

### Item and Upgrade System

- **Core combat itemization:** Souvenirs (cards) with life values and trigger effects define the combat identity.
- **MVP itemization principle:** compact, highly readable, and low-overlap categories.
- **MVP item classes:** persistent Souvenirs, temporary divine favors/modifiers, and a very small number of consumables/quest objects.
- **Build variety:** adjacency and sequencing synergies, plus selective god-favor modifiers.
- **Rarity model:** compact and readable rarity tiers (e.g., Common/Rare/Epic) with clear value signaling.
- **Scope guard:** avoid broad loot taxonomies that dilute Souvenir-centric gameplay.

### Character Selection

- **MVP character model:** single playable hero (the condemned warrior).
- **No alternate playable characters in MVP.**
- **Identity progression:** memory recovery and demigod ascension milestones provide growth fantasy.
- **Replay diversity source:** build variation and persistent consequence flags, not class roster expansion.

### Difficulty Modifiers

- **Primary difficulty curve:** floor-based escalation with controlled elite spikes.
- **Readability rule (MVP):** difficulty should increase decision pressure, not board confusion.
- **Escalation priority:** scale via numbers, timing pressure, enemy rulesets, and selective modifiers.
- **Avoid in MVP:** too many simultaneous board-effect exceptions that reduce clarity and create swingy outcomes.
- **Challenge scope (MVP):** one clear default path; optional challenge toggles deferred post-MVP.
- **Achievement linkage:** Steam achievements tied to chapter clear, progression milestones, and selected challenge conditions.

## Progression and Balance

### Player Progression

Tower Oblivion uses two separated progression tracks:

1. **Combat progression through XP**  
XP improves the hero's combat-facing upgrade tiers:
- **Life:** supports survival, risky sequencing, delayed combos, and sacrifice-oriented Souvenirs.
- **Strength:** supports direct pressure, adjacency damage, and breaking enemy formations.
- **Psyche:** supports control, memory, fear/debuff effects, prediction, and board-state manipulation.
- **Mana:** supports spell-like Souvenirs, rare high-impact effects, combo acceleration, and divine-energy plays.

XP progression should create new build hypotheses rather than passive stat inflation. Combat upgrades improve reliability and open stronger Souvenir expressions.

2. **Exploration/narrative progression through meta-currency**  
Meta-currency is spent in the rebirth hub to unlock non-combat skills and abilities:
- thief / lockpicking skills
- treasure access
- trap detection
- charisma or dialogue options
- room access
- narrative option unlocks
- investigation utilities

Meta-currency expands where the player can go, what risks they can take, and which narrative routes become available.

#### Souvenir Requirement Rule

For MVP, Souvenir requirements are usually **bonus conditions**, not hard locks.

Examples:
- A Souvenir remains playable by default.
- If the hero has **Psyche tier 2+**, it gains `+2 damage`.
- If the hero has **Strength tier 2+**, it also damages adjacent enemies.
- If the hero has **Mana tier 2+**, it triggers an additional divine effect.

Hard requirements are rare, clearly telegraphed, and reserved for aspirational or late-chapter Souvenirs.

Requirements should be tied to explicit upgrade tiers or named unlock flags, not raw stat values. This protects balance tuning and save compatibility.

#### Progression Types

- **Skill progression:** player learns board placement, adjacency effects, sequencing, and risk evaluation.
- **Power progression:** hero combat upgrade tiers improve through XP.
- **Narrative progression:** clues, memories, and persistent narrative flags reveal the tower's mystery.
- **Content progression:** skills open new rooms, treasures, puzzles, and event variants.
- **Collection progression:** Souvenir unlocks expand deckbuilding options.

#### Progression Pacing

- First XP upgrade target: within 3-5 minutes.
- First meaningful build choice target: <=10 minutes.
- First failed run should surface at least one visible meta-progression unlock or near-unlock.
- Early runs teach XP and one combat stat before layering meta-currency and Souvenir bonus requirements.
- Souvenir bonus requirements should appear after the player has already equipped and used basic Souvenirs successfully.

### Difficulty Curve

Difficulty follows a controlled escalation curve.

#### Challenge Scaling

- Difficulty increases by floor index, elite modifiers, boss phase thresholds, and enemy board quality.
- Higher floors increase decision pressure, not board confusion.
- Combat difficulty scales through enemy life, enemy Souvenir quality, clearer sequencing pressure, and selective modifiers.
- MVP avoids too many simultaneous board-rule exceptions.
- Avoid hidden stat requirement pressure as a main difficulty lever.

#### Difficulty Options

- MVP uses one clear default difficulty path.
- Accessibility support should come from readable UI, clear previews, undo/preview where appropriate, explicit tooltips, and strong onboarding.
- Optional challenge modes are post-MVP.

### Economy and Resources

Tower Oblivion has a small MVP economy with no in-run shop or purchase system.

#### Resources

- **XP:** earned through combat and progression; used for persistent combat upgrade tiers.
- **Meta-currency:** earned through runs, death conversion, and milestones; spent in the rebirth hub on exploration, utility, and narrative skills.
- **Souvenirs:** unlocked permanently into the available pool; deployed through battle hands/decks.
- **Temporary blessings:** earned from encounters/events only; lost on death.
- **Consumables / quest objects:** limited in scope for MVP; earned from encounters/events only.

#### Economy Flow

Explore -> fight -> earn XP/resources/Souvenirs -> choose rewards -> die or clear -> convert selected run gains -> spend meta-currency in hub -> re-enter with stronger combat expression or broader exploration options.

#### Economy Boundaries

- XP improves combat upgrade tiers only.
- Meta-currency unlocks exploration and narrative abilities only.
- Temporary rewards create run texture and surprise, but do not become permanent progression.
- No purchasable temporary blessings or consumables in-run.
- Do not let meta-currency buy combat stats.
- Do not let XP unlock hub traversal, room access, or narrative skills.

#### MVP Economy Guardrails

- Keep resource categories minimal and visually distinct.
- Avoid broad loot categories that compete with Souvenirs.
- Souvenir requirements should usually unlock bonuses or dormant effects, not reject rewards.
- No run should present more than one unusable reward option unless the player knowingly entered a specialized path.
- Store upgrade levels, unlocked Souvenirs, narrative flags, and currency totals as persistent data; recompute derived combat stats on load.

### Progression UX Requirements

The first-session progression contract must be explicit:
- XP makes the hero survive and fight better.
- Meta-currency lets the hero explore more of the tower.
- Souvenirs shape the build, with some dormant effects unlocked by upgrade tiers.
- Failed runs still move the hero forward.

Post-run summary should clearly show:
- **Combat Growth:** Life, Strength, Psyche, Mana improvements.
- **Tower Knowledge:** clues, memories, narrative flags, and exploration unlocks.
- **Next Opportunity:** newly reachable rooms, affordable skills, or newly activated Souvenir bonuses.

## Level Design Framework

### Structure Type

Tower Oblivion uses a **hub-based roguelite tower structure**.

The rebirth hub acts as the persistent safe space for upgrades, memory review, narrative beats, and re-entry. Each run sends the player into a fixed floor sequence with randomized room content, encounters, rewards, and modifiers.

The structure is not open world. It is a controlled ascent with replay variation.

### Level Types

- **Rebirth Hub:** safe upgrade/menu/narrative space after death or run completion.
- **Exploration Rooms:** point-and-click scenes with clues, objects, traps, treasures, and skill checks.
- **Combat Rooms:** rooms that trigger 4x4 Souvenir board battles.
- **Choice/Event Rooms:** narrative decisions, god encounters, memory fragments, or consequence flags.
- **Reward Rooms:** Souvenirs, temporary blessings, XP, meta-currency, quest objects.
- **Locked/Secret Rooms:** gated by meta skills such as lockpicking, trap detection, charisma, or memory flags.
- **Elite Rooms:** harder variants with stronger rewards and controlled modifiers.
- **Guardian Rooms:** floor or chapter climax fights.
- **Anchor Narrative Rooms:** handcrafted story beats placed at fixed floor/chapter points.

#### Tutorial Integration

The first 8-10 minutes use a guided micro-loop:
one clue -> one Souvenir combat -> one XP upgrade -> one death/rebirth or recovery moment -> one visible permanent improvement.

Tutorial content is embedded in normal rooms, not separated as a long standalone tutorial.

#### Special Levels

- MVP includes one chapter final guardian encounter.
- Zeus is foreshadowed but not confronted in MVP.
- Secret rooms should exist in small quantity and demonstrate meta-skill value.

### Level Progression

Progression follows a **fixed macro sequence with semi-procedural room content**.

Players climb floors in order during each run. Individual rooms vary through randomized encounter type, enemy composition, reward offering, modifier, and selected narrative/event variants.

#### Unlock System

- Floor access is primarily run-based: survive and ascend.
- Room access is partially gated by permanent skills and memory/narrative flags.
- New room variants can unlock after death, clue discovery, or meta-skill purchase.
- Final guardian access requires reaching the chapter end through the floor sequence.

#### Replayability

- Players replay from the basement/rebirth hub after death.
- Replays are meaningful because permanent skills, Souvenirs, clues, and narrative flags change available options.
- Semi-procedural room variation keeps the ascent familiar but not identical.
- Secret/locked rooms give players concrete reasons to revisit earlier floors with new abilities.

### Level Design Principles

- **One readable purpose per room:** clue, combat, reward, choice, risk, or transition.
- **Escalate decision pressure, not visual/board confusion.**
- **Every run should reveal or suggest one future opportunity.**
- **Narrative anchor rooms preserve story coherence inside procedural variation.**
- **Skill gates should create anticipation, not frustration.**
- **Rooms should be short enough to preserve the 30-minute run target.**

## Art and Audio Direction

### Art Style

Tower Oblivion uses a **2D illustrated point-and-click visual style** with low animation complexity.

The visual identity should prioritize strong still images, readable interactive objects, and atmospheric composition over technical spectacle. Rooms should feel like authored mythic scenes: sacred architecture, oppressive tower spaces, divine traces, forgotten battle memories, and symbolic environmental storytelling.

Art and audio are gameplay communication systems, not only mood layers. If the player misreads an interactable, combat state, exit, danger, or memory clue, the presentation has failed its purpose.

#### Visual Pillars

- **Tower scale:** rooms should imply impossible verticality and the oppressive scale of divine punishment.
- **Divine decay:** sacred architecture should feel old, wounded, and corrupted by repetition.
- **Ritual architecture:** doors, altars, locks, traps, and guardians should feel ceremonial rather than generic dungeon props.
- **Souvenir symbolism:** relics, scars, broken honors, and memory fragments should recur visually.
- **Impossible verticality:** ascension should be visible through composition, lighting, and room structure.
- **Memory corruption:** recovered memories should feel intimate, broken, warm, or spectral.
- **Divine judgment:** punishment should feel monumental, cold, imposed, and inescapable.

#### Visual References

Reference direction:
- mythic dark fantasy
- heroic fantasy ruins and sacred architecture
- illustrated adventure-game room composition
- point-and-click readability
- divine grandeur mixed with punishment and introspection

The game should avoid generic dark fantasy mud. Images should be beautiful, colorful, readable, and commercially usable.

#### Color Palette

- Dark mythic foundation: stone, shadow, old gold, faded marble, celestial blue, ember red.
- Divine contrast colors: gold, white, lightning blue, Promethean ember, spectral memory tones.
- Floor identity should shift subtly as the player ascends.
- Interactive elements must remain visually distinct from decorative detail.

#### Camera and Perspective

- 2D fixed-room presentation.
- Point-and-click scene framing.
- 4x4 combat board uses a dedicated readable tactical view.
- Minimal animation: ambient effects, UI highlights, hit feedback, card/Souvenir placement, divine impact cues.

#### Visual Hierarchy and Readability

- Exploration rooms may be rich and atmospheric, but interactable objects need consistent silhouette, lighting, local contrast, or highlight language.
- Exits, danger traces, memory clues, run-state changes, and reward objects must be visually legible.
- The combat board should use cleaner shapes than room illustrations: stable grid contrast, readable tokens, distinct hover/target/selection states, attack previews, threat zones, and status icons.
- Combat meaning must never rely on color alone; use icons, border language, motion pulses, text labels, or sound cues.
- Every room and combat state requires a readability checklist before final art approval.

### Production and Asset Pipeline

MVP production should use a small, repeatable visual grammar:
- fixed-room illustrations
- limited interactive object states
- one combat board style
- one icon set
- reusable UI/feedback kit
- restrained selective VFX

#### Room Asset Contract

Each room should follow a consistent Unity-friendly asset structure:
- base illustrated room image
- optional foreground layer
- lighting/color overlay layer
- interactive object sprites or highlight masks
- selective VFX layer
- UI layer separate from room art

The project should define target resolution, layered source format, export format, naming rules, import presets, texture compression rules, and memory budget per room during technical planning.

### Audio and Music

Audio should communicate brutal physical struggle, supernatural divine intervention, sacred architecture, and the hero's gradual shift from mortal to demigod.

#### Music Style

Music direction: **mythic dark fantasy with divine grandeur and tragic ascension**.

The soundtrack should support:
- mystery and introspection during exploration
- tension before combat
- sacred and oppressive tower atmosphere
- growing divine scale as the hero ascends
- chapter guardian intensity without overproducing the MVP

Purchased music is acceptable for MVP if licensing supports commercial Steam distribution, trailer use, streaming/video coverage, and avoids Content ID conflicts where possible.

#### Sound Design

Core SFX identity is a hybrid of:
- grounded melee impact
- stone / temple / celestial ambience
- divine energy signatures
- clear gameplay readability

Combat and interaction SFX must be distinct for:
- inspect
- valid click
- invalid click
- board select
- Souvenir placement
- attack commit
- damage
- shield/block
- status trigger
- divine charge/intervention
- victory/failure

Divine ambience must not mask interaction feedback. Combat readability is more important than audio spectacle.

#### Reusable Feedback Kit

The MVP should rely on reusable feedback tokens rather than bespoke animations per enemy or card:
- attack telegraph
- impact flash
- status pulse
- damage number
- tile danger overlay
- shield gain
- debuff marker
- divine trigger cue

#### Voice/Dialogue

Voice scope: **partial voice**.

VO is a premium layer, not a comprehension dependency. All important information must also appear in text and/or clear gameplay feedback.

Recommended MVP scope:
- narrator or god barks for major moments
- key divine encounters
- boss/guardian intros
- run-start / run-end reactive lines
- major story beats only

Avoid voicing routine procedural text. Every voiced line should have an ID, transcript, speaker, language, file path, usage context, and fallback behavior.

### Asset and Licensing Constraints

AI-generated, AI-assisted, purchased, edited, or commissioned assets may be used only if commercial usage rights are clear and documented.

#### Asset Provenance Gate

No asset enters the shipping build unless its provenance is documented. The asset register should track:
- asset ID
- source or vendor
- license terms
- AI tool/model used, if any
- prompt/reference inputs, if applicable
- human edit notes
- commercial-use status
- attribution requirements
- replacement risk rating

#### Style and Rights Safety

- Do not use prompts or briefs that reference living artists, named franchises, copyrighted characters, or "in the style of X."
- Avoid protected franchise styles, recognizable copyrighted likenesses, and unclear third-party assets.
- Prepare a plain-language Steam AI usage disclosure early, covering AI-assisted concepting/production, human curation, and rights tracking.
- Purchased SFX/music must allow commercial game use and should support modification, trailer use, streaming/video coverage, and low Content ID risk where possible.

### Aesthetic Goals

- Reinforce the feeling of divine punishment and repeated ascent.
- Make memory, death, and rebirth feel sacred rather than purely mechanical.
- Make every room communicate curiosity, unease, temptation, or divine scrutiny.
- Keep rooms readable for point-and-click interaction.
- Keep combat feedback clear enough that players understand every Souvenir trigger.
- Support the narrative arc from condemned mortal to emerging demigod.

## Technical Specifications

### Performance Requirements

Tower Oblivion targets stable PC performance with a lightweight 2D presentation, fixed-room scenes, and a dedicated 4x4 tactical board combat view.

#### Frame Rate Target

- Target: 60 FPS.
- Minimum acceptable: 30 FPS.
- Frame time target: ~16.7 ms at 60 FPS.
- Performance priority: readability and input responsiveness over decorative effects.

#### Resolution Support

- Primary target: 1080p.
- UI must scale cleanly for common PC resolutions.
- Combat board, tooltips, and point-and-click hotspots must remain readable at 1080p.
- 4K support is optional/post-MVP, but assets should not block future upscaling.

#### Load Times

- Run start and room transitions should remain short enough to preserve pacing.
- MVP target: no long loading interruption between standard rooms.
- Larger room illustrations should be loaded one room context at a time unless backtracking requires otherwise.

### Platform-Specific Details

#### PC / Steam Requirements

- Primary launch platform: PC via Steam.
- Engine: Unity.
- Input: mouse + keyboard and gamepad support.
- Steam feature target: achievements.
- Online features for MVP: none required.
- Save system: local save for meta-progression, unlocks, Souvenirs, narrative flags, and settings.
- Cloud saves: optional/post-MVP unless added during Steam integration planning.
- Mod support: out of scope for MVP.

#### Save Integrity Requirements

Persistent save data should store:
- upgrade levels
- unlocked Souvenirs
- memory/clue discoveries
- narrative flags
- meta-currency totals
- settings
- achievement-relevant milestones

Derived combat stats should be recomputed on load rather than stored as authoritative values.

Save structure should separate:
- `RunState`
- `CombatState`
- `MetaState`
- `WorldSeedState`

MVP save system should support versioning, validation, and recovery fallback to reduce corruption risk.

### Asset Requirements

#### Art Assets

Major art asset categories:
- fixed 2D room illustrations
- optional room foreground/overlay layers
- interactive object sprites or highlight masks
- combat board UI
- Souvenir card art/icons
- status icons and board effect tokens
- VFX feedback kit
- hub UI and progression screens
- Steam capsule/key art later in production

Room assets should follow a consistent contract:
- base room image
- optional foreground layer
- lighting/color overlay
- interactive object/highlight layer
- selective VFX layer
- separate UI layer

Asset pipeline requirements:
- consistent naming rules
- import presets
- texture compression rules
- memory budget per room
- asset provenance tracking

#### Audio Assets

Major audio asset categories:
- exploration ambience
- combat music
- guardian/boss music
- UI sounds
- board interaction SFX
- Souvenir trigger SFX
- divine favor SFX
- damage/heal/status cues
- room transition sounds
- partial VO lines for gods/guardian/key story moments

Audio must support gameplay clarity. Important feedback cues must be distinct and must not depend only on music or VO.

#### External Assets

Planned external assets:
- purchased SFX/music with commercial Steam usage rights
- AI-assisted or AI-generated 2D images only when commercial rights and provenance are documented
- possible purchased UI/audio tooling if needed

No external asset should enter the shipping build without documented source, license, usage rights, attribution requirements, and replacement risk.

### Technical Constraints

- Solo part-time development scope.
- Unity project should favor simple, reusable systems over bespoke per-room/per-card implementations.
- MVP should avoid animation-heavy combat or room production.
- Board combat feedback should use reusable tokens/effects.
- Art pipeline must protect readability and commercial licensing.
- No in-run shop system for MVP.
- No online dependencies for MVP.
- No mobile platform support in MVP scope.
- The core game engine must be reusable for future point-and-click narrative games with turn-based board combat.

### Reusable Game Engine Requirement

The Tower Oblivion MVP should be built as a reusable game framework for future games using similar mechanics: point-and-click exploration, narrative choice/consequence systems, meta-progression, and turn-based board combat.

Reusable engine modules should include:
- **Scene/Room Module:** fixed-room point-and-click scenes, hotspots, exits, object inspection, and room-state changes.
- **Narrative Module:** dialogue/event triggers, persistent flags, memory/clue unlocks, and consequence tracking.
- **Combat Module:** reusable turn-based board-combat framework, including board state, card/Souvenir definitions, placement rules, trigger effects, AI decision hooks, and deterministic resolution.
- **Progression Module:** XP, meta-currency, upgrades, unlocks, and post-run conversion.
- **Save Module:** versioned local save data, state separation, validation, and recovery.
- **Content Data Module:** data-driven definitions for rooms, encounters, Souvenirs/cards, rewards, narrative events, and modifiers.

Design principle:
Game-specific content should live in data/assets where possible, while reusable mechanics remain isolated in shared systems. Tower Oblivion should be the first game using the engine, not a one-off implementation.

## Development Epics

### Epic Overview

| # | Epic Name | Scope | Dependencies | Est. Stories |
|---|---|---|---|---|
| 1 | Minimal Reusable Game Foundation | Thin reusable primitives: state boundaries, save/load, input/UI shell, data definitions, scene/combat/narrative module seams | None | 8 |
| 2 | Playable Identity Vertical Slice | One mythological room, one discovery, one Souvenir reward, one thin 4x4 combat stub, one consequence, death/rebirth, saved progression | Epic 1 | 9 |
| 3 | Souvenir Board Combat MVP | Complete deterministic 4x4 combat, readability, enemy intent, combo behavior, board feedback | Epics 1-2 | 10 |
| 4 | Exploration and Narrative Rooms | Point-and-click room templates, hotspots, clues, flags, locked/secret rooms, readability rules | Epics 1-2 | 9 |
| 5 | Progression, Economy, and Rebirth Hub | XP, meta-currency, upgrade tiers, skills, post-run summary, unlocks | Epics 1-2 | 9 |
| 6 | Chapter Run Content and Guardian | Fixed floor sequence, room pools, modifiers, onboarding micro-loop, chapter guardian | Epics 3-5 | 10 |
| 7 | Art, Audio, UX, and Production Pipeline | Asset contract, feedback kit, audio cues, partial VO, provenance register, full presentation pass | Epics 2-6 | 8 |
| 8 | Steam MVP Polish and Release Readiness | Achievements, settings, save validation, performance pass, QA, build packaging | Epics 1-7 | 8 |

### Recommended Sequence

1. Build a minimal reusable foundation only as far as needed to support the first playable slice.
2. Validate the game identity immediately through a playable vertical slice.
3. Deepen combat, exploration, and progression after the loop proves value.
4. Build chapter content and the guardian once core systems are stable.
5. Complete full art/audio pipeline, production polish, Steam readiness, and QA last.

### Vertical Slice

**The first playable validation gate:**  
A short playable sequence where the player starts in the rebirth hub, enters one mythological illustrated room, inspects a meaningful discovery, earns or selects one Souvenir reward, uses that Souvenir in a thin 4x4 combat stub, experiences one consequence, dies or resolves the run, returns to the hub, sees retained progression, and starts again with a visible improvement.

The combat stub validates board presentation, Souvenir placement, basic feedback, and the room-to-combat-to-rebirth transition. Full board rules, enemy AI, effect variety, and balancing are implemented in Epic 3.

### Non-Overlap Rules

- Epic 1 provides reusable primitives and module seams only; it does not implement full gameplay or content.
- Epic 2 proves the player-facing identity with a thin combat stub; it does not implement the full combat system.
- Epic 3 owns full combat rules, enemy AI, effect variety, combat readability, and balancing.
- Epic 4 owns room and narrative frameworks plus proof rooms; Epic 6 owns MVP chapter content volume and pacing.
- Epic 5 owns gameplay progression and hub upgrades; Epic 8 owns save hardening, release stability, and Steam readiness.
- Epic 7 owns full production pipeline and presentation polish; core readability must already exist in Epics 2-4.

### Testability Gates

- Save/load restores hub progression.
- Death triggers rebirth and persists earned progression.
- Combat encounter can start, resolve, and grant reward.
- Room flow advances deterministically.
- Core framework has no hard Tower-specific dependency.
- Essential room and combat readability are validated before content expansion.

## Success Metrics

### Technical Metrics

Tower Oblivion technical success is measured by stability, save integrity, performance, input reliability, and asset pipeline safety.

#### Key Technical KPIs

| Metric | Target | Measurement Method |
|---|---|---|
| Target framerate | 60 FPS at 1080p on mid-tier PC hardware | Unity profiler, build profiling sessions |
| Minimum acceptable framerate | 30 FPS | Low-end hardware profiling |
| Frame time target | ~16.7 ms at 60 FPS | Unity profiler |
| Crash-free session rate | 98.5%+ | Playtest logs / crash reporting |
| Save corruption | 0 critical cases | Save/load test suite, interrupted-session tests |
| Room transition load | No long interruption between standard rooms | Build profiling |
| Input reliability | Mouse/keyboard and gamepad usable for full MVP loop | QA checklist |
| Asset provenance coverage | 100% of shipping assets documented | Asset register review |
| Commercial asset rights | 100% cleared before shipping build | License checklist |
| Reusable engine boundary | No hard Tower-specific dependency in core reusable modules | Architecture/code review |

### Gameplay Metrics

Gameplay success is measured by whether players understand the loop, feel death is meaningful, make build choices quickly, and want to replay.

#### Key Gameplay KPIs

| Metric | Target | Measurement Method |
|---|---|---|
| MVP run length | P50: 25-35 minutes | Playtest session telemetry/manual timing |
| First XP upgrade | Within 3-5 minutes | Playtest telemetry/manual timing |
| First meaningful build choice | <=10 minutes | Playtest telemetry/manual observation |
| First-session completion | 70%+ | Playtest cohort tracking |
| 48h return rate | 25-35%+ | Playtest/demo analytics |
| Players earning visible permanent progress within first 2 runs | 80%+ | Playtest progression logs |
| Death felt worth it sentiment | 70%+ positive | Post-run survey |
| Demo page visit -> demo install/download | 20%+ | Steam analytics |
| Demo players -> wishlist | 25%+ | Steam analytics |
| Steam page visit -> wishlist | 10-20% | Steam analytics |
| Tester review score | 7.5/10+ average | Structured tester survey |
| Steam sentiment goal | Very Positive direction | Review monitoring after release/demo |

### Design-Specific Metrics

| Metric | Target | Measurement Method |
|---|---|---|
| Combat readability | Players can explain why they won/lost after most battles | Post-battle survey / observation |
| Souvenir reward usability | No run presents more than one unusable reward unless player chose a specialized path | Reward generation test/log review |
| Board-state clarity | Player understands valid moves, previewed effects, and outcome changes within ~1 second | Playtest observation |
| Post-run clarity | Player can identify what was kept, lost, converted, and newly unlocked | Post-run survey |
| Exploration readability | Players identify interactable objects without excessive pixel hunting | Playtest observation |
| Narrative curiosity | Players mention unanswered mystery, memory, gods, or tower purpose after first session | Interview notes / survey tags |

### Qualitative Success Criteria

- Players describe the game using target terms: mysterious, tense, readable, replayable, story-rich, tactical.
- Players understand that death is part of progress, not simple failure.
- Players want to try "one more run" because they have a concrete next plan.
- Players remember Souvenirs as identity fragments, not generic cards.
- Players notice the difference between combat growth and exploration/narrative growth.
- Players mention the mythological punishment and Prometheus memory premise without needing external explanation.
- Testers identify the 4x4 Souvenir board as a distinctive combat hook.
- The game feels premium despite low animation through composition, sound, UI feedback, and writing.

### Metric Review Cadence

- **During early prototype:** review after each playable milestone.
- **During vertical slice:** review after every structured playtest session.
- **During MVP content production:** review weekly or after each build.
- **Before Steam demo/release:** review technical stability, first-session metrics, wishlist funnel, and qualitative feedback together.

## Out of Scope

The following items are explicitly outside the MVP/v1.0 scope:

- Mobile release and Google Play support.
- Console ports.
- Online multiplayer or online services.
- In-run shop or purchasable temporary blessings/consumables.
- Mod support or level editor.
- Full voice acting.
- Cinematic-heavy cutscenes.
- Large open world structure.
- Deep branching story trees.
- Complex god relationship matrix.
- Broad loot taxonomy beyond Souvenirs, temporary blessings/modifiers, and limited consumables/quest objects.
- Large enemy roster or many biomes.
- Full Zeus confrontation.
- Advanced animation-heavy combat presentation.
- Full localization.
- Player-facing run seed system.
- Generic multi-game editor or overbuilt reusable engine platform.

### Deferred to Post-Launch

- Android/mobile version if the PC/Steam release validates demand.
- Additional chapters and expanded tower floors.
- Zeus confrontation and full-release finale.
- Additional gods, blessings, and Souvenir families.
- Expanded narrative branches and optional story arcs.
- Optional challenge modes.
- Cloud saves.
- Additional languages.
- More advanced Steam/store marketing assets after the MVP visual direction is proven.

---

## Assumptions and Dependencies

### Key Assumptions

- Unity remains the engine for MVP development.
- PC/Steam remains the primary launch platform.
- Solo part-time development capacity requires strict scope control.
- AI-assisted art can support production only when commercial usage rights are documented.
- Purchased music/SFX will be available with commercial game, trailer, and streaming/video coverage rights.
- The 4x4 Souvenir board combat can be validated with a small card pool before larger content production.
- The first playable slice will guide architecture refinements before broad content expansion.
- Static illustrated rooms with strong feedback can feel premium without heavy animation.
- Players will accept low animation complexity if interaction readability, story, and combat feedback are strong.
- Reusability is achieved through clean module boundaries and data-driven content, not by building a broad engine product first.

### External Dependencies

- Unity editor and relevant Unity packages.
- Steamworks integration for achievements and release packaging.
- Purchased audio/music libraries with clear commercial licenses.
- AI image generation tools or external art sources with clear commercial-use terms.
- Asset provenance register maintained from the start of production.
- Local save testing tools/processes.
- Playtesters for vertical slice, combat readability, and first-session validation.

### Risk Factors

- Scope creep from combining roguelite, point-and-click adventure, tactical combat, RPG progression, and narrative choice systems.
- Save/meta-progression corruption affecting player trust.
- Combat readability degradation as Souvenir effects, divine modifiers, and enemy rules stack.
- AI/purchased asset licensing ambiguity blocking commercial release.
- Solo part-time development overload.
- Static room presentation feeling underproduced if composition, audio, writing, and UI feedback are not strong.
- Reusable engine ambition expanding beyond what Tower Oblivion MVP needs.

---

## Document Information

**Document:** Tower Oblivion - Game Design Document  
**Version:** 1.0  
**Created:** 2026-04-25  
**Author:** Franck  
**Status:** Complete

### Change Log

| Version | Date | Changes |
|---|---|---|
| 1.0 | 2026-04-25 | Initial GDD complete |
