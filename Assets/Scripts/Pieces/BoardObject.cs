using System;

namespace NoMoney.Assets.Scripts.Pieces
{
    /// <summary>
    /// 座標
    /// </summary>
    public struct Point
    {
        public int X;
        public int Y;

        public Point(int x, int y)
        {
            X = x;
            Y = y;
        }

        public readonly string ToDebugString() => $"({X}, {Y})";
    }

    public abstract class BoardObject
    {
        /// <summary>
        /// 駒の位置
        /// </summary>
        public Point Position { get; protected set; }
    }
}
