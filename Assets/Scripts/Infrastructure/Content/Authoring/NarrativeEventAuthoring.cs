using UnityEngine;

namespace TowerOblivion.Infrastructure.Content.Authoring
{
    [CreateAssetMenu(fileName = "NarrativeEventAuthoring", menuName = "TowerOblivion/Content/Narrative Event Authoring")]
    public sealed class NarrativeEventAuthoring : ScriptableObject
    {
        [SerializeField] internal string _id;
        public string Id => _id;

        [SerializeField] internal string _displayNameKey;
        public string DisplayNameKey => _displayNameKey;

        [Header("Requirements")]
        [SerializeField] internal string _requiredFlagId;
        public string RequiredFlagId => _requiredFlagId;

        [SerializeField] internal bool _requiredFlagValue = true;
        public bool RequiredFlagValue => _requiredFlagValue;

        [SerializeField] internal string _requiredCounterId;
        public string RequiredCounterId => _requiredCounterId;

        [SerializeField] internal int _requiredCounterMinValue = 0;
        public int RequiredCounterMinValue => _requiredCounterMinValue;
    }
}
