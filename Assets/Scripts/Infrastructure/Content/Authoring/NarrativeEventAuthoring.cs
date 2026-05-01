using UnityEngine;

namespace TowerOblivion.Infrastructure.Content.Authoring
{
    [CreateAssetMenu(fileName = "NarrativeEventAuthoring", menuName = "TowerOblivion/Content/Narrative Event Authoring")]
    public sealed class NarrativeEventAuthoring : ScriptableObject
    {
        [SerializeField] private string _id;
        public string Id => _id;

        [SerializeField] private string _displayNameKey;
        public string DisplayNameKey => _displayNameKey;

        [SerializeField] private string _requiredFlagId;
        public string RequiredFlagId => _requiredFlagId;

        [SerializeField] private bool _requiredFlagValue = true;
        public bool RequiredFlagValue => _requiredFlagValue;

        [SerializeField] private string _requiredCounterId;
        public string RequiredCounterId => _requiredCounterId;

        [SerializeField] private int _requiredCounterMinValue = 0;
        public int RequiredCounterMinValue => _requiredCounterMinValue;

        internal void SetData(string id, string displayNameKey, string flagId, bool flagVal, string counterId, int counterMin)
        {
            _id = id;
            _displayNameKey = displayNameKey;
            _requiredFlagId = flagId;
            _requiredFlagValue = flagVal;
            _requiredCounterId = counterId;
            _requiredCounterMinValue = counterMin;
        }
    }
}
