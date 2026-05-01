using System;
using System.Collections.Generic;
using TowerOblivion.Core;
using TowerOblivion.Gameplay.Content;

namespace TowerOblivion.Infrastructure.Content.Validation
{
    public sealed class PlaceholderContentValidator
    {
        public Result Validate(PlaceholderContentCatalogs catalogs)
        {
            if (catalogs == null)
            {
                return Result.Failure("content.null_catalogs", "Catalogs object is null.");
            }

            if (HasInvalidIds(catalogs))
            {
                return Result.Failure("content.invalid_id", "Empty IDs detected in content.");
            }

            if (HasDuplicateIds(catalogs))
            {
                return Result.Failure("content.duplicate_id", "Duplicate IDs detected in content.");
            }

            if (HasMissingReferences(catalogs))
            {
                return Result.Failure("content.missing_reference", "Missing required content references detected.");   
            }

            if (HasInvalidRewards(catalogs))
            {
                return Result.Failure("content.invalid_reward", "Invalid reward currency ID or amount detected.");     
            }

            if (HasInvalidNarrativeDefinitions(catalogs))
            {
                return Result.Failure("content.invalid_narrative", "Invalid narrative event flag or counter IDs detected.");
            }

            return Result.Success();
        }

        private static bool HasInvalidIds(PlaceholderContentCatalogs catalogs)
        {
            foreach (var d in catalogs.RoomDefinitions) if (string.IsNullOrEmpty(d.Id.Value)) return true;
            foreach (var d in catalogs.EncounterDefinitions) if (string.IsNullOrEmpty(d.Id.Value)) return true;
            foreach (var d in catalogs.SouvenirDefinitions) if (string.IsNullOrEmpty(d.Id.Value)) return true;
            foreach (var d in catalogs.RewardDefinitions) if (string.IsNullOrEmpty(d.Id.Value)) return true;
            foreach (var d in catalogs.NarrativeEventDefinitions) if (string.IsNullOrEmpty(d.Id.Value)) return true;
            foreach (var d in catalogs.ModifierDefinitions) if (string.IsNullOrEmpty(d.Id.Value)) return true;
            return false;
        }

        private static bool HasDuplicateIds(PlaceholderContentCatalogs catalogs)
        {
            if (HasDuplicateId(catalogs.RoomDefinitions, x => x.Id.Value)) return true;
            if (HasDuplicateId(catalogs.EncounterDefinitions, x => x.Id.Value)) return true;
            if (HasDuplicateId(catalogs.SouvenirDefinitions, x => x.Id.Value)) return true;
            if (HasDuplicateId(catalogs.RewardDefinitions, x => x.Id.Value)) return true;
            if (HasDuplicateId(catalogs.NarrativeEventDefinitions, x => x.Id.Value)) return true;
            if (HasDuplicateId(catalogs.ModifierDefinitions, x => x.Id.Value)) return true;
            return false;
        }

        private static bool HasDuplicateId<T>(IReadOnlyList<T> values, Func<T, string> idSelector)
        {
            var ids = new HashSet<string>();
            foreach (var val in values)
            {
                var id = idSelector(val) ?? string.Empty;
                if (!ids.Add(id)) return true;
            }
            return false;
        }

        private static bool HasMissingReferences(PlaceholderContentCatalogs catalogs)
        {
            foreach (var room in catalogs.RoomDefinitions)
            {
                foreach (var encounterId in room.EncounterIds)
                {
                    if (!catalogs.Encounters.TryGet(encounterId, out _)) return true;
                }
                foreach (var narrativeId in room.NarrativeEventIds)
                {
                    if (!catalogs.NarrativeEvents.TryGet(narrativeId, out _)) return true;
                }
            }

            foreach (var encounter in catalogs.EncounterDefinitions)
            {
                foreach (var souvenirId in encounter.SouvenirIds)
                {
                    if (!catalogs.Souvenirs.TryGet(souvenirId, out _)) return true;
                }
                foreach (var rewardId in encounter.RewardIds)
                {
                    if (!catalogs.Rewards.TryGet(rewardId, out _)) return true;
                }
                foreach (var modifierId in encounter.ModifierIds)
                {
                    if (!catalogs.Modifiers.TryGet(modifierId, out _)) return true;
                }
            }

            return false;
        }

        private static bool HasInvalidRewards(PlaceholderContentCatalogs catalogs)
        {
            foreach (var reward in catalogs.RewardDefinitions)
            {
                if (string.IsNullOrEmpty(reward.CurrencyId.Value) || reward.Amount < 0) return true;
            }
            return false;
        }

        private static bool HasInvalidNarrativeDefinitions(PlaceholderContentCatalogs catalogs)
        {
            foreach (var narrative in catalogs.NarrativeEventDefinitions)
            {
                if (string.IsNullOrEmpty(narrative.RequiredFlagId.Value) && string.IsNullOrEmpty(narrative.RequiredCounterId.Value)) return true;
            }
            return false;
        }
    }
}
