using System;
using System.Collections.Generic;
using NUnit.Framework;
using TowerOblivion.Core;
using TowerOblivion.Gameplay;

namespace TowerOblivion.Tests.EditMode
{
    public sealed class GameStateOrchestrationTests
    {
        private FakeEventBus _eventBus;
        private GameStateOrchestrator _orchestrator;

        [SetUp]
        public void SetUp()
        {
            _eventBus = new FakeEventBus();
            _orchestrator = new GameStateOrchestrator(_eventBus);
        }

        [Test]
        public void InitialMode_IsRebirthHub()
        {
            Assert.That(_orchestrator.CurrentMode, Is.EqualTo(GameModeId.RebirthHub));
        }

        [Test]
        public void LegalTransition_SucceedsUpdatesModeAndPublishesEvent()
        {
            var result = _orchestrator.TransitionTo(GameModeId.Exploration);

            Assert.That(result.IsSuccess, Is.True);
            Assert.That(_orchestrator.CurrentMode, Is.EqualTo(GameModeId.Exploration));
            Assert.That(_eventBus.PublishedEvents, Has.Count.EqualTo(1));
            
            var evt = _eventBus.PublishedEvents[0] as GameModeChanged;
            Assert.That(evt, Is.Not.Null);
            Assert.That(evt.PreviousMode, Is.EqualTo(GameModeId.RebirthHub));
            Assert.That(evt.CurrentMode, Is.EqualTo(GameModeId.Exploration));
        }

        [Test]
        public void FullLoop_Succeeds()
        {
            Assert.That(_orchestrator.TransitionTo(GameModeId.Exploration).IsSuccess, Is.True);
            Assert.That(_orchestrator.TransitionTo(GameModeId.Combat).IsSuccess, Is.True);
            Assert.That(_orchestrator.TransitionTo(GameModeId.Reward).IsSuccess, Is.True);
            Assert.That(_orchestrator.TransitionTo(GameModeId.Resolution).IsSuccess, Is.True);
            Assert.That(_orchestrator.TransitionTo(GameModeId.RebirthHub).IsSuccess, Is.True);
            
            Assert.That(_orchestrator.CurrentMode, Is.EqualTo(GameModeId.RebirthHub));
            Assert.That(_eventBus.PublishedEvents, Has.Count.EqualTo(5));
        }

        [Test]
        public void IllegalTransition_FailsLeavesModeUnchangedAndPublishesNoEvent()
        {
            // Initial is RebirthHub. Trying to skip to Combat.
            var result = _orchestrator.TransitionTo(GameModeId.Combat);

            Assert.That(result.IsFailure, Is.True);
            Assert.That(result.ErrorCode, Is.EqualTo("mode.invalid_transition"));
            Assert.That(_orchestrator.CurrentMode, Is.EqualTo(GameModeId.RebirthHub));
            Assert.That(_eventBus.PublishedEvents, Is.Empty);
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
