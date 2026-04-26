using System;

namespace TowerOblivion.Core
{
    [Serializable]
    public struct RunSeed : IEquatable<RunSeed>
    {
        public int Value { get; set; }

        public RunSeed(int value)
        {
            Value = value;
        }

        public bool Equals(RunSeed other)
        {
            return Value == other.Value;
        }

        public override bool Equals(object obj)
        {
            return obj is RunSeed other && Equals(other);
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
