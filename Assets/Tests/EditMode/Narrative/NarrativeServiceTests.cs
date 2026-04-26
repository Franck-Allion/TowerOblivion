using System;
using System.Collections.Generic;
using NUnit.Framework;
using TowerOblivion.Core;
using TowerOblivion.Gameplay.Content;
using TowerOblivion.Gameplay.Narrative;

namespace TowerOblivion.Tests.EditMode.Narrative
{
    public sealed class NarrativeServiceTests
    {
        private NarrativeState _state;
        private FakeEventBus _eventBus;
        private NarrativeService _service;

        [SetUp]
        public void SetUp()
        {
            _state = new NarrativeState();
            _eventBus = new FakeEventBus();
            _service = new NarrativeService(_state, _eventBus);
        }

        [Test]
        public void GetFlag_NewFlag_ReturnsFalse()
        {
            var flagId = new NarrativeFlagId("flag.test");
            Assert.That(_service.GetFlag(flagId), Is.False);
        }

        [Test]
        public void SetFlag_True_PersistsAndEmitsEvent()
        {
            var flagId = new NarrativeFlagId("flag.test");
            
            _service.SetFlag(flagId, true);

            Assert.That(_service.GetFlag(flagId), Is.True);
            Assert.That(_state.Flags.Count, Is.EqualTo(1));
            Assert.That(_state.Flags[0].FlagId, Is.EqualTo(flagId));
            Assert.That(_state.Flags[0].Value, Is.True);
            
            Assert.That(_eventBus.PublishedEvents.Count, Is.EqualTo(1));
            Assert.That(_eventBus.PublishedEvents[0], Is.InstanceOf<NarrativeFlagChanged>());
            var evt = (NarrativeFlagChanged)_eventBus.PublishedEvents[0];
            Assert.That(evt.FlagId, Is.EqualTo(flagId));
            Assert.That(evt.NewValue, Is.True);
        }

        [Test]
        public void ToggleFlag_FalseToTrue()
        {
            var flagId = new NarrativeFlagId("flag.test");
            
            _service.ToggleFlag(flagId);

            Assert.That(_service.GetFlag(flagId), Is.True);
        }

        [Test]
        public void GetCounter_NewCounter_ReturnsZero()
        {
            var counterId = new NarrativeFlagId("counter.test");
            Assert.That(_service.GetCounter(counterId), Is.EqualTo(0));
        }

        [Test]
        public void AdjustCounter_IncrementsValueAndEmitsEvent()
        {
            var counterId = new NarrativeFlagId("counter.test");
            
            _service.AdjustCounter(counterId, 5);

            Assert.That(_service.GetCounter(counterId), Is.EqualTo(5));
            Assert.That(_state.Counters.Count, Is.EqualTo(1));
            
            Assert.That(_eventBus.PublishedEvents.Count, Is.EqualTo(1));
            Assert.That(_eventBus.PublishedEvents[0], Is.InstanceOf<NarrativeCounterChanged>());
            var evt = (NarrativeCounterChanged)_eventBus.PublishedEvents[0];
            Assert.That(evt.CounterId, Is.EqualTo(counterId));
            Assert.That(evt.NewValue, Is.EqualTo(5));
        }

        [Test]
        public void AdjustCounter_ClampsAtIntMax()
        {
            var counterId = new NarrativeFlagId("counter.limit");
            _service.SetCounter(counterId, int.MaxValue - 10);
            
            _service.AdjustCounter(counterId, 20);

            Assert.That(_service.GetCounter(counterId), Is.EqualTo(int.MaxValue));
        }

        [Test]
        public void GetConsequence_PersistsSeparately()
        {
            var flagId = new NarrativeFlagId("consequence.test");
            _service.SetConsequence(flagId, true);

            Assert.That(_service.GetConsequence(flagId), Is.True);
            Assert.That(_service.GetFlag(flagId), Is.False); // Should be separate list
        }

        [Test]
        public void Orchestrator_CanTrigger_RespectsBooleanFlags()
        {
            var flagId = new NarrativeFlagId("flag.required");
            var eventDef = new NarrativeEventContentDefinition(new NarrativeEventId("event.test"), "Test Event", flagId);
            var orchestrator = new NarrativeEventOrchestrator(_service);

            // Initially false
            Assert.That(orchestrator.CanTrigger(eventDef), Is.False);

            // Set true
            _service.SetFlag(flagId, true);
            Assert.That(orchestrator.CanTrigger(eventDef), Is.True);
        }

        [Test]
        public void Orchestrator_CanTrigger_RespectsMustBeFalse()
        {
            var flagId = new NarrativeFlagId("flag.forbidden");
            var eventDef = new NarrativeEventContentDefinition(
                new NarrativeEventId("event.test"), 
                "Test Event", 
                flagId, 
                requiredFlagValue: false);
            
            var orchestrator = new NarrativeEventOrchestrator(_service);

            // Initially true (flag is false by default)
            Assert.That(orchestrator.CanTrigger(eventDef), Is.True);

            // Set true -> now blocked
            _service.SetFlag(flagId, true);
            Assert.That(orchestrator.CanTrigger(eventDef), Is.False);
        }

        [Test]
        public void Orchestrator_CanTrigger_RespectsCounterMinimum()
        {
            var counterId = new NarrativeFlagId("counter.xp");
            var eventDef = new NarrativeEventContentDefinition(
                new NarrativeEventId("event.test"), 
                "Test Event", 
                requiredFlagId: default,
                requiredCounterId: counterId,
                requiredCounterMinValue: 10);
            
            var orchestrator = new NarrativeEventOrchestrator(_service);

            // Initially false (zero < 10)
            Assert.That(orchestrator.CanTrigger(eventDef), Is.False);

            // Set 5 -> still false
            _service.SetCounter(counterId, 5);
            Assert.That(orchestrator.CanTrigger(eventDef), Is.False);

            // Set 10 -> true
            _service.SetCounter(counterId, 10);
            Assert.That(orchestrator.CanTrigger(eventDef), Is.True);
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
                return null;
            }
        }
    }
}
