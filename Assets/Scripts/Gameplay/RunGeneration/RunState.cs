using System;
using System.Collections.Generic;
using TowerOblivion.Core;

namespace TowerOblivion.Gameplay.RunGeneration
{
    [Serializable]
    public class RunState
    {
        public RunId RunId { get; set; }
        public RunSeed Seed { get; set; }
        public ContentVersion ContentVersion { get; set; }
        public int CurrentFloorIndex { get; set; }
        public RoomId CurrentRoomId { get; set; }
        public List<RoomId> VisitedRoomIds { get; set; } = new List<RoomId>();
        public List<EncounterId> CompletedEncounterIds { get; set; } = new List<EncounterId>();
        public List<ModifierId> ActiveModifierIds { get; set; } = new List<ModifierId>();
        public List<RunModifierState> ActiveModifiers { get; set; } = new List<RunModifierState>();
        public List<RunResourceState> TransientResources { get; set; } = new List<RunResourceState>();
        public List<SouvenirId> RunSouvenirIds { get; set; } = new List<SouvenirId>();
        public int TemporaryResourceAmount { get; set; }
    }
}
