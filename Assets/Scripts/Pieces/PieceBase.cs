using System;
using System.Collections;
using System.Collections.Generic;

namespace NoMoney.Assets.Scripts.Pieces
{
    public abstract class Shape
    {
        // 共通のプロパティやメソッドをここに定義
    }

    public class Normal : Shape
    {
        // Normal 特有のプロパティやメソッド
    }

    public class Abnormal : Shape
    {
        public List<Point> AnotherPositions { get; }

        public Abnormal(List<Point> anotherPositions)
        {
            AnotherPositions = anotherPositions;
        }
    }
    /// <summary>
    /// 駒の種類
    /// </summary>
    public enum PieceType
    {
        Tank,
        Porn,
        Troll,
        
        
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
        
        public Shape Shape { get; }

        public Point Position { get; private set; }

        public abstract List<Point> MoveablePoints { get; }

        public PieceBase(PieceType type, Point position, Shape shape = null)
        {
            Type = type;
            Position = position;
            Shape = shape ?? new Normal();
        }

        public void SetPosition(Point position) => Position = position;
    }
}

