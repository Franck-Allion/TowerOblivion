using System;
using TowerOblivion.Core;

namespace TowerOblivion.Gameplay.Persistence
{
    [Serializable]
    public class SaveMetadata
    {
        public SaveVersion SaveVersion { get; set; }
        public ContentVersion ContentVersion { get; set; }
        public DateTime SavedAtUtc { get; set; }
    }
}
