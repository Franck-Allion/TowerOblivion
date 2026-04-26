using System;
using System.Collections.Generic;

namespace TowerOblivion.Core
{
    public sealed class SimpleEventBus : IEventBus
    {
        private readonly Dictionary<Type, List<Action<IGameEvent>>> _handlers = new Dictionary<Type, List<Action<IGameEvent>>>();

        public void Publish<TEvent>(TEvent evt) where TEvent : IGameEvent
        {
            var type = typeof(TEvent);
            if (_handlers.TryGetValue(type, out var handlers))
            {
                // Iterate backwards to allow unsubscription during dispatch without allocation
                for (var i = handlers.Count - 1; i >= 0; i--)
                {
                    handlers[i](evt);
                }
            }
        }

        public IDisposable Subscribe<TEvent>(Action<TEvent> handler) where TEvent : IGameEvent
        {
            var type = typeof(TEvent);
            if (!_handlers.ContainsKey(type))
            {
                _handlers[type] = new List<Action<IGameEvent>>();
            }

            Action<IGameEvent> wrapper = (evt) => handler((TEvent)evt);
            _handlers[type].Add(wrapper);

            return new Unsubscriber(() => _handlers[type].Remove(wrapper));
        }

        private sealed class Unsubscriber : IDisposable
        {
            private readonly Action _unsubscribe;
            public Unsubscriber(Action unsubscribe) => _unsubscribe = unsubscribe;
            public void Dispose() => _unsubscribe();
        }
    }
}
