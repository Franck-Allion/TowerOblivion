using System;

namespace TowerOblivion.Core
{
    public interface IEventBus
    {
        void Publish<TEvent>(TEvent evt) where TEvent : IGameEvent;
        IDisposable Subscribe<TEvent>(Action<TEvent> handler) where TEvent : IGameEvent;
    }
}
