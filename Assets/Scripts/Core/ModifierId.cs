using System;

namespace TowerOblivion.Core
{
    [Serializable]
    public struct ModifierId : IEquatable<ModifierId>
    {
        public string Value { get; private set; }

        public ModifierId(string value)
        {
            Value = value ?? string.Empty;
        }

        public bool Equals(ModifierId other) => Value == other.Value;
        public override bool Equals(object obj) => obj is ModifierId other && Equals(other);
        public override int GetHashCode() => Value == null ? 0 : Value.GetHashCode();
        public override string ToString() => Value ?? string.Empty;
    }
}
