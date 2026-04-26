using System;
using System.IO;
using System.Text;
using Newtonsoft.Json;
using TowerOblivion.Core;
using TowerOblivion.Gameplay.Persistence;

namespace TowerOblivion.Infrastructure.Persistence
{
    public sealed class JsonSaveRepository : ISaveRepository
    {
        private readonly string _rootDirectory;
        private readonly SaveDataValidator _validator;
        private readonly JsonSerializerSettings _serializerSettings;

        public JsonSaveRepository(string rootDirectory, SaveDataValidator validator = null)
        {
            if (string.IsNullOrWhiteSpace(rootDirectory))
            {
                throw new ArgumentException("Root directory must be provided.", nameof(rootDirectory));
            }

            _rootDirectory = Path.GetFullPath(rootDirectory);
            _validator = validator;
            _serializerSettings = new JsonSerializerSettings
            {
                Formatting = Formatting.Indented
            };
        }

        public Result Save(SaveSlot slot, SaveSnapshotV1 snapshot)
        {
            if (snapshot == null)
            {
                return Result.Failure("save.invalid_snapshot", "Snapshot is required.");
            }

            if (_validator != null)
            {
                var validationResult = _validator.Validate(snapshot);
                if (validationResult.IsFailure)
                {
                    return validationResult;
                }
            }

            try
            {
                if (!TryResolvePath(slot, out var path, out var slotError))
                {
                    return Result.Failure("save.invalid_slot", slotError);
                }

                Directory.CreateDirectory(_rootDirectory);

                var backupPath = path + ".bak";
                var tempPath = path + ".tmp";

                // 1. Create backup of existing save
                if (File.Exists(path))
                {
                    if (File.Exists(backupPath)) File.Delete(backupPath);
                    File.Move(path, backupPath);
                }

                // 2. Write to temp file (atomic-ish)
                var json = JsonConvert.SerializeObject(snapshot, _serializerSettings);
                File.WriteAllText(tempPath, json, Encoding.UTF8);

                // 3. Move temp to primary
                File.Move(tempPath, path);

                return Result.Success();
            }
            catch (Exception exception)
            {
                return Result.Failure("save.write_failed", exception.Message);
            }
        }

        public SaveLoadResult Load(SaveSlot slot)
        {
            if (!TryResolvePath(slot, out var path, out var slotError))
            {
                return SaveLoadResult.Failure("save.invalid_slot", slotError);
            }

            var backupPath = path + ".bak";

            // Try primary then backup
            var result = TryLoadAndValidate(path, slot);
            if (result.Result.IsFailure && File.Exists(backupPath))
            {
                var backupResult = TryLoadAndValidate(backupPath, slot);
                if (backupResult.Result.IsSuccess)
                {
                    return backupResult;
                }
            }

            return result;
        }

        private SaveLoadResult TryLoadAndValidate(string path, SaveSlot slot)
        {
            try
            {
                if (!File.Exists(path))
                {
                    return SaveLoadResult.NotFound(slot);
                }

                var json = File.ReadAllText(path, Encoding.UTF8);
                var snapshot = JsonConvert.DeserializeObject<SaveSnapshotV1>(json, _serializerSettings);
                if (snapshot == null)
                {
                    return SaveLoadResult.Failure("save.deserialize_failed", $"Save file for slot '{slot}' produced a null snapshot.");
                }

                if (_validator != null)
                {
                    var validationResult = _validator.Validate(snapshot);
                    if (validationResult.IsFailure)
                    {
                        return SaveLoadResult.Failure(validationResult.ErrorCode, validationResult.ErrorMessage);
                    }
                }

                return SaveLoadResult.Success(snapshot);
            }
            catch (Exception exception)
            {
                return SaveLoadResult.Failure("save.read_failed", exception.Message);
            }
        }

        private bool TryResolvePath(SaveSlot slot, out string path, out string errorMessage)
        {
            path = string.Empty;
            errorMessage = string.Empty;

            var rawSlotName = string.IsNullOrWhiteSpace(slot.Value) ? "primary" : slot.Value.Trim();
            if (rawSlotName.Length > 64) // Added length limit
            {
                 errorMessage = $"Slot '{rawSlotName}' exceeds maximum length.";
                 return false;
            }

            if (rawSlotName.IndexOfAny(Path.GetInvalidFileNameChars()) >= 0 ||
                rawSlotName.Contains(Path.DirectorySeparatorChar.ToString(), StringComparison.Ordinal) ||
                rawSlotName.Contains(Path.AltDirectorySeparatorChar.ToString(), StringComparison.Ordinal) ||
                rawSlotName.Contains("..", StringComparison.Ordinal))
            {
                errorMessage = $"Slot '{rawSlotName}' contains invalid characters.";
                return false;
            }

            var normalizedSlotName = rawSlotName.ToLowerInvariant();
            var candidatePath = Path.GetFullPath(Path.Combine(_rootDirectory, $"{normalizedSlotName}.save.json"));
            var rootWithSeparator = _rootDirectory.EndsWith(Path.DirectorySeparatorChar.ToString(), StringComparison.Ordinal)
                ? _rootDirectory
                : _rootDirectory + Path.DirectorySeparatorChar;

            if (!candidatePath.StartsWith(rootWithSeparator, StringComparison.OrdinalIgnoreCase))
            {
                errorMessage = $"Slot '{rawSlotName}' resolves outside the save root.";
                return false;
            }

            path = candidatePath;
            return true;
        }
    }
}
