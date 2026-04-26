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
        private readonly JsonSerializerSettings _serializerSettings;

        public JsonSaveRepository(string rootDirectory)
        {
            if (string.IsNullOrWhiteSpace(rootDirectory))
            {
                throw new ArgumentException("Root directory must be provided.", nameof(rootDirectory));
            }

            _rootDirectory = Path.GetFullPath(rootDirectory);
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

            try
            {
                if (!TryResolvePath(slot, out var path, out var slotError))
                {
                    return Result.Failure("save.invalid_slot", slotError);
                }

                Directory.CreateDirectory(_rootDirectory);
                var json = JsonConvert.SerializeObject(snapshot, _serializerSettings);
                File.WriteAllText(path, json, Encoding.UTF8);
                return Result.Success();
            }
            catch (Exception exception)
            {
                return Result.Failure("save.write_failed", exception.Message);
            }
        }

        public SaveLoadResult Load(SaveSlot slot)
        {
            try
            {
                if (!TryResolvePath(slot, out var path, out var slotError))
                {
                    return SaveLoadResult.Failure("save.invalid_slot", slotError);
                }

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
