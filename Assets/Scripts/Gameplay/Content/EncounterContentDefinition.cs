using System.Collections.Generic;
using TowerOblivion.Core;

namespace TowerOblivion.Gameplay.Content
{
    public sealed class EncounterContentDefinition
    {
        public EncounterId Id { get; }
        public string DisplayNameKey { get; }
        public IReadOnlyList<SouvenirId> SouvenirIds { get; }
        public IReadOnlyList<RewardId> RewardIds { get; }
        public IReadOnlyList<ModifierId> ModifierIds { get; }

        public EncounterContentDefinition(
            EncounterId id,
            string displayNameKey,
            IReadOnlyList<SouvenirId> souvenirIds,
            IReadOnlyList<RewardId> rewardIds,
            IReadOnlyList<ModifierId> modifierIds)
        {
            Id = id;
            DisplayNameKey = displayNameKey ?? string.Empty;
            SouvenirIds = souvenirIds ?? new List<SouvenirId>();
            RewardIds = rewardIds ?? new List<RewardId>();
            ModifierIds = modifierIds ?? new List<ModifierId>();
        }
    }
}
