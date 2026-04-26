using TowerOblivion.Infrastructure.Content.Authoring;

namespace TowerOblivion.Infrastructure.Content.Conversion
{
    public sealed class PlaceholderContentAuthoringSet
    {
        [UnityEngine.SerializeField] internal RoomAuthoring[] _rooms = new RoomAuthoring[0];
        public RoomAuthoring[] Rooms => _rooms;

        [UnityEngine.SerializeField] internal EncounterAuthoring[] _encounters = new EncounterAuthoring[0];
        public EncounterAuthoring[] Encounters => _encounters;

        [UnityEngine.SerializeField] internal SouvenirAuthoring[] _souvenirs = new SouvenirAuthoring[0];
        public SouvenirAuthoring[] Souvenirs => _souvenirs;

        [UnityEngine.SerializeField] internal RewardAuthoring[] _rewards = new RewardAuthoring[0];
        public RewardAuthoring[] Rewards => _rewards;

        [UnityEngine.SerializeField] internal NarrativeEventAuthoring[] _narrativeEvents = new NarrativeEventAuthoring[0];
        public NarrativeEventAuthoring[] NarrativeEvents => _narrativeEvents;

        [UnityEngine.SerializeField] internal ModifierAuthoring[] _modifiers = new ModifierAuthoring[0];
        public ModifierAuthoring[] Modifiers => _modifiers;
    }
}
