using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using TowerOblivion.Core;
using TowerOblivion.Gameplay;

namespace TowerOblivion.Presentation.Hub
{
    public sealed class HubSceneLoader : MonoBehaviour
    {
        private IDisposable _subscription;

        public void Bind(IEventBus eventBus)
        {
            _subscription?.Dispose();
            _subscription = eventBus.Subscribe<RunStarted>(OnRunStarted);
        }

        private void OnDestroy()
        {
            _subscription?.Dispose();
        }

        private void OnRunStarted(RunStarted evt)
        {
            if (SceneFader.Instance == null)
            {
                SceneManager.LoadScene("Rooms");
                return;
            }

            SceneFader.Instance.TransitionToScene("Rooms");
        }
    }
}
