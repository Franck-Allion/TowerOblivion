using System;

namespace TowerOblivion.Core
{
    [Serializable]
    public struct UpgradeTrackId : IEquatable<UpgradeTrackId>
    {
        public string Value { get; set; }

        public UpgradeTrackId(string value)
        {
            Value = value ?? string.Empty;
        }

        public bool Equals(UpgradeTrackId other) => Value == other.Value;
        public override bool Equals(object obj) => obj is UpgradeTrackId other && Equals(other);
        public override int GetHashCode() => Value == null ? 0 : Value.GetHashCode();
        public override string ToString() => Value ?? string.Empty;
    }
}
