using UnityEngine;

namespace TowerOblivion.Infrastructure.Content.Authoring
{
    [CreateAssetMenu(fileName = "RoomAuthoring", menuName = "TowerOblivion/Content/Room Authoring")]
    public sealed class RoomAuthoring : ScriptableObject
    {
        [SerializeField] internal string _id;
        public string Id => _id;

        [SerializeField] internal string _displayNameKey;
        public string DisplayNameKey => _displayNameKey;

        [SerializeField] internal string[] _encounterIds = new string[0];
        public string[] EncounterIds => _encounterIds;

        [SerializeField] internal string[] _narrativeEventIds = new string[0];
        public string[] NarrativeEventIds => _narrativeEventIds;
    }
}
