using UnityEngine;
using TowerOblivion.Gameplay;

namespace TowerOblivion.Presentation
{
    /// <summary>
    /// Helper component to trigger mode transitions from Unity events (e.g. Button onClick).
    /// Used for placeholder navigation in the vertical slice.
    /// </summary>
    public sealed class DebugModeTransitionTrigger : MonoBehaviour
    {
        [SerializeField] private string _targetModeValue;

        public void TriggerTransition()
        {
            if (GlobalBootstrapper.Instance == null)
            {
                Debug.LogError("[DebugModeTransitionTrigger] GlobalBootstrapper instance not found.");
                return;
            }

            var targetId = new GameModeId(_targetModeValue);
            var result = GlobalBootstrapper.Instance.GameStateOrchestrator.TransitionTo(targetId);
            
            if (result.IsFailure)
            {
                Debug.LogWarning($"[DebugModeTransitionTrigger] Transition to {targetId} failed: {result.ErrorMessage}");
            }
        }

        // Helper for inspector wiring
        public void SetTargetMode(string modeValue)
        {
            _targetModeValue = modeValue;
        }
    }
}
