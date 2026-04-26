using System;
using TowerOblivion.Core;

namespace TowerOblivion.Gameplay.Progression
{
    [Serializable]
    public class UpgradeTierState
    {
        public UpgradeTrackId UpgradeTrackId { get; set; }
        public int Tier { get; set; }
    }
}
