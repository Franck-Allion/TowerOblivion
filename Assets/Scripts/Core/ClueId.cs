using System;

namespace TowerOblivion.Core
{
    [Serializable]
    public struct ClueId : IEquatable<ClueId>
    {
        public string Value { get; set; }

        public ClueId(string value)
        {
            Value = value ?? string.Empty;
        }

        public bool Equals(ClueId other) => Value == other.Value;
        public override bool Equals(object obj) => obj is ClueId other && Equals(other);
        public override int GetHashCode() => Value == null ? 0 : Value.GetHashCode();
        public override string ToString() => Value ?? string.Empty;
    }
}
