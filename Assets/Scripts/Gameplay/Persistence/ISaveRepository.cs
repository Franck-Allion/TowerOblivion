using TowerOblivion.Core;

namespace TowerOblivion.Gameplay.Persistence
{
    public interface ISaveRepository
    {
        Result Save(SaveSlot slot, SaveSnapshotV1 snapshot);
        SaveLoadResult Load(SaveSlot slot);
    }
}
