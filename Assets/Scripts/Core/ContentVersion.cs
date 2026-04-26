using System;

namespace TowerOblivion.Core
{
    [Serializable]
    public struct ContentVersion : IEquatable<ContentVersion>
    {
        public string Value { get; private set; }

        public ContentVersion(string value)
        {
            Value = value ?? string.Empty;
        }

        public bool Equals(ContentVersion other)
        {
            return Value == other.Value;
        }

        public override bool Equals(object obj)
        {
            return obj is ContentVersion other && Equals(other);
        }

        public override int GetHashCode()
        {
            return Value == null ? 0 : Value.GetHashCode();
        }

        public override string ToString()
        {
            return Value ?? string.Empty;
        }
    }
}
