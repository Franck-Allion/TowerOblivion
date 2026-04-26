using System;
using System.Collections.Generic;
using TowerOblivion.Core;
using TowerOblivion.Gameplay.Content;
using TowerOblivion.Gameplay.Narrative;

namespace TowerOblivion.Gameplay.Progression
{
    [Serializable]
    public class PlayerProfileState
    {
        public ContentVersion ContentVersion { get; set; }
        public SaveVersion SaveVersion { get; set; }
        public SettingsProfileId SettingsProfileId { get; set; }
        public List<SouvenirId> UnlockedSouvenirIds { get; set; } = new List<SouvenirId>();
        public List<UpgradeTierState> UpgradeTiers { get; set; } = new List<UpgradeTierState>();
        public List<CurrencyAmountState> Currencies { get; set; } = new List<CurrencyAmountState>();
        public List<MemoryId> UnlockedMemoryIds { get; set; } = new List<MemoryId>();
        public List<ChapterMilestoneId> ChapterMilestoneIds { get; set; } = new List<ChapterMilestoneId>();
    }
}
