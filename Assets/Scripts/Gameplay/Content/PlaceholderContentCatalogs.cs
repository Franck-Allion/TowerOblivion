using System.Collections.Generic;

using TowerOblivion.Gameplay.Narrative;
namespace TowerOblivion.Gameplay.Content
{
    public sealed class PlaceholderContentCatalogs
    {
        public IReadOnlyList<RoomContentDefinition> RoomDefinitions { get; }
        public IReadOnlyList<EncounterContentDefinition> EncounterDefinitions { get; }
        public IReadOnlyList<SouvenirContentDefinition> SouvenirDefinitions { get; }
        public IReadOnlyList<RewardContentDefinition> RewardDefinitions { get; }
        public IReadOnlyList<NarrativeEventContentDefinition> NarrativeEventDefinitions { get; }
        public IReadOnlyList<ModifierContentDefinition> ModifierDefinitions { get; }

        public IContentCatalog<RoomId, RoomContentDefinition> Rooms { get; }
        public IContentCatalog<EncounterId, EncounterContentDefinition> Encounters { get; }
        public IContentCatalog<SouvenirId, SouvenirContentDefinition> Souvenirs { get; }
        public IContentCatalog<RewardId, RewardContentDefinition> Rewards { get; }
        public IContentCatalog<NarrativeEventId, NarrativeEventContentDefinition> NarrativeEvents { get; }
        public IContentCatalog<ModifierId, ModifierContentDefinition> Modifiers { get; }

        public PlaceholderContentCatalogs(
            IReadOnlyList<RoomContentDefinition> roomDefinitions,
            IReadOnlyList<EncounterContentDefinition> encounterDefinitions,
            IReadOnlyList<SouvenirContentDefinition> souvenirDefinitions,
            IReadOnlyList<RewardContentDefinition> rewardDefinitions,
            IReadOnlyList<NarrativeEventContentDefinition> narrativeEventDefinitions,
            IReadOnlyList<ModifierContentDefinition> modifierDefinitions,
            IContentCatalog<RoomId, RoomContentDefinition> rooms,
            IContentCatalog<EncounterId, EncounterContentDefinition> encounters,
            IContentCatalog<SouvenirId, SouvenirContentDefinition> souvenirs,
            IContentCatalog<RewardId, RewardContentDefinition> rewards,
            IContentCatalog<NarrativeEventId, NarrativeEventContentDefinition> narrativeEvents,
            IContentCatalog<ModifierId, ModifierContentDefinition> modifiers)
        {
            RoomDefinitions = roomDefinitions ?? new List<RoomContentDefinition>();
            EncounterDefinitions = encounterDefinitions ?? new List<EncounterContentDefinition>();
            SouvenirDefinitions = souvenirDefinitions ?? new List<SouvenirContentDefinition>();
            RewardDefinitions = rewardDefinitions ?? new List<RewardContentDefinition>();
            NarrativeEventDefinitions = narrativeEventDefinitions ?? new List<NarrativeEventContentDefinition>();
            ModifierDefinitions = modifierDefinitions ?? new List<ModifierContentDefinition>();
            Rooms = rooms;
            Encounters = encounters;
            Souvenirs = souvenirs;
            Rewards = rewards;
            NarrativeEvents = narrativeEvents;
            Modifiers = modifiers;
        }
    }
}
