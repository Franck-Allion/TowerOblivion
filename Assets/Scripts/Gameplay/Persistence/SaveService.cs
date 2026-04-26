using System;
using TowerOblivion.Core;

namespace TowerOblivion.Gameplay.Persistence
{
    public sealed class SaveService
    {
        private readonly ISaveRepository _saveRepository;

        public SaveService(ISaveRepository saveRepository)
        {
            _saveRepository = saveRepository ?? throw new ArgumentNullException(nameof(saveRepository));
        }

        public SaveSnapshotV1 CreateDefaultSnapshot(ContentVersion contentVersion, SaveVersion saveVersion, DateTime savedAtUtc)
        {
            return SaveSnapshotFactory.CreateDefault(contentVersion, saveVersion, savedAtUtc);
        }

        public Result Save(SaveSlot slot, SaveSnapshotV1 snapshot)
        {
            return _saveRepository.Save(slot, snapshot);
        }

        public SaveLoadResult Load(SaveSlot slot)
        {
            return _saveRepository.Load(slot);
        }
    }
}
