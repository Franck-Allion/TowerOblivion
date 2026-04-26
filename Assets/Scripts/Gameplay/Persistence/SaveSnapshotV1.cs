using System;
using TowerOblivion.Gameplay.Narrative;
using TowerOblivion.Gameplay.Progression;
using TowerOblivion.Gameplay.RunGeneration;

namespace TowerOblivion.Gameplay.Persistence
{
    [Serializable]
    public class SaveSnapshotV1
    {
        public SaveMetadata Metadata { get; set; } = new SaveMetadata();
        public PlayerProfileState PlayerProfile { get; set; } = new PlayerProfileState();
        public NarrativeState Narrative { get; set; } = new NarrativeState();
        public RunState ActiveRun { get; set; }
    }
}
