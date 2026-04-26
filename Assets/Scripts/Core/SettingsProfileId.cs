using System;

namespace TowerOblivion.Core
{
    [Serializable]
    public struct SettingsProfileId : IEquatable<SettingsProfileId>
    {
        public string Value { get; set; }

        public SettingsProfileId(string value)
        {
            Value = value ?? string.Empty;
        }

        public bool Equals(SettingsProfileId other) => Value == other.Value;
        public override bool Equals(object obj) => obj is SettingsProfileId other && Equals(other);
        public override int GetHashCode() => Value == null ? 0 : Value.GetHashCode();
        public override string ToString() => Value ?? string.Empty;
    }
}
