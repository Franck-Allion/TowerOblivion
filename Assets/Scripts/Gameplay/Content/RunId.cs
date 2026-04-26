using System;

namespace TowerOblivion.Gameplay.Content
{
    [Serializable]
    public struct RunId : IEquatable<RunId>
    {
        public string Value { get; private set; }

        public RunId(string value)
        {
            Value = value ?? string.Empty;
        }

        public bool Equals(RunId other) => Value == other.Value;
        public override bool Equals(object obj) => obj is RunId other && Equals(other);
        public override int GetHashCode() => Value == null ? 0 : Value.GetHashCode();
        public override string ToString() => Value ?? string.Empty;
    }
}
