using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System;

namespace TowerOblivion.Presentation.Resolution
{
    public sealed class ResolutionPlaceholderView : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _outcomeTitle;
        [SerializeField] private TextMeshProUGUI _progressionInfo;
        [SerializeField] private Button _returnToHubButton;

        public event Action ReturnToHubClicked;

        private void Awake()
        {
            if (_returnToHubButton != null)
            {
                _returnToHubButton.onClick.AddListener(() => ReturnToHubClicked?.Invoke());
            }
        }

        public void SetResolutionInfo(string title, string progression)
        {
            if (_outcomeTitle != null) _outcomeTitle.text = title;
            if (_progressionInfo != null) _progressionInfo.text = progression;
        }

        public void Initialize(Button returnButton)
        {
            _returnToHubButton = returnButton;
            if (_returnToHubButton != null)
            {
                _returnToHubButton.onClick.RemoveAllListeners();
                _returnToHubButton.onClick.AddListener(() => ReturnToHubClicked?.Invoke());
            }
        }
    }
}
