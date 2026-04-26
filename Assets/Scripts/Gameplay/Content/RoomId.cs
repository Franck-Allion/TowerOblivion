using System;

namespace TowerOblivion.Gameplay.Content
{
    [Serializable]
    public struct RoomId : IEquatable<RoomId>
    {
        public string Value { get; private set; }

        public RoomId(string value)
        {
            Value = value ?? string.Empty;
        }

        public bool Equals(RoomId other) => Value == other.Value;
        public override bool Equals(object obj) => obj is RoomId other && Equals(other);
        public override int GetHashCode() => Value == null ? 0 : Value.GetHashCode();
        public override string ToString() => Value ?? string.Empty;
    }
}
