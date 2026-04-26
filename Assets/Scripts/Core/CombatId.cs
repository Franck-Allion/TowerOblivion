using System;

namespace TowerOblivion.Core
{
    [Serializable]
    public readonly struct CombatId : IEquatable<CombatId>
    {
        public string Value { get; }

        public CombatId(string value)
        {
            Value = value ?? string.Empty;
        }

        public bool Equals(CombatId other) => Value == other.Value;
        public override bool Equals(object obj) => obj is CombatId other && Equals(other);
        public override int GetHashCode() => Value == null ? 0 : Value.GetHashCode();
        public override string ToString() => Value ?? string.Empty;
    }
}
