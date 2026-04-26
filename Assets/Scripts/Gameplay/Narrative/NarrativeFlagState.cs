using System;
using TowerOblivion.Core;

namespace TowerOblivion.Gameplay.Narrative
{
    [Serializable]
    public class NarrativeFlagState
    {
        public NarrativeFlagId FlagId { get; set; }
        public bool Value { get; set; }
    }
}
