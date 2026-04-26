using System;
using TowerOblivion.Core;

namespace TowerOblivion.Gameplay.Combat
{
    [Serializable]
    public sealed class CombatCardState
    {
        public CardInstanceId InstanceId { get; set; }
        public SouvenirId SouvenirId { get; set; }
        public ActorId OwnerId { get; set; }
        public int CurrentLife { get; set; }
        public BoardCell Cell { get; set; }
        public bool IsDestroyed { get; set; }
    }
}
