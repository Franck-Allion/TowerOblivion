using TowerOblivion.Core;

namespace TowerOblivion.Gameplay
{
    public sealed record GameModeChanged(GameModeId PreviousMode, GameModeId CurrentMode) : IGameEvent;
}
