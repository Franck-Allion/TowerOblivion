using System;

namespace TowerOblivion.Gameplay.Narrative
{
    [Serializable]
    public struct NarrativeFlagId : IEquatable<NarrativeFlagId>
    {
        public string Value { get; private set; }

        public NarrativeFlagId(string value)
        {
            Value = value ?? string.Empty;
        }

        public bool Equals(NarrativeFlagId other) => Value == other.Value;
        public override bool Equals(object obj) => obj is NarrativeFlagId other && Equals(other);
        public override int GetHashCode() => Value == null ? 0 : Value.GetHashCode();
        public override string ToString() => Value ?? string.Empty;
    }
}
