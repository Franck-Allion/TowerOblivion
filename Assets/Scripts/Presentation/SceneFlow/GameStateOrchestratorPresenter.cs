using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using TowerOblivion.Core;
using TowerOblivion.Gameplay;

namespace TowerOblivion.Presentation.SceneFlow
{
    /// <summary>
    /// Coordinates Unity scene and UI transitions based on game mode changes.
    /// Acts as the bridge between Domain state and Unity presentation.
    /// </summary>
    public sealed class GameStateOrchestratorPresenter : MonoBehaviour
    {
        private IDisposable _subscription;
        private GameModeId? _pendingMode;

        public void Bind(IEventBus eventBus)
        {
            _subscription?.Dispose();
            _subscription = eventBus.Subscribe<GameModeChanged>(OnGameModeChanged);
        }

        private void OnDestroy()
        {
            _subscription?.Dispose();
        }

        private void OnGameModeChanged(GameModeChanged evt)
        {
            var sceneName = MapModeToScene(evt.CurrentMode);
            if (string.IsNullOrEmpty(sceneName)) return;

            // Prevent desync: if already transitioning, we might need to queue or handle it
            // For now, we allow the fader to handle its own guard, but we log if we are forcing a jump
            if (SceneFader.Instance != null && SceneFader.Instance.IsTransitioning)
            {
                Debug.LogWarning($"[GameStateOrchestratorPresenter] Transition to {evt.CurrentMode} requested while already transitioning. Presentation may lag behind Domain state.");
            }

            LoadScene(sceneName);
        }

        private string MapModeToScene(GameModeId mode)
        {
            if (mode == GameModeId.RebirthHub) return "Hub";
            if (mode == GameModeId.Exploration) return "Rooms";
            if (mode == GameModeId.Combat) return "Combat";
            if (mode == GameModeId.Reward) return "Reward";
            if (mode == GameModeId.Resolution) return "Resolution";
            
            return null;
        }

        private void LoadScene(string sceneName)
        {
            if (SceneFader.Instance != null)
            {
                SceneFader.Instance.TransitionToScene(sceneName);
            }
            else
            {
                SceneManager.LoadScene(sceneName);
            }
        }
    }
}
