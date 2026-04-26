using System.Collections.Generic;
using TowerOblivion.Core;

namespace TowerOblivion.Gameplay.Content
{
    public sealed class RoomContentDefinition
    {
        public RoomId Id { get; }
        public string DisplayNameKey { get; }
        public IReadOnlyList<EncounterId> EncounterIds { get; }
        public IReadOnlyList<NarrativeEventId> NarrativeEventIds { get; }

        public RoomContentDefinition(
            RoomId id,
            string displayNameKey,
            IReadOnlyList<EncounterId> encounterIds,
            IReadOnlyList<NarrativeEventId> narrativeEventIds)
        {
            Id = id;
            DisplayNameKey = displayNameKey ?? string.Empty;
            EncounterIds = encounterIds ?? new List<EncounterId>();
            NarrativeEventIds = narrativeEventIds ?? new List<NarrativeEventId>();
        }
    }
}
