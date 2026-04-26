using System;
using System.Collections.Generic;
using TowerOblivion.Core;

namespace TowerOblivion.Gameplay.Narrative
{
    [Serializable]
    public class NarrativeState
    {
        public ContentVersion ContentVersion { get; set; }
        public List<NarrativeFlagState> Flags { get; set; } = new List<NarrativeFlagState>();
        public List<NarrativeFlagState> Consequences { get; set; } = new List<NarrativeFlagState>();
        public List<ClueId> DiscoveredClueIds { get; set; } = new List<ClueId>();
        public List<MemoryId> UnlockedMemoryIds { get; set; } = new List<MemoryId>();
    }
}
