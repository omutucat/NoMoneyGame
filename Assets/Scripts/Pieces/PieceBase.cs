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
        Tank,
        Porn,
        Troll,
        Hero,
        Ghost
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

        protected Dictionary<AttributeType, PieceAttribute> Attributes { get; }

        protected PieceBase(PieceType type, Point position, IEnumerable<PieceAttribute> attributes = null)
        {
            Type = type;
            Position = position;
            Attributes = new Dictionary<AttributeType, PieceAttribute>();
            if (attributes != null)
            {
                foreach (var attr in attributes)
                {
                    Attributes[attr.Type] = attr;
                }
            }
        }

        public abstract void OnTurnEnd();

        public void SetPosition(Point position) => Position = position;
    }
}

