using TowerOblivion.Core;

namespace TowerOblivion.Gameplay.Content
{
    public sealed class NarrativeEventContentDefinition
    {
        public NarrativeEventId Id { get; }
        public string DisplayNameKey { get; }
        
        // Boolean Requirement
        public NarrativeFlagId RequiredFlagId { get; }
        public bool RequiredFlagValue { get; }

        // Counter Requirement
        public NarrativeFlagId RequiredCounterId { get; }
        public int RequiredCounterMinValue { get; }

        public NarrativeEventContentDefinition(
            NarrativeEventId id, 
            string displayNameKey, 
            NarrativeFlagId requiredFlagId,
            bool requiredFlagValue = true,
            NarrativeFlagId requiredCounterId = default,
            int requiredCounterMinValue = 0)
        {
            Id = id;
            DisplayNameKey = displayNameKey ?? string.Empty;
            RequiredFlagId = requiredFlagId;
            RequiredFlagValue = requiredFlagValue;
            RequiredCounterId = requiredCounterId;
            RequiredCounterMinValue = requiredCounterMinValue;
        }
    }
}
