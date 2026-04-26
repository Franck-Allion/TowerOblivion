using TowerOblivion.Core;

namespace TowerOblivion.Gameplay.Content
{
    public sealed class NarrativeEventContentDefinition
    {
        public NarrativeEventId Id { get; }
        public string DisplayNameKey { get; }
        public NarrativeFlagId RequiredFlagId { get; }

        public NarrativeEventContentDefinition(NarrativeEventId id, string displayNameKey, NarrativeFlagId requiredFlagId)
        {
            Id = id;
            DisplayNameKey = displayNameKey ?? string.Empty;
            RequiredFlagId = requiredFlagId;
        }
    }
}
