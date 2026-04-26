using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using TowerOblivion.Core;
using TowerOblivion.Gameplay.Narrative;
using TowerOblivion.Gameplay.Persistence;
using TowerOblivion.Gameplay.Progression;

namespace TowerOblivion.Infrastructure.Persistence
{
    public sealed class SaveDataValidator
    {
        private readonly ContentVersion _currentVersion;
        private static readonly Regex IdRegex = new Regex(@"^[a-z0-9]+(\.[a-z0-9_]+)*$", RegexOptions.Compiled);

        public SaveDataValidator(ContentVersion currentVersion)
        {
            _currentVersion = currentVersion;
        }

        public Result Validate(SaveSnapshotV1 snapshot)
        {
            if (snapshot == null)
            {
                return Result.Failure("save.invalid_snapshot", "Snapshot is null.");
            }

            if (snapshot.Metadata == null)
            {
                return Result.Failure("save.missing_metadata", "Metadata section is missing.");
            }

            if (!IsCompatible(snapshot.Metadata.ContentVersion, _currentVersion))
            {
                return Result.Failure("save.incompatible_version", 
                    $"Content version mismatch. Expected major of {_currentVersion}, got {snapshot.Metadata.ContentVersion}.");
            }

            if (snapshot.PlayerProfile == null)
            {
                return Result.Failure("save.missing_profile", "Player Profile section is missing.");
            }

            if (snapshot.Narrative == null)
            {
                return Result.Failure("save.missing_narrative", "Narrative section is missing.");
            }

            if (snapshot.ActiveRun != null)
            {
                if (snapshot.ActiveRun.CurrentFloorIndex < 0)
                {
                    return Result.Failure("save.invalid_field", "Current floor index cannot be negative.");
                }

                if (!IsValidId(snapshot.ActiveRun.RunId.Value))
                {
                    return Result.Failure("save.invalid_id", $"Invalid RunId: {snapshot.ActiveRun.RunId.Value}");
                }
            }

            return Result.Success();
        }

        private bool IsCompatible(ContentVersion saved, ContentVersion current)
        {
            // Softened check: Allow loads if the major version matches (e.g. content.1.x.x)
            var savedMajor = GetMajorVersion(saved.Value);
            var currentMajor = GetMajorVersion(current.Value);
            
            return savedMajor == currentMajor;
        }

        private string GetMajorVersion(string version)
        {
            if (string.IsNullOrEmpty(version)) return string.Empty;
            
            // Expected format: "content.X.Y.Z" or "X.Y.Z"
            var parts = version.Split('.');
            foreach (var part in parts)
            {
                if (int.TryParse(part, out _)) return part;
            }
            return version;
        }

        private bool IsValidId(string id)
        {
            if (string.IsNullOrEmpty(id)) return true; // Allow empty IDs to be handled by other logic if needed
            return IdRegex.IsMatch(id);
        }
    }
}
