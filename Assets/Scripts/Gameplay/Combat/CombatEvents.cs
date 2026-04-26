using TowerOblivion.Core;

namespace TowerOblivion.Gameplay.Combat
{
    public sealed record CombatStarted(CombatState State) : IGameEvent;
}
