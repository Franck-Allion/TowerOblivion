---
stepsCompleted: [1, 2, 3, 4, 5, 6, 7, 8]
inputDocuments: []
documentCounts:
  brainstorming: 0
  research: 0
  notes: 0
workflowType: 'game-brief'
lastStep: 8
project_name: 'IA'
user_name: 'Franck'
date: '2026-04-22T20:57:25+02:00'
game_name: 'Tower Oblivion'
---

# Game Brief: Tower Oblivion

**Date:** 2026-04-22
**Author:** Franck
**Status:** Draft for GDD Development

---

## Game Vision

### Core Concept

Tower Oblivion is a story-driven heroic-fantasy roguelite investigation game where a condemned mortal repeatedly climbs Zeus's celestial tower, keeping memory, skills, and consequences across runs as he transforms toward demigodhood.

### Elevator Pitch

After daring to rival the gods, a mortal warrior is condemned by Zeus to die and be reborn forever at the foot of a divine tower. Prometheus secretly preserves his memory, turning each ascent into a trial where Souvenirs of past glory, divine favors, and hard-won knowledge carry forward. Run by run, he ceases to be merely mortal, until the final ascent forces Zeus to face what his punishment created.

### Vision Statement

Tower Oblivion aims to deliver an addictive "one more run" loop where narrative discovery is as rewarding as progression, and where player choices can sacrifice short-term gain for long-term impact across runs. The game's identity is not technical spectacle but a coherent mystery, strong worldbuilding, and deep meta-progression that makes every attempt meaningful. The MVP will prioritize a modular gameplay engine (runs, progression, memory system, inventory, event resolution) so content, combat innovation, and story branches can be expanded cleanly over time.

## Target Market

### Primary Audience

Tower Oblivion targets a Steam-first MVP audience: PC players aged 16+ who want a narrative mystery delivered through repeatable roguelite runs with accessible but meaningful build decisions.

**Demographics:**
- Age range: 16+
- Platform: PC first (Steam)
- Session pattern: ~30 minutes per run (MVP target)

**Gaming Preferences:**
- Story-rich progression over pure mechanical mastery
- Roguelite structure with clear meta-progression
- Fast onboarding and early agency (first level-up + first meaningful build choice within 10 minutes)

**Motivations:**
- Discover new truth/clues each run
- Optimize builds over time
- Feel persistent advancement even after death

### Secondary Audience

Post-MVP expansion audiences:
- Story/adventure players who prefer investigation and narrative payoff
- RPG progression fans who value long-term build development
- Mobile players via adapted run structure and UX (short-session design, interruption-friendly flow)

### Market Context

MVP strategy is Steam-first validation before mobile expansion.

**Positioning:**
A mystery-driven roguelite where each ascent reveals narrative truth and forces impactful build decisions.

**Steam Discoverability Baseline:**
- Tag stack: Roguelite, Story Rich, RPG, Choices Matter, Atmospheric
- Keep tagging focused (avoid over-tagging)

**Competitive Framing:**
- Competes for attention with story-forward roguelites and progression-focused indie RPGs
- Differentiates through the balance of roguelite loop + investigation framing + persistent memory/skill progression

**Market Opportunity:**
- Address players underserved by action-first roguelites by prioritizing narrative coherence and repeat-run meaning
- Validate with Steam metrics first, then adapt for mobile distribution

**Validation Signals for MVP:**
- Store page to wishlist conversion
- Demo/first-run completion
- Run-2 return rate
- Early retention of players who reach first build choice within 10 minutes

## Game Fundamentals

### Core Gameplay Pillars

1. **Die, remember, return stronger**  
Failure is meaningful: temporary loadout is lost, while skills, discovered clues, and persistent world flags are retained.

2. **Investigate through replay**  
Each run advances both traversal and understanding; the tower is a mystery system, not only a combat ladder.

3. **Choices shape future runs**  
A limited set of persistent consequences (MVP) carries across runs: shortcut state, threat state, chamber/event reveal state.

4. **Tense room-by-room survival**  
Every room presents readable risk-reward decisions with fair telegraphing.

**Pillar Priority:** `1 > 2 > 3 > 4`

### Primary Mechanics

- `explore`
- `inspect`
- `interact` (open/activate/dialogue-trigger)
- `collect clue`
- `place souvenir card`
- `resolve placement effect`
- `use skill`
- `choose upgrade`

**Core Loop:**  
Enter room -> read danger/opportunity -> inspect + interact -> collect clues -> if combat, resolve a 4x4 Souvenir board duel -> claim rewards and choose upgrades/divine favors -> push upward -> die -> return with persistent skills, clues, and world-state flags that alter future runs.

**Combat Model (MVP):**
- Board: 4x4.
- Hero and enemy each play 8 Souvenir cards (one placement per turn).
- Each placed card has life and triggers an immediate placement effect.
- Battle end: after all placements, or if a hero life total reaches 0.
- Win rule: side with highest hero life wins.
- Tiebreaker 1: side with highest total surviving card life on board wins.
- Tiebreaker 2: if still tied, hero wins.
- Souvenir cards represent fragments of the hero's glorious past, making progression both mechanical and narrative.

**Implementation Baseline (v0.1):**
- Starting hero life: Hero 30 / Enemy 30 (tunable in balance passes).
- Placement legality: cards can only be placed on empty cells.
- Turn order: strict alternating turns, one placement per turn.
- Capture/flip rule: ownership changes occur only through explicit card effects (no implicit adjacency flip).
- Card destruction: card is removed when card life reaches 0 or below.
- Effect resolution order: place card -> resolve its effect -> resolve triggered destructions -> check hero defeat -> pass turn.
- Invalid targeting: if an effect targets a non-existing or empty target, that portion fizzles.

### Player Experience Goals

- **During a run:** tension, curiosity, meaningful progress, fair danger
- **After death:** immediate re-engagement ("I know what to try next")
- **After several runs:** obsession with unresolved mystery, pride in cumulative mastery, ownership of long-arc consequences

**Emotional Journey:**  
Curiosity + tension -> controlled risk escalation -> setback with retained meaning -> compounding agency across runs.

### MVP Guardrails (for pillar integrity)

- Consequence system limited to **3 persistent state families**: shortcuts, threat modifiers, reveal flags
- First meaningful level-up/build choice visible within **10 minutes**
- Run target around **30 minutes**
- Any new feature must map to at least one pillar; otherwise deferred

## Scope and Constraints

### Target Platforms

**Primary (MVP launch):** PC (Windows, Steam)  
**Secondary (post-MVP only):** Android (Google Play), after PC stabilization (minimum 2-3 patches and demand validation)

### Budget Considerations

Self-funded project.

- **In-house:** design, implementation, AI-assisted art production (Google Banana), QA, base marketing execution
- **External/purchased:** SFX and music packs
- **Budget pressure points:** Steam store assets/trailer quality, playtest coverage, potential art rework, release operations
- **Budget discipline rule:** any spend must improve one of the MVP KPIs (tutorial completion, run completion, wishlist conversion)

### Team Resources

Solo developer setup: 1 core member + AI agent support.

- **Availability:** evenings + part of weekends (part-time cadence)
- **Covered roles:** gameplay design, implementation, QA, basic marketing
- **Capacity gaps:** content throughput, visual consistency at ship quality, advanced store-page optimization, large-scale external playtesting
- **Scope protection:** no mid-sprint feature additions unless replacing existing scope

### Technical Constraints

- **Engine:** Unity
- **Performance targets:** 60 FPS target, 30 FPS minimum acceptable
- **Frame-time target:** ~16.7 ms at 60 FPS on target spec
- **Online features in MVP:** none
- **Save system in MVP:** local save only, versioned schema (`saveVersion` + migration path), corruption fallback

### Scope Realities

- PC-only at MVP launch. No Android-specific code paths before PC release candidate.
- MVP content slice is intentionally narrow:
  - one playable character
  - one starter Souvenir deck family
  - one progression tree (~12-20 nodes)
  - one tower theme/tileset
  - ~8-12 room modules
  - 3 regular enemy archetypes + 1 elite + 1 boss
  - ~20-30 Souvenir cards and 5-7 effect families
- Ban list for MVP: multiplayer, cloud saves, leaderboards, live-ops hooks, major procgen rewrites, dynamic lighting overhaul, ECS migration.
- Feature gate: if it does not improve the first 30 minutes of player experience, defer post-MVP.
- Marketing focus: one funnel only -> Steam wishlist growth.

### MVP Technical Exit Criteria

1. **Performance:** in a 20-minute stress run, average FPS >= 60 and 1% low FPS >= 30 on defined minimum-spec hardware.
2. **Save reliability:** 100 consecutive save/load cycles across at least 3 game states, with zero corruption and zero progression blockers.
3. **Stability:** 60-minute soak test with no crash, no soft-lock, and no Sev-1/Sev-2 bug in combat/progression/save-load core loop.

## Reference Framework

### Inspiration Games

**Hades**
- Taking: strong roguelite loop, meaningful between-run progression, narrative that evolves across deaths.
- Not Taking: action-heavy real-time combat focus, high mechanical-skill ceiling as core identity, fast pace as primary driver.

**Return of the Obra Dinn**
- Taking: deep deduction, player-driven understanding, satisfaction from connecting clues over time.
- Not Taking: static non-replayable structure, single-run progression, pure logic puzzle design without progression systems.

**Slay the Spire**
- Taking: run-based structure, branching paths, clear risk/reward at each step, build progression during a run.
- Not Taking: deckbuilding core loop, abstract combat as main interaction, purely numerical optimization focus.

**Disco Elysium**
- Taking: narrative-first design, skills that influence dialogue/interactions, psychological depth in writing.
- Not Taking: large open-world structure, constant high text density, purely dialogue-led gameplay without systemic exploration.

**Darkest Dungeon**
- Taking: oppressive atmosphere, attrition pressure across attempts, long-term progression affecting future runs.
- Not Taking: party-management complexity, turn-based squad combat as main focus, heavy resource micromanagement.

### Competitive Analysis

**Direct Competitor:**
- Myst

**Adjacent Attention Competitors:**
- Hades
- Return of the Obra Dinn
- Darkest Dungeon
- Disco Elysium

**Competitor Strengths:**
- Mystery titles: strong narrative puzzle engagement and memorable discovery moments.
- Roguelite titles: clear replay loop, strong run pacing, high retention from progression.
- Narrative RPG titles: strong character/skill-driven interaction depth.

**Competitor Weaknesses (relative to Tower Oblivion vision):**
- Usually emphasize one axis (puzzle-only, action-only, or dialogue-only) rather than combining investigation + replay + persistent consequence.
- Limited integration of failure as narrative/systemic progress in short structured runs.

### Key Differentiators

1. **Investigation-first replay loop**
Each run is designed to reveal information, not only deliver combat or loot progression.

2. **Persistent consequence architecture**
Choices in one run alter future runs through explicit state families (shortcuts, threat modifiers, reveal flags).

3. **Failure converts to strategic progress**
Temporary run power is lost on death, but retained knowledge/skills/world impact creates forward momentum.

4. **Narrative payoff in MVP-length sessions**
PC-first ~30-minute run structure balances tension, progression, and story revelation without requiring marathon sessions.

**Unique Value Proposition:**
Explore a mysterious tower in short, replayable runs where knowledge, choices, and consequences carry over between attempts, turning failure into investigation and progress into understanding.

## Content Framework

### World and Setting

Tower Oblivion takes place in a medieval heroic-fantasy tower world centered on confinement, ascent, and truth-seeking.
Atmosphere: **Mysterious, Introspective, Fantasy, Heroes**.

### Narrative Approach

Narrative is fragment-based and replay-compatible, with environmental storytelling as the primary delivery channel.

**Story Delivery (MVP):**
- 50% environmental storytelling
- 20% short dialogue
- 20% boon/item/relic text
- 8% milestone scenes
- 2% codex notes (deferred unless reused from shipped text)

Narrative rules:
- Run-start and run-end reactive lines
- One meaningful fragment per run minimum
- Story fragments shown only in safe moments (never mid-combat)

### Content Volume

MVP content scope: 1 core mystery thread (required), 1 secondary thread (optional stretch), 6-8 clues, 1-2 motifs, 1 small twist revealed by floor 3 clear.

---

## Art and Audio Direction

### Visual Style

- 2D presentation
- Very low animation complexity
- Readability-first composition and contrast

### Audio Style

- Music: mythic dark fantasy with divine grandeur and tragic ascension
- SFX: heavy, readable, divine, transformation-focused
- Voice: partial, capped for MVP (<=12 short bark-style lines), no lip-sync

SFX should communicate:
- brutal physical struggle
- supernatural divine intervention
- sacred architecture
- the hero's shift from mortal to demi-god

Core SFX identity:
- grounded melee impact
- stone/temple/celestial ambience
- divine energy signatures
- high gameplay readability

### Production Approach

- In-house: integration, AI-assisted art production, QA, base marketing
- Purchased: SFX/music packs
- Text-first narrative delivery for MVP
- No cinematic branching for MVP

---

## Risk Assessment

### Key Risks

1. **Performance degradation in complex scenes**  
Category: Technical | Likelihood: Medium | Impact: High

2. **Save/meta-progression corruption or unlock integrity loss**  
Category: Technical | Likelihood: Medium | Impact: High

3. **Combat readability loss under stacked effects/powers**  
Category: Technical / Production | Likelihood: High | Impact: High

4. **Solo part-time overload and feature creep**  
Category: Scope / Team-Time | Likelihood: High | Impact: High

5. **Weak Steam discoverability / wishlist conversion**  
Category: Market | Likelihood: Medium | Impact: High

6. **Narrative/content inconsistency across gods, boons, lore, and assets**  
Category: Production / Content | Likelihood: Medium | Impact: Medium

7. **Legal/commercial rights risk for AI-generated images**  
Category: Legal / Production | Likelihood: Medium | Impact: High

### Technical Challenges

- Real-time performance under effect-heavy scenes
- Reliable separation of run-state vs permanent progression
- Maintaining telegraph clarity as powers and enemy effects stack

### Market Risks

- Positioning may look familiar in myth-roguelite space
- Store messaging may under-communicate true differentiation

### Mitigation Strategies

- Enforce early budgets for VFX, shaders, UI effects, and spawn density; profile on target low-end PCs from first playable.
- Keep save architecture simple and versioned; separate run-state and meta-progression, with validation checks, backup slot, and migration tests.
- Define telegraph standards (shape cue + sound cue + minimum windup), limit overlapping VFX, and require readability playtests on stacked builds.
- Apply strict MVP scope gate and 1-in/1-out replacement rule after content lock.
- Center Steam positioning on core differentiators with gameplay-first trailer and clear one-sentence hook.
- Maintain a compact Lore Bible with clue IDs and contradiction checks before content lock.
- Run an AI asset compliance pipeline: provenance log, tool/license policy review, archive prompts/outputs, and replace any uncertain asset before release.

### Production Gates (Must Pass)

- Performance: stable 60 FPS at 1080p on target min-spec scene; no floor transition >2.5s.
- Save safety: versioned save schema + one backup slot + crash-safe atomic write; migration test required before release candidate.
- Combat readability: all enemy telegraphs readable at 1x speed without audio; no critical cue only in color (shape + sound redundancy required).
- UI clarity: minimal but decisive HUD; clear hit-direction cue, status icons with visible countdown arcs, and high-contrast damage/heal feedback. Decorative motion must never mask threat signals.
- Narrative consistency: single-source Lore Bible (1 page), clue IDs mapped to scenes/items, contradiction check before content lock.

## Executive Summary

Tower Oblivion is a story-driven heroic-fantasy roguelite investigation game where a condemned mortal repeatedly climbs a divine tower, dies, returns, and progressively transforms toward demigodhood through retained memory, skills, and consequences.

**Target Audience:** PC-first Steam players (16+) seeking mystery, progression, and meaningful replay in ~30-minute runs.

**Core Pillars:** Die, remember, return stronger; investigate through replay; choices shape future runs; tense room-by-room survival.

**Key Differentiators:** Investigation-first replay loop, persistent cross-run consequence states, failure as forward progression, narrative payoff in MVP-length sessions.

**Platform:** PC (Windows/Steam) for MVP; Android deferred post-MVP.

**Success Vision:** Validate a compelling first 30-minute loop with strong narrative hook, reliable progression integrity, and conversion/retention signals that justify expansion.

## Success Criteria

### MVP Definition

Absolute must-have MVP scope:
- One complete core loop: start run -> climb tower -> fight -> die -> return -> spend meta-progression -> re-enter.
- 4x4 divine capture/flip puzzle combat for encounters.
- Hero and enemy each play 8 Souvenir cards per battle.
- Card placement effects resolve immediately and drive tactical outcomes.
- Combat outcome rules are fixed and readable (hero-life win, then board-life tiebreak, hero-favored final tie).
- Run-based procedural or semi-procedural encounter progression through the tower.
- Death/resurrection loop explicitly tied to Zeus's punishment.
- Persistent meta-progression across runs.
- Prometheus memory mechanic reflected in retained progression and narrative unlocks.
- Divine favor system with a small set of gods and distinct build variation.
- Player starts with a small starter set of Souvenir cards from a glorious past, then expands through progression.
- Early demi-god progression fantasy visible in gameplay and presentation.
- One hub/rebirth space for upgrades, narrative beats, and re-entry.
- Basic save/load system that protects meta-progression integrity.
- Core narrative framing present in MVP: Zeus punishment, Prometheus aid, repeated ascent, growing recognition from other gods.
- One strong end-of-MVP goal: reaching a major upper-floor guardian.

### Success Metrics

Acquisition:
- Steam page visit -> wishlist: 10-20%
- Demo page visit -> demo download/install: 20%+
- Demo players -> wishlist: 25%+

Engagement and quality:
- First-session completion: 70%+
- 48h return rate: 25-35%+
- Crash-free session rate: 98.5%+
- Save corruption rate: 0 critical cases
- Review target: 7.5/10+ average tester score and Very Positive sentiment trend

### Launch Goals

- Deliver a stable, readable, replayable PC-first MVP that proves the narrative-progression loop.
- Validate Steam conversion and early retention before any platform expansion.
- Exit MVP with clear evidence on whether the core 4x4 Souvenir combat loop and divine-progression fantasy sustain repeat play.

---

## Next Steps

### Immediate Actions

1. Lock an MVP contract in one page (must-have, deferred list, and hard acceptance gates).
2. Set up Unity production foundation (URP 2D, scene flow, save architecture with versioning and backup).
3. Implement core loop skeleton with placeholders (hub -> run -> encounter -> death -> return -> spend -> rerun).
4. Build one full combat slice for the 4x4 Souvenir system (8 cards each side, placement effects, tie rules, and readability rules) with 3 enemy archetypes + 1 elite behavior + 1 guardian encounter.
5. Implement progression and narrative glue (Prometheus memory retention, small divine favor set, clue unlocks, run-start/run-end reactive lines).

### Research Needs

- Balance framework for the 4x4 Souvenir system (hero life values, card life ranges, and effect tuning).
- Enemy AI heuristics for card placement priority (kill, adjacency value, board control, tie-break awareness).
- Readability validation for stacked placement effects on a compact 4x4 board.
- Balance framework for divine favor build variation without combinatorial explosion.
- Save-data schema tests for progression integrity under interruption/failure cases.
- Steam store-page and trailer messaging tests for differentiation clarity.
- AI-generated image commercial-rights workflow and replacement fallback policy.

### Open Questions

- What is the final MVP Souvenir card count at launch (20-30 target range)?
- Which 5-7 placement effect families are included in MVP and which are explicitly deferred?
- Which 3-4 gods are included in the first divine favor set for launch?
- What is the minimum encounter variety needed to avoid repetition in the MVP slice?
- Which exact onboarding/tutorial format best teaches the 4x4 combat rules without slowing first-session pacing?
