using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System;

namespace TowerOblivion.Presentation.Reward
{
    public sealed class RewardPlaceholderView : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _rewardTitle;
        [SerializeField] private TextMeshProUGUI _rewardEffect;
        [SerializeField] private Button _continueButton;

        public event Action ContinueClicked;

        private void Awake()
        {
            if (_continueButton != null)
            {
                _continueButton.onClick.AddListener(() => ContinueClicked?.Invoke());
            }
        }

        public void SetRewardInfo(string title, string effect)
        {
            if (_rewardTitle != null) _rewardTitle.text = title;
            if (_rewardEffect != null) _rewardEffect.text = effect;
        }

        public void Initialize(Button continueButton)
        {
            _continueButton = continueButton;
            if (_continueButton != null)
            {
                _continueButton.onClick.RemoveAllListeners();
                _continueButton.onClick.AddListener(() => ContinueClicked?.Invoke());
            }
        }
    }
}
