using System;

namespace TowerOblivion.Core
{
    [Serializable]
    public struct CardInstanceId : IEquatable<CardInstanceId>
    {
        public string Value { get; private set; }

        public CardInstanceId(string value)
        {
            Value = value ?? string.Empty;
        }

        public bool Equals(CardInstanceId other) => Value == other.Value;
        public override bool Equals(object obj) => obj is CardInstanceId other && Equals(other);
        public override int GetHashCode() => Value == null ? 0 : Value.GetHashCode();
        public override string ToString() => Value ?? string.Empty;
    }
}
