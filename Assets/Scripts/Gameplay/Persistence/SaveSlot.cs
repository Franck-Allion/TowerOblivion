using System;

namespace TowerOblivion.Gameplay.Persistence
{
    [Serializable]
    public struct SaveSlot : IEquatable<SaveSlot>
    {
        public string Value { get; set; }

        public SaveSlot(string value)
        {
            Value = string.IsNullOrWhiteSpace(value) ? "primary" : value.Trim();
        }

        public static SaveSlot Primary => new SaveSlot("primary");

        public bool Equals(SaveSlot other) => string.Equals(Value, other.Value, StringComparison.OrdinalIgnoreCase);
        public override bool Equals(object obj) => obj is SaveSlot other && Equals(other);
        public override int GetHashCode()
        {
            var canonical = string.IsNullOrWhiteSpace(Value) ? "primary" : Value;
            return StringComparer.OrdinalIgnoreCase.GetHashCode(canonical);
        }
        public override string ToString() => Value ?? "primary";
    }
}
