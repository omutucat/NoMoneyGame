using System;

namespace NoMoney.Assets.Scripts.Pieces
{
    /// <summary>
    /// 座標
    /// </summary>
    public readonly struct Point : IEquatable<Point>
    {
        public readonly int X;
        public readonly int Y;

        public Point(int x, int y)
        {
            X = x;
            Y = y;
        }

        public readonly string ToDebugString() => $"({X}, {Y})";

        public bool Equals(Point other)
        {
            return X == other.X && Y == other.Y;
        }

        public override bool Equals(object obj)
        {
            return obj is Point other && Equals(other);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(X, Y);
        }

        public static bool operator ==(Point left, Point right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(Point left, Point right)
        {
            return !left.Equals(right);
        }
    }

    public abstract class BoardObject
    {
        /// <summary>
        /// 駒の位置
        /// </summary>
        public Point Position { get; protected set; }

        protected BoardObject(Point position) => Position = position;
    }
}
