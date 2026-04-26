using System;
using TowerOblivion.Core;
using TowerOblivion.Gameplay;
using UnityEngine;

namespace TowerOblivion.Presentation.RoomView
{
    public sealed class RoomPresenter : MonoBehaviour
    {
        [SerializeField] private RoomView _view;

        public void InitializeView(RoomView view)
        {
            _view = view;
        }
        
        private IEventBus _eventBus;
        private IDisposable _subscription;

        public void Initialize(IEventBus eventBus)
        {
            _subscription?.Dispose();
            _eventBus = eventBus;
            _subscription = _eventBus.Subscribe<RoomLoaded>(OnRoomLoaded);
        }

        private void OnDestroy()
        {
            _subscription?.Dispose();
        }

        private void OnRoomLoaded(RoomLoaded evt)
        {
            if (_view != null)
            {
                // Placeholder for Localization: using DisplayNameKey directly for now
                _view.SetRoomName(evt.Room.DisplayNameKey);
            }
        }
    }
}
