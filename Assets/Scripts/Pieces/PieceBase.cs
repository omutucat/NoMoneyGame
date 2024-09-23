using System.Collections.Generic;
using System.Linq;
using NoMoney.Assets.Scripts.Board;

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
        public List<PieceStatus> StatusList { get; private set; }

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
            // 駒がInSleepかImmobilizedの時は移動できない
            { } when StatusList.Any(s => s is InSleep or Immobilized) => new List<Point>(),
            _ => CalculateMoveablePoints(Position, SpecificMovablePoints, Direction)
        };

        protected Piece(Point position, PieceSide side, IEnumerable<PieceStatus>? statusList = null) : base(position)
        {
            Side = side;
            StatusList = statusList?.ToList() ?? new List<PieceStatus>();
            Direction = side switch
            {
                PieceSide.Player => PieceDirection.Up,
                PieceSide.Enemy => PieceDirection.Down,
                _ => throw new System.NotImplementedException()
            };
        }

        public void SetPosition(Point position) => Position = position;

        public bool IsContainStatus(PieceStatus status) => StatusList.Contains(status);

        public virtual void OnTurnEnd() => StatusList.Where(s => s is ITurnEndListener).Cast<ITurnEndListener>().ToList().ForEach(s => s.OnTurnEnd());

        public void AddStatus(PieceStatus status)
        {
            if (!IsContainStatus(status))
            {
                StatusList.Add(status);
                status.OnRemove += (status) => StatusList.Remove(status);
            }
        }

        public virtual bool TryMove(Point point, BoardModel board)
        {
            if (!board.GetMovablePoints(this).Contains(point))
            {
                return false;
            }

            var objects = board.GetObjectsAt(point);

            var canMove = objects switch
            {
                { } o when o.Any(o => o is IUnbreakable) => false,
                { } o when o.Any(o => o is Piece p && p.Side == Side) => false,
                _ => true
            };

            if (!canMove)
            {
                return false;
            }

            objects.ForEach(o => o.Destroy());
            SetPosition(point);
            return true;
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

