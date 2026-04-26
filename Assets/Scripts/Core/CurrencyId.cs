using System;

namespace TowerOblivion.Core
{
    [Serializable]
    public struct CurrencyId : IEquatable<CurrencyId>
    {
        public string Value { get; set; }

        public CurrencyId(string value)
        {
            Value = value ?? string.Empty;
        }

        public bool Equals(CurrencyId other) => Value == other.Value;
        public override bool Equals(object obj) => obj is CurrencyId other && Equals(other);
        public override int GetHashCode() => Value == null ? 0 : Value.GetHashCode();
        public override string ToString() => Value ?? string.Empty;
    }
}
