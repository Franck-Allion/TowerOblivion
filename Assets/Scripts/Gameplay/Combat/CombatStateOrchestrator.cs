using System;
using System.Collections.Generic;
using TowerOblivion.Core;
using TowerOblivion.Gameplay.Content;
using TowerOblivion.Gameplay.Progression;
using TowerOblivion.Gameplay.RunGeneration;

namespace TowerOblivion.Gameplay.Combat
{
    public sealed class CombatStateOrchestrator
    {
        private readonly IEventBus _eventBus;
        private const int DEFAULT_LIFE = 20;

        public CombatStateOrchestrator(IEventBus eventBus)
        {
            _eventBus = eventBus ?? throw new ArgumentNullException(nameof(eventBus));
        }

        public CombatState StartCombat(EncounterContentDefinition encounter, RunState runState, RunSeed combatSeed, PlayerProfileState profile = null)
        {
            if (encounter == null) throw new ArgumentNullException(nameof(encounter));
            if (runState == null) throw new ArgumentNullException(nameof(runState));

            // Deterministic ID generation using the seed
            var combatId = new CombatId($"combat.{encounter.Id.Value}.{combatSeed.Value}");

            // Initial life from profile if available, otherwise default
            // Note: In MVP, life upgrades might be tracked in MetaState/Profile upgrade tiers.
            // For now, we look for a baseline or default to 20.
            int heroLife = DEFAULT_LIFE;
            
            var combatState = new CombatState
            {
                CombatId = combatId,
                EncounterId = encounter.Id,
                CombatSeed = combatSeed,
                TurnIndex = 0,
                HeroLife = heroLife,
                EnemyLife = DEFAULT_LIFE,
                IsResolved = false,
                HeroHand = new List<SouvenirId>(runState.RunSouvenirIds),
                EnemyHand = new List<SouvenirId>(encounter.SouvenirIds),
                ActiveActorId = new ActorId("actor.hero")
            };

            _eventBus.Publish(new CombatStarted(combatState));

            return combatState;
        }
    }
}
