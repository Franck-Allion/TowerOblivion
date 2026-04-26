using System.Collections.Generic;
using TowerOblivion.Core;
using TowerOblivion.Gameplay.Combat;
using TowerOblivion.Gameplay.Content;
using TowerOblivion.Gameplay.Narrative;
using TowerOblivion.Gameplay.Progression;
using TowerOblivion.Infrastructure.Content.Authoring;

namespace TowerOblivion.Infrastructure.Content.Conversion
{
    public sealed class PlaceholderContentConverter
    {
        public PlaceholderContentCatalogs Convert(PlaceholderContentAuthoringSet authoringSet)
        {
            if (authoringSet == null)
            {
                authoringSet = new PlaceholderContentAuthoringSet();
            }

            var rooms = ConvertRooms(authoringSet.Rooms);
            var encounters = ConvertEncounters(authoringSet.Encounters);
            var souvenirs = ConvertSouvenirs(authoringSet.Souvenirs);
            var rewards = ConvertRewards(authoringSet.Rewards);
            var narrativeEvents = ConvertNarrativeEvents(authoringSet.NarrativeEvents);
            var modifiers = ConvertModifiers(authoringSet.Modifiers);

            return new PlaceholderContentCatalogs(
                rooms,
                encounters,
                souvenirs,
                rewards,
                narrativeEvents,
                modifiers,
                new InMemoryContentCatalog<RoomId, RoomContentDefinition>(rooms, definition => definition.Id),
                new InMemoryContentCatalog<EncounterId, EncounterContentDefinition>(encounters, definition => definition.Id),
                new InMemoryContentCatalog<SouvenirId, SouvenirContentDefinition>(souvenirs, definition => definition.Id),
                new InMemoryContentCatalog<RewardId, RewardContentDefinition>(rewards, definition => definition.Id),
                new InMemoryContentCatalog<NarrativeEventId, NarrativeEventContentDefinition>(narrativeEvents, definition => definition.Id),
                new InMemoryContentCatalog<ModifierId, ModifierContentDefinition>(modifiers, definition => definition.Id));
        }

        private static List<RoomContentDefinition> ConvertRooms(RoomAuthoring[] authoring)
        {
            var definitions = new List<RoomContentDefinition>();
            if (authoring == null)
            {
                return definitions;
            }

            for (var i = 0; i < authoring.Length; i++)
            {
                var source = authoring[i];
                if (source == null)
                {
                    continue;
                }

                var encounterIds = new List<EncounterId>();
                var narrativeEventIds = new List<NarrativeEventId>();
                AddIds(source.EncounterIds, encounterIds, value => new EncounterId(value));
                AddIds(source.NarrativeEventIds, narrativeEventIds, value => new NarrativeEventId(value));

                definitions.Add(new RoomContentDefinition(
                    new RoomId(source.Id),
                    source.DisplayNameKey,
                    encounterIds,
                    narrativeEventIds));
            }

            return definitions;
        }

        private static List<EncounterContentDefinition> ConvertEncounters(EncounterAuthoring[] authoring)
        {
            var definitions = new List<EncounterContentDefinition>();
            if (authoring == null)
            {
                return definitions;
            }

            for (var i = 0; i < authoring.Length; i++)
            {
                var source = authoring[i];
                if (source == null)
                {
                    continue;
                }

                var souvenirIds = new List<SouvenirId>();
                var rewardIds = new List<RewardId>();
                var modifierIds = new List<ModifierId>();
                AddIds(source.SouvenirIds, souvenirIds, value => new SouvenirId(value));
                AddIds(source.RewardIds, rewardIds, value => new RewardId(value));
                AddIds(source.ModifierIds, modifierIds, value => new ModifierId(value));

                definitions.Add(new EncounterContentDefinition(
                    new EncounterId(source.Id),
                    source.DisplayNameKey,
                    souvenirIds,
                    rewardIds,
                    modifierIds));
            }

            return definitions;
        }

        private static List<SouvenirContentDefinition> ConvertSouvenirs(SouvenirAuthoring[] authoring)
        {
            var definitions = new List<SouvenirContentDefinition>();
            if (authoring == null)
            {
                return definitions;
            }

            for (var i = 0; i < authoring.Length; i++)
            {
                var source = authoring[i];
                if (source == null)
                {
                    continue;
                }

                definitions.Add(new SouvenirContentDefinition(
                    new SouvenirId(source.Id),
                    source.DisplayNameKey,
                    source.BaseLife));
            }

            return definitions;
        }

        private static List<RewardContentDefinition> ConvertRewards(RewardAuthoring[] authoring)
        {
            var definitions = new List<RewardContentDefinition>();
            if (authoring == null)
            {
                return definitions;
            }

            for (var i = 0; i < authoring.Length; i++)
            {
                var source = authoring[i];
                if (source == null)
                {
                    continue;
                }

                definitions.Add(new RewardContentDefinition(
                    new RewardId(source.Id),
                    source.DisplayNameKey,
                    new CurrencyId(source.CurrencyId),
                    source.Amount));
            }

            return definitions;
        }

        private static List<NarrativeEventContentDefinition> ConvertNarrativeEvents(NarrativeEventAuthoring[] authoring)
        {
            var definitions = new List<NarrativeEventContentDefinition>();
            if (authoring == null)
            {
                return definitions;
            }

            for (var i = 0; i < authoring.Length; i++)
            {
                var source = authoring[i];
                if (source == null)
                {
                    continue;
                }

                definitions.Add(new NarrativeEventContentDefinition(
                    new NarrativeEventId(source.Id),
                    source.DisplayNameKey,
                    new NarrativeFlagId(source.RequiredFlagId),
                    source.RequiredFlagValue,
                    new NarrativeFlagId(source.RequiredCounterId),
                    source.RequiredCounterMinValue));
            }

            return definitions;
        }

        private static List<ModifierContentDefinition> ConvertModifiers(ModifierAuthoring[] authoring)
        {
            var definitions = new List<ModifierContentDefinition>();
            if (authoring == null)
            {
                return definitions;
            }

            for (var i = 0; i < authoring.Length; i++)
            {
                var source = authoring[i];
                if (source == null)
                {
                    continue;
                }

                definitions.Add(new ModifierContentDefinition(
                    new ModifierId(source.Id),
                    source.DisplayNameKey,
                    source.Magnitude));
            }

            return definitions;
        }

        private static void AddIds<TId>(string[] values, List<TId> destination, System.Func<string, TId> factory)
        {
            if (values == null || destination == null || factory == null)
            {
                return;
            }

            for (var i = 0; i < values.Length; i++)
            {
                var val = values[i];
                if (string.IsNullOrEmpty(val))
                {
                    continue;
                }

                destination.Add(factory(val));
            }
        }
    }
}
