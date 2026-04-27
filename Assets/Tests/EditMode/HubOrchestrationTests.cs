using System;
using System.Collections.Generic;
using NUnit.Framework;
using TowerOblivion.Core;
using TowerOblivion.Gameplay;
using TowerOblivion.Gameplay.Content;
using TowerOblivion.Gameplay.RunGeneration;

namespace TowerOblivion.Tests.EditMode
{
    public sealed class HubOrchestrationTests
    {
        private FakeEventBus _eventBus;
        private ContentVersion _version;
        private HubStateOrchestrator _orchestrator;

        [SetUp]
        public void SetUp()
        {
            _eventBus = new FakeEventBus();
            _version = new ContentVersion("content.1.2.3");
            _orchestrator = new HubStateOrchestrator(_eventBus, _version);
        }

        [Test]
        public void EnterHub_PublishesHubEnteredEvent()
        {
            _orchestrator.EnterHub();

            Assert.That(_eventBus.PublishedEvents, Has.Count.EqualTo(1));
            Assert.That(_eventBus.PublishedEvents[0], Is.InstanceOf<HubEntered>());
        }

        [Test]
        public void CreateNewRun_InitializesRunStateAndPublishesFactEvent()
        {
            var runId = new RunId("run.test");
            var seed = new RunSeed(42);

            var runState = _orchestrator.CreateNewRun(runId, seed);

            Assert.That(runState.RunId, Is.EqualTo(runId));
            Assert.That(runState.Seed, Is.EqualTo(seed));
            Assert.That(runState.ContentVersion, Is.EqualTo(_version));
            Assert.That(runState.ActiveRoomId.Value, Is.EqualTo("room.1"));

            Assert.That(_eventBus.PublishedEvents, Has.Count.EqualTo(1));
            Assert.That(_eventBus.PublishedEvents[0], Is.InstanceOf<RunStarted>());
            var startedEvent = (RunStarted)_eventBus.PublishedEvents[0];
            Assert.That(startedEvent.InitialState.RunId, Is.EqualTo(runId));
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
