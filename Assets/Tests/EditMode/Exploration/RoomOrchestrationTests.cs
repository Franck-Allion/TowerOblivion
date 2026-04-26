using System;
using System.Collections.Generic;
using NUnit.Framework;
using TowerOblivion.Core;
using TowerOblivion.Gameplay;
using TowerOblivion.Gameplay.Content;
using TowerOblivion.Gameplay.RunGeneration;

namespace TowerOblivion.Tests.EditMode.Exploration
{
    public sealed class RoomOrchestrationTests
    {
        private RunState _runState;
        private FakeEventBus _eventBus;
        private RoomStateOrchestrator _orchestrator;
        private FakeContentCatalog<RoomId, RoomContentDefinition> _roomCatalog;

        [SetUp]
        public void SetUp()
        {
            _runState = new RunState();
            _eventBus = new FakeEventBus();
            _roomCatalog = new FakeContentCatalog<RoomId, RoomContentDefinition>();
            _orchestrator = new RoomStateOrchestrator(_runState, _eventBus, _roomCatalog);
        }

        [Test]
        public void LoadRoom_ExistingRoom_UpdatesStateAndEmitsEvent()
        {
            var roomId = new RoomId("room.test");
            var roomDef = new RoomContentDefinition(roomId, "Test Room", new List<EncounterId>(), new List<NarrativeEventId>());
            _roomCatalog.Add(roomId, roomDef);

            var result = _orchestrator.LoadRoom(roomId);

            Assert.That(result.IsSuccess, Is.True);
            Assert.That(_runState.ActiveRoomId, Is.EqualTo(roomId));
            Assert.That(_eventBus.PublishedEvents, Has.Count.EqualTo(1));
            Assert.That(_eventBus.PublishedEvents[0], Is.InstanceOf<RoomLoaded>());
            Assert.That(((RoomLoaded)_eventBus.PublishedEvents[0]).Room, Is.EqualTo(roomDef));
        }

        [Test]
        public void LoadRoom_MissingRoom_ReturnsFailure()
        {
            var roomId = new RoomId("room.missing");

            var result = _orchestrator.LoadRoom(roomId);

            Assert.That(result.IsSuccess, Is.False);
            Assert.That(result.ErrorCode, Is.EqualTo("exploration.room_not_found"));
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

        private sealed class FakeContentCatalog<TId, TDto> : IContentCatalog<TId, TDto>
        {
            private readonly Dictionary<TId, TDto> _items = new Dictionary<TId, TDto>();

            public void Add(TId id, TDto dto) => _items[id] = dto;

            public bool TryGet(TId id, out TDto dto)
            {
                return _items.TryGetValue(id, out dto);
            }

            public IReadOnlyList<TDto> GetAll()
            {
                return new List<TDto>(_items.Values);
            }
        }
    }
}
