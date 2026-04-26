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
                return Result.Failure("content.validation_failed", "Catalogs are required.");
            }

            if (HasInvalidIds(catalogs))
            {
                return Result.Failure("content.invalid_id", "Invalid (null or empty) content IDs detected.");
            }

            if (HasDuplicates(catalogs))
            {
                return Result.Failure("content.duplicate_id", "Duplicate content IDs detected.");
            }

            if (HasMissingReferences(catalogs))
            {
                return Result.Failure("content.missing_reference", "Missing required content references detected.");
            }

            return Result.Success();
        }

        private static bool HasInvalidIds(PlaceholderContentCatalogs catalogs)
        {
            return HasAnyInvalidId(catalogs.RoomDefinitions, definition => definition.Id.Value) ||
                HasAnyInvalidId(catalogs.EncounterDefinitions, definition => definition.Id.Value) ||
                HasAnyInvalidId(catalogs.SouvenirDefinitions, definition => definition.Id.Value) ||
                HasAnyInvalidId(catalogs.RewardDefinitions, definition => definition.Id.Value) ||
                HasAnyInvalidId(catalogs.NarrativeEventDefinitions, definition => definition.Id.Value) ||
                HasAnyInvalidId(catalogs.ModifierDefinitions, definition => definition.Id.Value);
        }

        private static bool HasDuplicates(PlaceholderContentCatalogs catalogs)
        {
            return HasDuplicateIds(catalogs.RoomDefinitions, definition => definition.Id.Value) ||
                HasDuplicateIds(catalogs.EncounterDefinitions, definition => definition.Id.Value) ||
                HasDuplicateIds(catalogs.SouvenirDefinitions, definition => definition.Id.Value) ||
                HasDuplicateIds(catalogs.RewardDefinitions, definition => definition.Id.Value) ||
                HasDuplicateIds(catalogs.NarrativeEventDefinitions, definition => definition.Id.Value) ||
                HasDuplicateIds(catalogs.ModifierDefinitions, definition => definition.Id.Value);
        }

        private static bool HasMissingReferences(PlaceholderContentCatalogs catalogs)
        {
            for (var i = 0; i < catalogs.RoomDefinitions.Count; i++)
            {
                var room = catalogs.RoomDefinitions[i];
                for (var j = 0; j < room.EncounterIds.Count; j++)
                {
                    if (!catalogs.Encounters.TryGet(room.EncounterIds[j], out _))
                    {
                        return true;
                    }
                }

                for (var j = 0; j < room.NarrativeEventIds.Count; j++)
                {
                    if (!catalogs.NarrativeEvents.TryGet(room.NarrativeEventIds[j], out _))
                    {
                        return true;
                    }
                }
            }

            for (var i = 0; i < catalogs.EncounterDefinitions.Count; i++)
            {
                var encounter = catalogs.EncounterDefinitions[i];
                for (var j = 0; j < encounter.SouvenirIds.Count; j++)
                {
                    if (!catalogs.Souvenirs.TryGet(encounter.SouvenirIds[j], out _))
                    {
                        return true;
                    }
                }

                for (var j = 0; j < encounter.RewardIds.Count; j++)
                {
                    if (!catalogs.Rewards.TryGet(encounter.RewardIds[j], out _))
                    {
                        return true;
                    }
                }

                for (var j = 0; j < encounter.ModifierIds.Count; j++)
                {
                    if (!catalogs.Modifiers.TryGet(encounter.ModifierIds[j], out _))
                    {
                        return true;
                    }
                }
            }

            for (var i = 0; i < catalogs.RewardDefinitions.Count; i++)
            {
                var reward = catalogs.RewardDefinitions[i];
                if (string.IsNullOrEmpty(reward.CurrencyId.Value))
                {
                    return true;
                }
            }

            return false;
        }

        private static bool HasAnyInvalidId<TDefinition>(IReadOnlyList<TDefinition> values, System.Func<TDefinition, string> idSelector)
        {
            for (var i = 0; i < values.Count; i++)
            {
                if (string.IsNullOrEmpty(idSelector(values[i])))
                {
                    return true;
                }
            }

            return false;
        }

        private static bool HasDuplicateIds<TDefinition>(IReadOnlyList<TDefinition> values, System.Func<TDefinition, string> idSelector)
        {
            var ids = new HashSet<string>();
            for (var i = 0; i < values.Count; i++)
            {
                var id = idSelector(values[i]) ?? string.Empty;
                if (!ids.Add(id))
                {
                    return true;
                }
            }

            return false;
        }
    }
}
