using TowerOblivion.Core;

namespace TowerOblivion.Gameplay.Narrative
{
    public sealed record NarrativeFlagChanged(NarrativeFlagId FlagId, bool NewValue) : IGameEvent;
    public sealed record NarrativeCounterChanged(NarrativeFlagId CounterId, int NewValue) : IGameEvent;
}
