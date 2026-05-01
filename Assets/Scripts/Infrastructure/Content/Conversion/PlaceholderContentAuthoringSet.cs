using UnityEngine;
using TowerOblivion.Infrastructure.Content.Authoring;

namespace TowerOblivion.Infrastructure.Content.Conversion
{
    [CreateAssetMenu(fileName = "ContentAuthoringSet", menuName = "TowerOblivion/Content/Authoring Set")]
    public sealed class PlaceholderContentAuthoringSet : ScriptableObject
    {
        [SerializeField] internal RoomAuthoring[] _rooms = new RoomAuthoring[0];
        public RoomAuthoring[] Rooms => _rooms;

        [SerializeField] internal EncounterAuthoring[] _encounters = new EncounterAuthoring[0];
        public EncounterAuthoring[] Encounters => _encounters;

        [SerializeField] internal SouvenirAuthoring[] _souvenirs = new SouvenirAuthoring[0];
        public SouvenirAuthoring[] Souvenirs => _souvenirs;

        [SerializeField] internal RewardAuthoring[] _rewards = new RewardAuthoring[0];
        public RewardAuthoring[] Rewards => _rewards;

        [SerializeField] internal NarrativeEventAuthoring[] _narrativeEvents = new NarrativeEventAuthoring[0];
        public NarrativeEventAuthoring[] NarrativeEvents => _narrativeEvents;

        [SerializeField] internal ModifierAuthoring[] _modifiers = new ModifierAuthoring[0];
        public ModifierAuthoring[] Modifiers => _modifiers;

        // Internal setter for fixture/testing
        internal void SetData(
            RoomAuthoring[] rooms,
            EncounterAuthoring[] encounters,
            SouvenirAuthoring[] souvenirs,
            RewardAuthoring[] rewards,
            NarrativeEventAuthoring[] narrativeEvents,
            ModifierAuthoring[] modifiers)
        {
            _rooms = rooms;
            _encounters = encounters;
            _souvenirs = souvenirs;
            _rewards = rewards;
            _narrativeEvents = narrativeEvents;
            _modifiers = modifiers;
        }
    }
}
