using System;

namespace TowerOblivion.Core
{
    [Serializable]
    public struct SaveVersion : IEquatable<SaveVersion>
    {
        public int Value { get; set; }

        public SaveVersion(int value)
        {
            Value = value;
        }

        public bool Equals(SaveVersion other)
        {
            return Value == other.Value;
        }

        public override bool Equals(object obj)
        {
            return obj is SaveVersion other && Equals(other);
        }

        public override int GetHashCode()
        {
            return Value;
        }

        public override string ToString()
        {
            return Value.ToString();
        }
    }
}
