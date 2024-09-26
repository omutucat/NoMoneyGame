using System;

namespace NoMoney.Assets.Scripts.Game.Objects
{
    /// <summary>
    /// 盤面上の座標
    /// </summary>
    public readonly struct BoardPoint : IEquatable<BoardPoint>
    {
        public readonly int X;
        public readonly int Y;

        public BoardPoint(int x, int y)
        {
            X = x;
            Y = y;
        }

        public readonly string ToDebugString() => $"({X}, {Y})";
        public bool Equals(BoardPoint other) => X == other.X && Y == other.Y;
        public override bool Equals(object obj) => obj is BoardPoint other && Equals(other);
        public override int GetHashCode() => HashCode.Combine(X, Y);
        public static bool operator ==(BoardPoint left, BoardPoint right) => left.Equals(right);
        public static bool operator !=(BoardPoint left, BoardPoint right) => !left.Equals(right);
        public static BoardPoint operator +(BoardPoint a, BoardPoint b) => new(a.X + b.X, a.Y + b.Y);
        public static BoardPoint operator -(BoardPoint a, BoardPoint b) => new(a.X - b.X, a.Y - b.Y);
    }
}