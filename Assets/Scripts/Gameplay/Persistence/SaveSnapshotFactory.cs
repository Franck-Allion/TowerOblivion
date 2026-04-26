using System;
using TowerOblivion.Core;
using TowerOblivion.Gameplay.Narrative;
using TowerOblivion.Gameplay.Progression;

namespace TowerOblivion.Gameplay.Persistence
{
    public static class SaveSnapshotFactory
    {
        public static SaveSnapshotV1 CreateDefault(ContentVersion contentVersion, SaveVersion saveVersion, DateTime savedAtUtc)
        {
            return new SaveSnapshotV1
            {
                Metadata = new SaveMetadata
                {
                    SaveVersion = saveVersion,
                    ContentVersion = contentVersion,
                    SavedAtUtc = savedAtUtc
                },
                PlayerProfile = new PlayerProfileState
                {
                    ContentVersion = contentVersion,
                    SaveVersion = saveVersion
                },
                Narrative = new NarrativeState
                {
                    ContentVersion = contentVersion
                }
            };
        }
    }
}
