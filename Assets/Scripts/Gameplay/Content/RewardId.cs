using System;

namespace TowerOblivion.Gameplay.Content
{
    [Serializable]
    public struct RewardId : IEquatable<RewardId>
    {
        public string Value { get; private set; }

        public RewardId(string value)
        {
            Value = value ?? string.Empty;
        }

        public bool Equals(RewardId other) => Value == other.Value;
        public override bool Equals(object obj) => obj is RewardId other && Equals(other);
        public override int GetHashCode() => Value == null ? 0 : Value.GetHashCode();
        public override string ToString() => Value ?? string.Empty;
    }
}
