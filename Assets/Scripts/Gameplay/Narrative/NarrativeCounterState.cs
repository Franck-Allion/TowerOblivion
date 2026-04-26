using System;
using TowerOblivion.Core;

namespace TowerOblivion.Gameplay.Narrative
{
    [Serializable]
    public sealed class NarrativeCounterState
    {
        public NarrativeFlagId CounterId { get; set; }
        public int Value { get; set; }
    }
}
