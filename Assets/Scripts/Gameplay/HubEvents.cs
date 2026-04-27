using TowerOblivion.Core;
using TowerOblivion.Gameplay.RunGeneration;

namespace TowerOblivion.Gameplay
{
    /// <summary>
    /// Fact: The player has entered the Hub area.
    /// </summary>
    public sealed record HubEntered : IGameEvent;

    /// <summary>
    /// Intent: The player wants to start a new run.
    /// </summary>
    public sealed record StartRunRequested : IGameEvent;

    /// <summary>
    /// Fact: A new run has been successfully initialized.
    /// </summary>
    /// <param name="InitialState">The starting state of the run.</param>
    public sealed record RunStarted(RunState InitialState) : IGameEvent;
}
