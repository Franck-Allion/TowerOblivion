using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System;

namespace TowerOblivion.Presentation.Hub
{
    public sealed class HubView : MonoBehaviour
    {
        [SerializeField] private Button _startRunButton;
        [SerializeField] private TextMeshProUGUI _progressionSummaryLabel;
        [SerializeField] private TextMeshProUGUI _saveStatusLabel;

        public event Action StartRunClicked;

        private void Awake()
        {
            if (_startRunButton != null)
            {
                _startRunButton.onClick.AddListener(() => StartRunClicked?.Invoke());
            }
        }

        public void SetProgressionSummary(string summary)
        {
            if (_progressionSummaryLabel != null)
            {
                _progressionSummaryLabel.text = summary;
            }
        }

        public void SetSaveStatus(string status)
        {
            if (_saveStatusLabel != null)
            {
                _saveStatusLabel.text = status;
            }
        }

        // For testing without reflection
        public void Initialize(Button startRunButton, TextMeshProUGUI progressionSummaryLabel, TextMeshProUGUI saveStatusLabel)
        {
            _startRunButton = startRunButton;
            _progressionSummaryLabel = progressionSummaryLabel;
            _saveStatusLabel = saveStatusLabel;
            
            // Re-bind if initialized after Awake
            if (_startRunButton != null)
            {
                _startRunButton.onClick.RemoveAllListeners();
                _startRunButton.onClick.AddListener(() => StartRunClicked?.Invoke());
            }
        }
    }
}
