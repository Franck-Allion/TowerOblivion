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
    public class StateDomainSerializationTests
    {
        [Test]
        public void RunState_ConstructsAndRoundtripsWithoutUnityObjects()
        {
            var state = new RunState
            {
                RunId = new RunId("run.001"),
                Seed = new RunSeed(12345),
                ContentVersion = new ContentVersion("content.0.1.0"),
                CurrentFloorIndex = 2,
                ActiveRoomId = new RoomId("room.floor_02.entry"),
                TemporaryResourceAmount = 3
            };
            state.VisitedRoomIds.Add(new RoomId("room.floor_01.entry"));
            state.CompletedEncounterIds.Add(new EncounterId("encounter.guardian_intro"));
            state.ActiveModifiers.Add(new RunModifierState
            {
                ModifierId = new ModifierId("modifier.ashen_pressure"),
                StackCount = 1,
                RemainingRooms = 2
            });
            state.RunSouvenirIds.Add(new SouvenirId("souvenir.broken_laurel"));

            var roundtripped = RoundTrip(state);

            Assert.That(roundtripped.RunId, Is.EqualTo(state.RunId));
            Assert.That(roundtripped.ActiveRoomId, Is.EqualTo(state.ActiveRoomId));
            Assert.That(roundtripped.ActiveModifiers[0].ModifierId, Is.EqualTo(state.ActiveModifiers[0].ModifierId));
            Assert.That(roundtripped.RunSouvenirIds[0], Is.EqualTo(state.RunSouvenirIds[0]));
        }

        [Test]
        public void CombatState_ConstructsAndRoundtripsWithoutCombatRules()
        {
            var state = new CombatState
            {
                CombatId = new CombatId("combat.001"),
                EncounterId = new EncounterId("encounter.floor_01_sentinel"),
                CombatSeed = new RunSeed(9001),
                ActiveActorId = new ActorId("actor.hero"),
                TurnIndex = 4,
                HeroLife = 18,
                EnemyLife = 11,
                IsResolved = false
            };
            state.HeroHand.Add(new SouvenirId("souvenir.first_victory_spear"));
            state.EnemyHand.Add(new SouvenirId("souvenir.sentinel_oath"));
            state.BoardCards.Add(new CombatCardState
            {
                InstanceId = new CardInstanceId("card.001"),
                OwnerId = new ActorId("actor.hero"),
                SouvenirId = new SouvenirId("souvenir.first_victory_spear"),
                Cell = new BoardCell(1, 2),
                CurrentLife = 3,
                IsDestroyed = false
            });

            var roundtripped = RoundTrip(state);

            Assert.That(roundtripped.CombatId, Is.EqualTo(state.CombatId));
            Assert.That(roundtripped.ActiveActorId, Is.EqualTo(state.ActiveActorId));
            Assert.That(roundtripped.BoardCards[0].Cell, Is.EqualTo(new BoardCell(1, 2)));
            Assert.That(roundtripped.BoardCards[0].SouvenirId, Is.EqualTo(state.BoardCards[0].SouvenirId));
        }

        [Test]
        public void PlayerProfileState_ConstructsAndRoundtripsPersistentProgressionShape()
        {
            var state = new PlayerProfileState
            {
                ContentVersion = new ContentVersion("content.0.1.0"),
                SaveVersion = new SaveVersion(1)
            };
            state.UnlockedSouvenirIds.Add(new SouvenirId("souvenir.broken_laurel"));
            state.UpgradeTiers.Add(new UpgradeTierState
            {
                UpgradeTrackId = new UpgradeTrackId("upgrade.thief"),
                Tier = 2
            });
            state.Currencies.Add(new CurrencyAmountState
            {
                CurrencyId = new CurrencyId("currency.memory_embers"),
                Amount = 5
            });
            state.UnlockedMemoryIds.Add(new MemoryId("memory.prometheus_ember"));
            state.ChapterMilestoneIds.Add(new ChapterMilestoneId("chapter.condemned"));

            var roundtripped = RoundTrip(state);

            Assert.That(roundtripped.SaveVersion, Is.EqualTo(state.SaveVersion));
            Assert.That(roundtripped.UnlockedSouvenirIds[0], Is.EqualTo(state.UnlockedSouvenirIds[0]));
            Assert.That(roundtripped.UpgradeTiers[0].UpgradeTrackId, Is.EqualTo(state.UpgradeTiers[0].UpgradeTrackId));
            Assert.That(roundtripped.Currencies[0].Amount, Is.EqualTo(5));
            Assert.That(roundtripped.ChapterMilestoneIds[0], Is.EqualTo(state.ChapterMilestoneIds[0]));
        }

        [Test]
        public void NarrativeState_ConstructsAndRoundtripsFlagsCluesAndConsequences()
        {
            var state = new NarrativeState
            {
                ContentVersion = new ContentVersion("content.0.1.0")
            };
            state.Flags.Add(new NarrativeFlagState
            {
                FlagId = new NarrativeFlagId("flag.prometheus_contacted"),
                Value = true,
                Counter = 1
            });
            state.Consequences.Add(new NarrativeFlagState
            {
                FlagId = new NarrativeFlagId("consequence.spared_guardian"),
                Value = true,
                Counter = 0
            });
            state.DiscoveredClueIds.Add(new ClueId("clue.burned_oath"));
            state.UnlockedMemoryIds.Add(new MemoryId("memory.first_death"));

            var roundtripped = RoundTrip(state);

            Assert.That(roundtripped.Flags[0].FlagId, Is.EqualTo(state.Flags[0].FlagId));
            Assert.That(roundtripped.Consequences[0].FlagId, Is.EqualTo(state.Consequences[0].FlagId));
            Assert.That(roundtripped.DiscoveredClueIds[0], Is.EqualTo(state.DiscoveredClueIds[0]));
            Assert.That(roundtripped.UnlockedMemoryIds[0], Is.EqualTo(state.UnlockedMemoryIds[0]));
        }

        [Test]
        public void WorldSeedState_ConstructsAndRoundtripsGeneratorInputs()
        {
            var state = new WorldSeedState
            {
                RunSeed = new RunSeed(777),
                ContentVersion = new ContentVersion("content.0.1.0"),
                GeneratorVersion = 1,
                EncounterSeedOffset = 10,
                RewardSeedOffset = 20,
                NarrativeSeedOffset = 30
            };

            var roundtripped = RoundTrip(state);

            Assert.That(roundtripped.RunSeed, Is.EqualTo(state.RunSeed));
            Assert.That(roundtripped.ContentVersion, Is.EqualTo(state.ContentVersion));
            Assert.That(roundtripped.GeneratorVersion, Is.EqualTo(1));
            Assert.That(roundtripped.NarrativeSeedOffset, Is.EqualTo(30));
        }

        [Test]
        public void StateModels_DoNotExposeDerivedOrComputedAuthoritativeFields()
        {
            var stateTypes = new[]
            {
                typeof(RunState),
                typeof(CombatState),
                typeof(PlayerProfileState),
                typeof(NarrativeState),
                typeof(WorldSeedState)
            };

            var suspiciousProperties = stateTypes
                .SelectMany(type => type.GetProperties(BindingFlags.Instance | BindingFlags.Public)
                    .Where(property => property.Name.Contains("Derived") || property.Name.Contains("Computed"))
                    .Select(property => $"{type.Name}.{property.Name}"))
                .ToArray();

            Assert.That(suspiciousProperties, Is.Empty);
        }

        private static T RoundTrip<T>(T state)
        {
            var json = JsonSerializationHelper.ToJson(state);
            return JsonSerializationHelper.FromJson<T>(json);
        }
    }
}
