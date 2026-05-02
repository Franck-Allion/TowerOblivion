using UnityEngine;
using TowerOblivion.Gameplay;
using TowerOblivion.Gameplay.Content;

namespace TowerOblivion.Presentation.Exploration
{
    public sealed class ExplorationBootstrapper : MonoBehaviour
    {
        private RoomStateOrchestrator _roomOrchestrator;
        
        private void Start()
        {
            var global = GlobalBootstrapper.Instance;
            if (global == null || global.ActiveRun == null)
            {
                Debug.LogError("[ExplorationBootstrapper] Cannot start exploration: No active run state found.");      
                return;
            }

            if (global.ContentService == null || global.ContentService.Catalogs == null)
            {
                Debug.LogError("[ExplorationBootstrapper] ContentService or Catalogs missing.");
                return;
            }

            var presenter = GetComponent<TowerOblivion.Presentation.RoomView.RoomPresenter>();
            if (presenter != null)
            {
                presenter.Initialize(global.EventBus);
            }

            _roomOrchestrator = new RoomStateOrchestrator(global.ActiveRun, global.EventBus, global.ContentService.Catalogs.Rooms);
            var result = _roomOrchestrator.LoadRoom(global.ActiveRun.ActiveRoomId);

            if (result.IsFailure)
            {
                Debug.LogError($"[ExplorationBootstrapper] Failed to load room: {result.ErrorMessage}");
            }
        }
    }
}
