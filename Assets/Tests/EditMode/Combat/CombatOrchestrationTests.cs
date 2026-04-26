using System;
using System.Collections.Generic;
using NUnit.Framework;
using TowerOblivion.Core;
using TowerOblivion.Gameplay.Combat;
using TowerOblivion.Gameplay.Content;
using TowerOblivion.Gameplay.RunGeneration;

namespace TowerOblivion.Tests.EditMode.Combat
{
    public sealed class CombatOrchestrationTests
    {
        private FakeEventBus _eventBus;
        private CombatStateOrchestrator _orchestrator;

        [SetUp]
        public void SetUp()
        {
            _eventBus = new FakeEventBus();
            _orchestrator = new CombatStateOrchestrator(_eventBus);
        }

        [Test]
        public void StartCombat_ValidInputs_InitializesCorrectly()
        {
            var encounter = CreateTestEncounter("encounter.test");
            var runState = new RunState();
            runState.RunSouvenirIds.Add(new SouvenirId("souvenir.hero.1"));
            var combatSeed = new RunSeed(42);

            var combatState = _orchestrator.StartCombat(encounter, runState, combatSeed);

            Assert.That(combatState.EncounterId, Is.EqualTo(encounter.Id));
            Assert.That(combatState.CombatSeed, Is.EqualTo(combatSeed));
            Assert.That(combatState.HeroLife, Is.EqualTo(20));
            Assert.That(combatState.EnemyLife, Is.EqualTo(20));
            
            Assert.That(combatState.HeroHand, Has.Count.EqualTo(1));
            Assert.That(combatState.HeroHand[0].Value, Is.EqualTo("souvenir.hero.1"));
            
            Assert.That(combatState.EnemyHand, Has.Count.EqualTo(1));
            Assert.That(combatState.EnemyHand[0].Value, Is.EqualTo("souvenir.enemy.1"));

            Assert.That(_eventBus.PublishedEvents, Has.Count.EqualTo(1));
            Assert.That(_eventBus.PublishedEvents[0], Is.InstanceOf<CombatStarted>());
        }

        [Test]
        public void StartCombat_IsDeterministic_SameSeedProducesSameIds()
        {
            var encounter = CreateTestEncounter("encounter.repro");
            var runState = new RunState();
            var combatSeed = new RunSeed(12345);

            var state1 = _orchestrator.StartCombat(encounter, runState, combatSeed);
            var state2 = _orchestrator.StartCombat(encounter, runState, combatSeed);

            Assert.That(state1.CombatId, Is.EqualTo(state2.CombatId));
            Assert.That(state1.CombatId.Value, Does.Contain("12345"));
        }

        private static EncounterContentDefinition CreateTestEncounter(string id)
        {
            return new EncounterContentDefinition(
                new EncounterId(id),
                "Test Encounter",
                new List<SouvenirId> { new SouvenirId("souvenir.enemy.1") },
                new List<RewardId>(),
                new List<ModifierId>()
            );
        }

        private sealed class FakeEventBus : IEventBus
        {
            public List<IGameEvent> PublishedEvents { get; } = new List<IGameEvent>();

            public void Publish<TEvent>(TEvent evt) where TEvent : IGameEvent
            {
                PublishedEvents.Add(evt);
            }

            public IDisposable Subscribe<TEvent>(Action<TEvent> handler) where TEvent : IGameEvent
            {
                return new NoOpDisposable();
            }

            private sealed class NoOpDisposable : IDisposable
            {
                public void Dispose() { }
            }
        }
    }
}
