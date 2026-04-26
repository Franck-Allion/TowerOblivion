using System;

namespace TowerOblivion.Gameplay.Combat
{
    [Serializable]
    public struct ActorId : IEquatable<ActorId>
    {
        public string Value { get; private set; }

        public ActorId(string value)
        {
            Value = value ?? string.Empty;
        }

        public bool Equals(ActorId other) => Value == other.Value;
        public override bool Equals(object obj) => obj is ActorId other && Equals(other);
        public override int GetHashCode() => Value == null ? 0 : Value.GetHashCode();
        public override string ToString() => Value ?? string.Empty;
    }
}
