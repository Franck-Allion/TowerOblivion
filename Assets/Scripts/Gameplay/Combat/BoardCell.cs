using System;

namespace TowerOblivion.Gameplay.Combat
{
    [Serializable]
    public struct BoardCell : IEquatable<BoardCell>
    {
        public int X { get; set; }
        public int Y { get; set; }

        public BoardCell(int x, int y)
        {
            X = x;
            Y = y;
        }

        public bool Equals(BoardCell other)
        {
            return X == other.X && Y == other.Y;
        }

        public override bool Equals(object obj)
        {
            return obj is BoardCell other && Equals(other);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return (X * 397) ^ Y;
            }
        }
    }
}
