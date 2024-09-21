using System;
using System.Collections;
using System.Collections.Generic;

namespace NoMoney.Assets.Scripts.Pieces
{
    /// <summary>
    /// 駒の種類
    /// </summary>
    public enum PieceType
    {
        Tank
    }

    public struct Point
    {
        public int X;
        public int Y;

        public Point(int x, int y)
        {
            X = x;
            Y = y;
        }
    }

    /// <summary>
    /// 駒の基底クラス
    /// </summary>
    public abstract class PieceBase
    {
        /// <summary>
        /// 駒の種類
        /// </summary>
        public PieceType Type { get; }

        public Point Position { get; private set; }

        public abstract List<Point> MoveablePoints { get; }

        public PieceBase(PieceType type, Point position)
        {
            Type = type;
            Position = position;
        }

        public void SetPosition(Point position) => Position = position;
    }
}

