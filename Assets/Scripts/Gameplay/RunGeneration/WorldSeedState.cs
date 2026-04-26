using System;
using TowerOblivion.Core;

namespace TowerOblivion.Gameplay.RunGeneration
{
    [Serializable]
    public class WorldSeedState
    {
        public RunSeed RunSeed { get; set; }
        public ContentVersion ContentVersion { get; set; }
        public int GeneratorVersion { get; set; }
        public int EncounterSeedOffset { get; set; }
        public int RewardSeedOffset { get; set; }
        public int NarrativeSeedOffset { get; set; }
    }
}
