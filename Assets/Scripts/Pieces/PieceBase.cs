using System.Collections.Generic;
using System.Linq;

namespace NoMoney.Assets.Scripts.Pieces
{
    /// <summary>
    /// 駒の基底クラス
    /// </summary>
    public abstract class Piece : BoardObject
    {
        public PieceDirection Direction { get; protected set; }

        public PieceSide Side { get; protected set; }

        /// <summary>
        /// 駒の属性
        /// </summary>
        protected List<PieceStatus> StatusList { get; }

        /// <summary>
        /// 派生クラスで実装する移動可能な座標
        /// </summary>
        /// <returns></returns>
        protected abstract List<Point> SpecificMovablePoints { get; }

        /// <summary>
        /// 移動可能な座標
        /// </summary>
        public List<Point> MoveablePoints => StatusList switch
        {
            // 駒がImmobilizedの場合は移動できない
            { } when StatusList.Any(s => s == PieceStatus.Immobilized) => new List<Point>(),
            _ => CalculateMoveablePoints(Position, SpecificMovablePoints, Direction)
        };

        protected Piece(Point position, IEnumerable<PieceStatus>? statusList = null, PieceDirection direction = PieceDirection.Up, PieceSide side = PieceSide.Player) : base(position)
        {
            Direction = direction;
            Side = side;
            StatusList = statusList?.ToList() ?? new List<PieceStatus>();
        }

        public void SetPosition(Point position) => Position = position;

        public bool IsContainStatus(PieceStatus status) => StatusList.Contains(status);

        public virtual void OnTurnEnd()
        {
        }

        /// <summary>
        /// 方向を加味して移動可能な座標を計算する
        /// </summary>
        /// <param name="position"></param>
        /// <param name="moveablePoints"></param>
        /// <param name="direction"></param>
        /// <returns></returns>
        /// <exception cref="System.NotImplementedException"></exception>
        private static List<Point> CalculateMoveablePoints(Point position, List<Point> moveablePoints, PieceDirection direction) =>
            moveablePoints.Select(p => direction switch
            {
                PieceDirection.Up => new Point(position.X + p.X, position.Y + p.Y),
                PieceDirection.Down => new Point(position.X - p.X, position.Y - p.Y),
                _ => throw new System.NotImplementedException()
            }).ToList();
    }

    public enum PieceDirection
    {
        Up,
        Down,
        // Left,
        // Right
    }

    public enum PieceSide
    {
        Player,
        Enemy
    }
}

