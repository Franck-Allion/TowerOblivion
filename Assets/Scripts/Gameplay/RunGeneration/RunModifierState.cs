using System;
using TowerOblivion.Core;

namespace TowerOblivion.Gameplay.RunGeneration
{
    [Serializable]
    public class RunModifierState
    {
        public ModifierId ModifierId { get; set; }
        public int StackCount { get; set; }
        public int RemainingRooms { get; set; }
    }
}
