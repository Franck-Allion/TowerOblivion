using TowerOblivion.Core;

namespace TowerOblivion.Gameplay.Content
{
    public sealed class SouvenirContentDefinition
    {
        public SouvenirId Id { get; }
        public string DisplayNameKey { get; }
        public int BaseLife { get; }

        public SouvenirContentDefinition(SouvenirId id, string displayNameKey, int baseLife)
        {
            Id = id;
            DisplayNameKey = displayNameKey ?? string.Empty;
            BaseLife = baseLife;
        }
    }
}
