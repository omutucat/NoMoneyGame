﻿using System;

namespace NoMoney.Assets.Scripts.Game.Objects.Pieces
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

        public bool Equals(Point other) => X == other.X && Y == other.Y;

        public override bool Equals(object obj) => obj is Point other && Equals(other);

        public override int GetHashCode() => HashCode.Combine(X, Y);

        public static bool operator ==(Point left, Point right) => left.Equals(right);

        public static bool operator !=(Point left, Point right) => !left.Equals(right);
    }

    public abstract class BoardObject
    {
        /// <summary>
        /// 駒の位置
        /// </summary>
        public Point Position { get; protected set; }

        public delegate void DestroyEventHandler(BoardObject sender);

        public event DestroyEventHandler OnDestroy;

        protected BoardObject(Point position) => Position = position;

        /// <summary>
        /// 破壊される時に呼び出すメソッド
        /// OnDestroyイベントを発火する
        /// </summary>
        public virtual void Destroy() => OnDestroy?.Invoke(this);
    }
}