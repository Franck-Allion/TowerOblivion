using System;
using System.Collections.Generic;
using TowerOblivion.Core;

using TowerOblivion.Gameplay.Content;
namespace TowerOblivion.Gameplay.Combat
{
    [Serializable]
    public class CombatState
    {
        public CombatId CombatId { get; set; }
        public EncounterId EncounterId { get; set; }
        public RunSeed CombatSeed { get; set; }
        public ActorId ActiveActorId { get; set; }
        public int TurnIndex { get; set; }
        public int HeroLife { get; set; }
        public int EnemyLife { get; set; }
        public bool IsResolved { get; set; }
        public List<CombatCardState> BoardCards { get; set; } = new List<CombatCardState>();
        public List<SouvenirId> HeroHand { get; set; } = new List<SouvenirId>();
        public List<SouvenirId> EnemyHand { get; set; } = new List<SouvenirId>();
    }
}
