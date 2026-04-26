using System;

namespace TowerOblivion.Core
{
    [Serializable]
    public struct EncounterId : IEquatable<EncounterId>
    {
        public string Value { get; private set; }

        public EncounterId(string value)
        {
            Value = value ?? string.Empty;
        }

        public bool Equals(EncounterId other) => Value == other.Value;
        public override bool Equals(object obj) => obj is EncounterId other && Equals(other);
        public override int GetHashCode() => Value == null ? 0 : Value.GetHashCode();
        public override string ToString() => Value ?? string.Empty;
    }
}
