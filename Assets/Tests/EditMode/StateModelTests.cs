using System;
using System.Linq;
using System.Reflection;
using NUnit.Framework;
using TowerOblivion.Core;
using TowerOblivion.Gameplay.Combat;
using TowerOblivion.Gameplay.Narrative;
using TowerOblivion.Gameplay.Progression;
using TowerOblivion.Gameplay.RunGeneration;
using TowerOblivion.Infrastructure.Persistence;

namespace TowerOblivion.Tests.EditMode
{
    public sealed class StateModelTests
    {
        [Test]
        public void StateModelsCanBeConstructedWithTypedIdsAndSeeds()
        {
            var run = new RunState
            {
                RunId = new RunId("run.test"),
                Seed = new RunSeed(42),
                CurrentFloorIndex = 1,
                CurrentRoomId = new RoomId("room.floor_01.entry")
            };
            run.ActiveModifierIds.Add(new ModifierId("modifier.elite_trial"));
            run.TransientResources.Add(new RunResourceState
            {
                ResourceId = new CurrencyId("currency.temp_embers"),
                Amount = 3
            });

            var combat = new CombatState
            {
                CombatId = new CombatId("combat.test"),
                EncounterId = new EncounterId("encounter.guardian_seed"),
                ActiveActorId = new ActorId("actor.hero"),
                HeroLife = 12,
                EnemyLife = 10
            };
            combat.BoardCards.Add(new CombatCardState
            {
                InstanceId = new CardInstanceId("card.001"),
                OwnerId = new ActorId("actor.hero"),
                SouvenirId = new SouvenirId("souvenir.broken_laurel"),
                CurrentLife = 4,
                Cell = new BoardCell(1, 2)
            });

            var profile = new PlayerProfileState
            {
                SettingsProfileId = new SettingsProfileId("settings.default")
            };
            profile.UnlockedSouvenirIds.Add(new SouvenirId("souvenir.broken_laurel"));
            profile.UpgradeTiers.Add(new UpgradeTierState
            {
                UpgradeTrackId = new UpgradeTrackId("upgrade.psyche"),
                Tier = 2
            });

            var narrative = new NarrativeState();
            narrative.Flags.Add(new NarrativeFlagState
            {
                FlagId = new NarrativeFlagId("flag.prometheus_whisper_heard"),
                Value = true
            });
            narrative.DiscoveredClueIds.Add(new ClueId("clue.first_chains"));

            var worldSeed = new WorldSeedState
            {
                RunSeed = new RunSeed(42),
                ContentVersion = new ContentVersion("content.test"),
                GeneratorVersion = 1
            };

            Assert.That(run.RunId.Value, Is.EqualTo("run.test"));
            Assert.That(combat.BoardCards[0].Cell, Is.EqualTo(new BoardCell(1, 2)));
            Assert.That(profile.UpgradeTiers[0].Tier, Is.EqualTo(2));
            Assert.That(narrative.Flags[0].Value, Is.True);
            Assert.That(worldSeed.ContentVersion.Value, Is.EqualTo("content.test"));
        }

        [Test]
        public void StateModelsRoundTripThroughInMemorySerialization()
        {
            var original = new RunState
            {
                RunId = new RunId("run.roundtrip"),
                Seed = new RunSeed(1234),
                CurrentFloorIndex = 2,
                CurrentRoomId = new RoomId("room.floor_02.shrine")
            };
            original.ActiveModifierIds.Add(new ModifierId("modifier.dark_omen"));

            var copy = RoundTrip(original);

            Assert.That(copy.RunId, Is.EqualTo(original.RunId));
            Assert.That(copy.Seed, Is.EqualTo(original.Seed));
            Assert.That(copy.CurrentFloorIndex, Is.EqualTo(original.CurrentFloorIndex));
            Assert.That(copy.CurrentRoomId, Is.EqualTo(original.CurrentRoomId));
            Assert.That(copy.ActiveModifierIds.Single(), Is.EqualTo(original.ActiveModifierIds.Single()));
        }

        [Test]
        public void GameplayAssemblyDoesNotReferenceUnityEngine()
        {
            Assembly gameplayAssembly = typeof(RunState).Assembly;
            string[] references = gameplayAssembly
                .GetReferencedAssemblies()
                .Select(name => name.Name)
                .ToArray();

            Assert.That(references, Does.Not.Contain("UnityEngine"));
            Assert.That(references, Does.Not.Contain("UnityEngine.CoreModule"));
        }

        [Test]
        public void StateModelsDoNotExposeDerivedCombatStatsAsAuthoritativeState()
        {
            PropertyInfo[] profileProperties = typeof(PlayerProfileState).GetProperties();

            Assert.That(profileProperties.Select(property => property.Name), Does.Not.Contain("DerivedStrength"));
            Assert.That(profileProperties.Select(property => property.Name), Does.Not.Contain("DerivedPsyche"));
            Assert.That(profileProperties.Select(property => property.Name), Does.Not.Contain("DerivedMana"));
            Assert.That(profileProperties.Select(property => property.Name), Does.Not.Contain("DerivedLife"));
        }

        private static T RoundTrip<T>(T value)
        {
            return JsonSerializationHelper.FromJson<T>(JsonSerializationHelper.ToJson(value));
        }
    }
}
