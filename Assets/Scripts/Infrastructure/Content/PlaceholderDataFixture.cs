using UnityEngine;
using TowerOblivion.Infrastructure.Content.Authoring;
using TowerOblivion.Infrastructure.Content.Conversion;

namespace TowerOblivion.Infrastructure.Content
{
    /// <summary>
    /// Provides an in-memory hardcoded data set for the Epic 2 playable slice.
    /// This fixture is replaceable by serialized authoring assets later.
    /// </summary>
    public static class PlaceholderDataFixture
    {
        public static PlaceholderContentAuthoringSet Create()
        {
            var room = ScriptableObject.CreateInstance<RoomAuthoring>();
            room.SetData("room.entry", "Exploration.Room.Entry", new[] { "encounter.entry" }, new[] { "narrative.prometheus_whisper" });

            var encounter = ScriptableObject.CreateInstance<EncounterAuthoring>();
            encounter.SetData("encounter.entry", "Combat.Encounter.Entry", new[] { "souvenir.broken_laurel" }, new[] { "reward.memory_embers" }, new string[0]);

            var souvenir = ScriptableObject.CreateInstance<SouvenirAuthoring>();
            souvenir.SetData("souvenir.broken_laurel", "Reward.Souvenir.BrokenLaurel", 5);

            var reward = ScriptableObject.CreateInstance<RewardAuthoring>();
            reward.SetData("reward.memory_embers", "Reward.Currency.MemoryEmbers", "currency.memory_embers", 10);

            var narrativeEvent = ScriptableObject.CreateInstance<NarrativeEventAuthoring>();
            narrativeEvent.SetData("narrative.prometheus_whisper", "Narrative.Event.PrometheusWhisper", "flag.prometheus_contacted", true, "counter.none", 0);

            var authoringSet = new PlaceholderContentAuthoringSet
            {
                _rooms = new[] { room },
                _encounters = new[] { encounter },
                _souvenirs = new[] { souvenir },
                _rewards = new[] { reward },
                _narrativeEvents = new[] { narrativeEvent },
                _modifiers = new ModifierAuthoring[0]
            };

            return authoringSet;
        }
    }
}
