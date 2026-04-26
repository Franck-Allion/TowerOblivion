using TowerOblivion.Core;

namespace TowerOblivion.Gameplay.Content
{
    public sealed class ModifierContentDefinition
    {
        public ModifierId Id { get; }
        public string DisplayNameKey { get; }
        public int Magnitude { get; }

        public ModifierContentDefinition(ModifierId id, string displayNameKey, int magnitude)
        {
            Id = id;
            DisplayNameKey = displayNameKey ?? string.Empty;
            Magnitude = magnitude;
        }
    }
}
