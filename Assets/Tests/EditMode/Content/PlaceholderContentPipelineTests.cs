using System;
using NUnit.Framework;
using TowerOblivion.Core;
using TowerOblivion.Gameplay.Content;
using TowerOblivion.Gameplay.Narrative;
using TowerOblivion.Infrastructure.Content;
using TowerOblivion.Infrastructure.Content.Authoring;
using TowerOblivion.Infrastructure.Content.Conversion;
using TowerOblivion.Infrastructure.Content.Validation;
using UnityEngine;

using TowerOblivion.Gameplay.Progression;
namespace TowerOblivion.Tests.EditMode.Content
{
    public sealed class PlaceholderContentPipelineTests
    {
        [Test]
        public void Convert_ValidAuthoringData_CreatesDeterministicCatalogLookups()
        {
            var authoring = CreateValidAuthoringSet();
            var converter = new PlaceholderContentConverter();
            var catalogs = converter.Convert(authoring);

            var roomFound = catalogs.Rooms.TryGet(new RoomId("room.entry"), out var room);
            var encounterFound = catalogs.Encounters.TryGet(new EncounterId("encounter.entry"), out var encounter);
            var rewardFound = catalogs.Rewards.TryGet(new RewardId("reward.memory_embers"), out var reward);

            Assert.That(roomFound, Is.True);
            Assert.That(encounterFound, Is.True);
            Assert.That(rewardFound, Is.True);
            Assert.That(room.EncounterIds.Count, Is.EqualTo(1));
            Assert.That(encounter.RewardIds.Count, Is.EqualTo(1));
            Assert.That(reward.CurrencyId.Value, Is.EqualTo("currency.memory_embers"));

            Cleanup(authoring);
        }

        [Test]
        public void Convert_DuplicateIds_ThrowsArgumentException()
        {
            var authoring = CreateValidAuthoringSet();
            authoring._souvenirs = new[]
            {
                CreateSouvenir("souvenir.broken_laurel", "souvenir.broken_laurel"),
                CreateSouvenir("souvenir.broken_laurel", "souvenir.duplicate")
            };

            var converter = new PlaceholderContentConverter();

            Assert.Throws<ArgumentException>(() => converter.Convert(authoring));

            Cleanup(authoring);
        }

        [Test]
        public void Validate_MissingReferences_ReturnsFailure()
        {
            var authoring = CreateValidAuthoringSet();
            authoring.Encounters[0]._rewardIds = new[] { "reward.missing" };

            var converter = new PlaceholderContentConverter();
            var catalogs = converter.Convert(authoring);
            var validator = new PlaceholderContentValidator();

            var result = validator.Validate(catalogs);

            Assert.That(result.IsFailure, Is.True);
            Assert.That(result.ErrorCode, Is.EqualTo("content.missing_reference"));

            Cleanup(authoring);
        }

        [Test]
        public void CatalogLookup_NotFound_ReturnsFalse()
        {
            var authoring = CreateValidAuthoringSet();
            var converter = new PlaceholderContentConverter();
            var catalogs = converter.Convert(authoring);

            var found = catalogs.NarrativeEvents.TryGet(new NarrativeEventId("narrative.unknown"), out var narrativeEventDefinition);

            Assert.That(found, Is.False);
            Assert.That(narrativeEventDefinition, Is.Null);

            Cleanup(authoring);
        }

        private static PlaceholderContentAuthoringSet CreateValidAuthoringSet()
        {
            return new PlaceholderContentAuthoringSet
            {
                _rooms = new[] { CreateRoom("room.entry") },
                _encounters = new[] { CreateEncounter("encounter.entry") },
                _souvenirs = new[] { CreateSouvenir("souvenir.broken_laurel", "souvenir.broken_laurel") },
                _rewards = new[] { CreateReward("reward.memory_embers") },
                _narrativeEvents = new[] { CreateNarrativeEvent("narrative.prometheus_whisper", "flag.prometheus_contacted") },
                _modifiers = new[] { CreateModifier("modifier.divine_glow") }
            };
        }

        private static RoomAuthoring CreateRoom(string id)
        {
            var room = ScriptableObject.CreateInstance<RoomAuthoring>();
            room._id = id;
            room._displayNameKey = id;
            room._encounterIds = new[] { "encounter.entry" };
            room._narrativeEventIds = new[] { "narrative.prometheus_whisper" };
            return room;
        }

        private static EncounterAuthoring CreateEncounter(string id)
        {
            var encounter = ScriptableObject.CreateInstance<EncounterAuthoring>();
            encounter._id = id;
            encounter._displayNameKey = id;
            encounter._rewardIds = new[] { "reward.memory_embers" };
            encounter._modifierIds = new[] { "modifier.divine_glow" };
            encounter._souvenirIds = new[] { "souvenir.broken_laurel" };
            return encounter;
        }

        private static SouvenirAuthoring CreateSouvenir(string id, string displayKey)
        {
            var souvenir = ScriptableObject.CreateInstance<SouvenirAuthoring>();
            souvenir._id = id;
            souvenir._displayNameKey = displayKey;
            souvenir._baseLife = 4;
            return souvenir;
        }

        private static RewardAuthoring CreateReward(string id)
        {
            var reward = ScriptableObject.CreateInstance<RewardAuthoring>();
            reward._id = id;
            reward._displayNameKey = id;
            reward._currencyId = "currency.memory_embers";
            reward._amount = 10;
            return reward;
        }

        private static NarrativeEventAuthoring CreateNarrativeEvent(string id, string flagId)
        {
            var narrative = ScriptableObject.CreateInstance<NarrativeEventAuthoring>();
            narrative._id = id;
            narrative._displayNameKey = id;
            narrative._requiredFlagId = flagId;
            return narrative;
        }

        private static ModifierAuthoring CreateModifier(string id)
        {
            var modifier = ScriptableObject.CreateInstance<ModifierAuthoring>();
            modifier._id = id;
            modifier._displayNameKey = id;
            modifier._magnitude = 1;
            return modifier;
        }

        private static void Cleanup(PlaceholderContentAuthoringSet authoring)
        {
            DestroyCollection(authoring.Rooms);
            DestroyCollection(authoring.Encounters);
            DestroyCollection(authoring.Souvenirs);
            DestroyCollection(authoring.Rewards);
            DestroyCollection(authoring.NarrativeEvents);
            DestroyCollection(authoring.Modifiers);
        }

        private static void DestroyCollection<T>(T[] values) where T : UnityEngine.Object
        {
            if (values == null)
            {
                return;
            }

            for (var i = 0; i < values.Length; i++)
            {
                if (values[i] != null)
                {
                    UnityEngine.Object.DestroyImmediate(values[i]);
                }
            }
        }
    }
}
