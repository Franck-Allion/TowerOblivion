using System;

namespace TowerOblivion.Core
{
    [Serializable]
    public struct NarrativeEventId : IEquatable<NarrativeEventId>
    {
        public string Value { get; private set; }

        public NarrativeEventId(string value)
        {
            Value = value ?? string.Empty;
        }

        public bool Equals(NarrativeEventId other) => Value == other.Value;
        public override bool Equals(object obj) => obj is NarrativeEventId other && Equals(other);
        public override int GetHashCode() => Value == null ? 0 : Value.GetHashCode();
        public override string ToString() => Value ?? string.Empty;
    }
}
