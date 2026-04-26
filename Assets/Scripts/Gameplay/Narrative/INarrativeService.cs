using TowerOblivion.Core;

namespace TowerOblivion.Gameplay.Narrative
{
    public interface INarrativeService
    {
        // Boolean Flags
        bool GetFlag(NarrativeFlagId flagId);
        void SetFlag(NarrativeFlagId flagId, bool value);
        void ToggleFlag(NarrativeFlagId flagId);

        // Integer Counters
        int GetCounter(NarrativeFlagId counterId);
        void SetCounter(NarrativeFlagId counterId, int value);
        void AdjustCounter(NarrativeFlagId counterId, int amount);
    }
}
