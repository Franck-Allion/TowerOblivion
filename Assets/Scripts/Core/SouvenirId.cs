using System;

namespace TowerOblivion.Core
{
    [Serializable]
    public struct SouvenirId : IEquatable<SouvenirId>
    {
        public string Value { get; private set; }

        public SouvenirId(string value)
        {
            Value = value ?? string.Empty;
        }

        public bool Equals(SouvenirId other) => Value == other.Value;
        public override bool Equals(object obj) => obj is SouvenirId other && Equals(other);
        public override int GetHashCode() => Value == null ? 0 : Value.GetHashCode();
        public override string ToString() => Value ?? string.Empty;
    }
}
