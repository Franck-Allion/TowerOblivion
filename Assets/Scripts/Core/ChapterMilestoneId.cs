using System;

namespace TowerOblivion.Core
{
    [Serializable]
    public struct ChapterMilestoneId : IEquatable<ChapterMilestoneId>
    {
        public string Value { get; set; }

        public ChapterMilestoneId(string value)
        {
            Value = value ?? string.Empty;
        }

        public bool Equals(ChapterMilestoneId other) => Value == other.Value;
        public override bool Equals(object obj) => obj is ChapterMilestoneId other && Equals(other);
        public override int GetHashCode() => Value == null ? 0 : Value.GetHashCode();
        public override string ToString() => Value ?? string.Empty;
    }
}
