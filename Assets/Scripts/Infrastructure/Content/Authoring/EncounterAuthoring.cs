using UnityEngine;

namespace TowerOblivion.Infrastructure.Content.Authoring
{
    [CreateAssetMenu(fileName = "EncounterAuthoring", menuName = "TowerOblivion/Content/Encounter Authoring")]
    public sealed class EncounterAuthoring : ScriptableObject
    {
        [SerializeField] private string _id;
        public string Id => _id;

        [SerializeField] private string _displayNameKey;
        public string DisplayNameKey => _displayNameKey;

        [SerializeField] private string[] _souvenirIds = new string[0];
        public string[] SouvenirIds => _souvenirIds;

        [SerializeField] private string[] _rewardIds = new string[0];
        public string[] RewardIds => _rewardIds;

        [SerializeField] private string[] _modifierIds = new string[0];
        public string[] ModifierIds => _modifierIds;

        internal void SetData(string id, string displayNameKey, string[] souvenirIds, string[] rewardIds, string[] modifierIds)
        {
            _id = id;
            _displayNameKey = displayNameKey;
            _souvenirIds = souvenirIds;
            _rewardIds = rewardIds;
            _modifierIds = modifierIds;
        }
    }
}
