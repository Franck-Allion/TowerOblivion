using System.Collections.Generic;

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

        public IContentCatalog<TowerOblivion.Core.RoomId, RoomContentDefinition> Rooms { get; }
        public IContentCatalog<TowerOblivion.Core.EncounterId, EncounterContentDefinition> Encounters { get; }
        public IContentCatalog<TowerOblivion.Core.SouvenirId, SouvenirContentDefinition> Souvenirs { get; }
        public IContentCatalog<TowerOblivion.Core.RewardId, RewardContentDefinition> Rewards { get; }
        public IContentCatalog<TowerOblivion.Core.NarrativeEventId, NarrativeEventContentDefinition> NarrativeEvents { get; }
        public IContentCatalog<TowerOblivion.Core.ModifierId, ModifierContentDefinition> Modifiers { get; }

        public PlaceholderContentCatalogs(
            IReadOnlyList<RoomContentDefinition> roomDefinitions,
            IReadOnlyList<EncounterContentDefinition> encounterDefinitions,
            IReadOnlyList<SouvenirContentDefinition> souvenirDefinitions,
            IReadOnlyList<RewardContentDefinition> rewardDefinitions,
            IReadOnlyList<NarrativeEventContentDefinition> narrativeEventDefinitions,
            IReadOnlyList<ModifierContentDefinition> modifierDefinitions,
            IContentCatalog<TowerOblivion.Core.RoomId, RoomContentDefinition> rooms,
            IContentCatalog<TowerOblivion.Core.EncounterId, EncounterContentDefinition> encounters,
            IContentCatalog<TowerOblivion.Core.SouvenirId, SouvenirContentDefinition> souvenirs,
            IContentCatalog<TowerOblivion.Core.RewardId, RewardContentDefinition> rewards,
            IContentCatalog<TowerOblivion.Core.NarrativeEventId, NarrativeEventContentDefinition> narrativeEvents,
            IContentCatalog<TowerOblivion.Core.ModifierId, ModifierContentDefinition> modifiers)
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
