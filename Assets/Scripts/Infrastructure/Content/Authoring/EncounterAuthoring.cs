using UnityEngine;

namespace TowerOblivion.Infrastructure.Content.Authoring
{
    [CreateAssetMenu(fileName = "EncounterAuthoring", menuName = "TowerOblivion/Content/Encounter Authoring")]
    public sealed class EncounterAuthoring : ScriptableObject
    {
        [SerializeField] internal string _id;
        public string Id => _id;

        [SerializeField] internal string _displayNameKey;
        public string DisplayNameKey => _displayNameKey;

        [SerializeField] internal string[] _souvenirIds = new string[0];
        public string[] SouvenirIds => _souvenirIds;

        [SerializeField] internal string[] _rewardIds = new string[0];
        public string[] RewardIds => _rewardIds;

        [SerializeField] internal string[] _modifierIds = new string[0];
        public string[] ModifierIds => _modifierIds;
    }
}
