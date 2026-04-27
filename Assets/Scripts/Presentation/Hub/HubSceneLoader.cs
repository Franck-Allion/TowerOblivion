using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using TowerOblivion.Core;
using TowerOblivion.Gameplay;

namespace TowerOblivion.Presentation.Hub
{
    /// <summary>
    /// Presentation-layer component that handles Unity scene transitions 
    /// based on domain events.
    /// </summary>
    public sealed class HubSceneLoader : MonoBehaviour
    {
        private IDisposable _subscription;

        public void Bind(IEventBus eventBus)
        {
            _subscription?.Dispose();
            
            // Listen for the FACT that a run has successfully started to trigger the actual Unity load
            _subscription = eventBus.Subscribe<RunStarted>(OnRunStarted);
        }

        private void OnDestroy()
        {
            _subscription?.Dispose();
        }

        private void OnRunStarted(RunStarted evt)
        {
            Debug.Log($"[HubSceneLoader] Run started with ID {evt.InitialState.RunId}. Loading Room 1...");
            
            // For MVP, we load "Rooms" as the placeholder for Room 1.
            // Story 2.1 AC 6: "Start Run" triggers transition to the first exploration room (Room 1).
            SceneManager.LoadScene("Rooms"); 
        }
    }
}
