using TowerOblivion.Core;

namespace TowerOblivion.Gameplay.Persistence
{
    public sealed class SaveLoadResult
    {
        public Result Result { get; set; }
        public SaveSnapshotV1 Snapshot { get; set; }

        public bool HasSnapshot => Snapshot != null;

        public static SaveLoadResult Success(SaveSnapshotV1 snapshot)
        {
            return new SaveLoadResult
            {
                Result = Result.Success(),
                Snapshot = snapshot
            };
        }

        public static SaveLoadResult NotFound(SaveSlot slot)
        {
            return new SaveLoadResult
            {
                Result = Result.Failure("save.not_found", $"No save file exists for slot '{slot}'."),
                Snapshot = null
            };
        }

        public static SaveLoadResult Failure(string errorCode, string errorMessage)
        {
            return new SaveLoadResult
            {
                Result = Result.Failure(errorCode, errorMessage),
                Snapshot = null
            };
        }
    }
}
