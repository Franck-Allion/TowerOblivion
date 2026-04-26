using System;
using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using TMPro;
using TowerOblivion.Core;
using TowerOblivion.Gameplay;
using TowerOblivion.Gameplay.Content;
using TowerOblivion.Presentation.RoomView;
using UnityEngine;
using UnityEngine.TestTools;

namespace TowerOblivion.Tests.PlayMode
{
    public sealed class RoomViewTests
    {
        [UnityTest]
        public IEnumerator RoomPresenter_UpdatesViewOnRoomLoaded()
        {
            // Setup
            var go = new GameObject("RoomViewTest");
            var view = go.AddComponent<RoomView>();
            var presenter = go.AddComponent<RoomPresenter>();
            var textLabel = go.AddComponent<TextMeshProUGUI>();
            
            view.Initialize(textLabel);
            presenter.InitializeView(view);

            var eventBus = new FakeEventBus();
            presenter.Initialize(eventBus);

            // Act
            var roomId = new RoomId("room.test");
            var roomDef = new RoomContentDefinition(roomId, "Test Room Display Name", new List<EncounterId>(), new List<NarrativeEventId>());
            eventBus.Publish(new RoomLoaded(roomDef));

            yield return null; // Wait a frame

            // Assert
            Assert.That(textLabel.text, Is.EqualTo("Test Room Display Name"));

            // Cleanup
            UnityEngine.Object.DestroyImmediate(go);
        }

        private sealed class FakeEventBus : IEventBus
        {
            private readonly List<Action<object>> _handlers = new List<Action<object>>();

            public void Publish<TEvent>(TEvent evt) where TEvent : IGameEvent
            {
                foreach (var handler in _handlers)
                {
                    if (handler.Target is Action<TEvent> typedHandler)
                    {
                         // This is tricky with manual mock and generics
                    }
                    handler(evt);
                }
            }

            public IDisposable Subscribe<TEvent>(Action<TEvent> handler) where TEvent : IGameEvent
            {
                Action<object> wrapper = (obj) =>
                {
                    if (obj is TEvent typedEvt)
                    {
                        handler(typedEvt);
                    }
                };
                _handlers.Add(wrapper);
                return new Unsubscriber(() => _handlers.Remove(wrapper));
            }

            private class Unsubscriber : IDisposable
            {
                private readonly Action _unsubscribe;
                public Unsubscriber(Action unsubscribe) => _unsubscribe = unsubscribe;
                public void Dispose() => _unsubscribe();
            }
        }
    }
}
