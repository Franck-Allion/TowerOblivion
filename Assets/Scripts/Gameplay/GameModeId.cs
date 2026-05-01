using System;

namespace TowerOblivion.Gameplay
{
    /// <summary>
    /// Identifies a high-level game mode/state for the vertical slice.
    /// Follows the typed ID pattern used in the project.
    /// </summary>
    [Serializable]
    public readonly struct GameModeId : IEquatable<GameModeId>
    {
        public string Value { get; }

        public GameModeId(string value)
        {
            Value = value ?? string.Empty;
        }

        public static readonly GameModeId RebirthHub = new("mode.rebirth_hub");
        public static readonly GameModeId Exploration = new("mode.exploration");
        public static readonly GameModeId Combat = new("mode.combat");
        public static readonly GameModeId Reward = new("mode.reward");
        public static readonly GameModeId Resolution = new("mode.resolution");

        public bool Equals(GameModeId other) => string.Equals(Value, other.Value);
        public override bool Equals(object obj) => obj is GameModeId other && Equals(other);
        public override int GetHashCode() => Value != null ? Value.GetHashCode() : 0;
        public override string ToString() => Value ?? string.Empty;

        public static bool operator ==(GameModeId left, GameModeId right) => left.Equals(right);
        public static bool operator !=(GameModeId left, GameModeId right) => !left.Equals(right);
    }
}
