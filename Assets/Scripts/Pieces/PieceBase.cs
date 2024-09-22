using System.Collections.Generic;
using System.Linq;

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

        public string ToDebugString() => $"({X}, {Y})";
    }

    /// <summary>
    /// 駒の基底クラス
    /// </summary>
    public abstract class PieceBase
    {
        /// <summary>
        /// 駒の位置
        /// </summary>
        public Point Position { get; private set; }

        /// <summary>
        /// 移動可能な座標
        /// </summary>
        public abstract List<Point> MoveablePoints { get; }

        /// <summary>
        /// 駒の属性
        /// </summary>
        protected List<PieceStatus> StatusList { get; }

        protected PieceBase(Point position, IEnumerable<PieceStatus> statusList = null)
        {
            Position = position;
            StatusList = statusList?.ToList() ?? new List<PieceStatus>();
        }

        public virtual void OnTurnEnd()
        {
        }

        public void SetPosition(Point position) => Position = position;
        
        public bool isContainStatus(PieceStatus status) => StatusList.Contains(status);
    }
}

