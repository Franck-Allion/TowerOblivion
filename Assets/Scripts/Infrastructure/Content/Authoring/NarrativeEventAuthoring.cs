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

        [SerializeField] internal string _requiredFlagId;
        public string RequiredFlagId => _requiredFlagId;
    }
}
