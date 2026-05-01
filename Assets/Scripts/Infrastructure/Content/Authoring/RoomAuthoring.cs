using UnityEngine;

namespace TowerOblivion.Infrastructure.Content.Authoring
{
    [CreateAssetMenu(fileName = "RoomAuthoring", menuName = "TowerOblivion/Content/Room Authoring")]
    public sealed class RoomAuthoring : ScriptableObject
    {
        [SerializeField] private string _id;
        public string Id => _id;

        [SerializeField] private string _displayNameKey;
        public string DisplayNameKey => _displayNameKey;

        [SerializeField] private string[] _encounterIds = new string[0];
        public string[] EncounterIds => _encounterIds;

        [SerializeField] private string[] _narrativeEventIds = new string[0];
        public string[] NarrativeEventIds => _narrativeEventIds;

        // Internal setter for fixture creation in testing/bootstrap
        internal void SetData(string id, string displayNameKey, string[] encounterIds, string[] narrativeEventIds)
        {
            _id = id;
            _displayNameKey = displayNameKey;
            _encounterIds = encounterIds;
            _narrativeEventIds = narrativeEventIds;
        }
    }
}
