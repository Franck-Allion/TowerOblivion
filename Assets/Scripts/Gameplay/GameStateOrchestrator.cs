using System;
using System.Collections.Generic;
using TowerOblivion.Core;

namespace TowerOblivion.Gameplay
{
    /// <summary>
    /// Manages the high-level game mode transitions for the playable slice.
    /// Follows the Unity-free Gameplay rule.
    /// </summary>
    public sealed class GameStateOrchestrator
    {
        private readonly IEventBus _eventBus;
        private readonly Dictionary<GameModeId, HashSet<GameModeId>> _legalTransitions;

        public GameModeId CurrentMode { get; private set; }

        public GameStateOrchestrator(IEventBus eventBus)
        {
            _eventBus = eventBus ?? throw new ArgumentNullException(nameof(eventBus));
            CurrentMode = GameModeId.RebirthHub;

            // Define the Epic 2 Story 2 placeholder loop
            _legalTransitions = new Dictionary<GameModeId, HashSet<GameModeId>>
            {
                { GameModeId.RebirthHub, new HashSet<GameModeId> { GameModeId.Exploration } },
                { GameModeId.Exploration, new HashSet<GameModeId> { GameModeId.Combat } },
                { GameModeId.Combat, new HashSet<GameModeId> { GameModeId.Reward } },
                { GameModeId.Reward, new HashSet<GameModeId> { GameModeId.Resolution } },
                { GameModeId.Resolution, new HashSet<GameModeId> { GameModeId.RebirthHub, GameModeId.Exploration } }
            };
        }

        /// <summary>
        /// Attempts to transition the game to a new mode.
        /// Only allowed transitions from the placeholder loop are permitted.
        /// </summary>
        public Result TransitionTo(GameModeId nextMode)
        {
            if (!_legalTransitions.TryGetValue(CurrentMode, out var allowed) || !allowed.Contains(nextMode))
            {
                return Result.Failure("mode.invalid_transition", $"Cannot transition from {CurrentMode} to {nextMode}");
            }

            var previousMode = CurrentMode;
            CurrentMode = nextMode;

            _eventBus.Publish(new GameModeChanged(previousMode, CurrentMode));

            return Result.Success();
        }
    }
}
