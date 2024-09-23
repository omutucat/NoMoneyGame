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
        /// <summary>
        /// 駒の向き
        /// </summary>
        public PieceDirection Direction { get; protected set; }

        /// <summary>
        /// 駒の所属
        /// </summary>
        public PieceSide Side { get; protected set; }

        /// <summary>
        /// 駒の属性
        /// </summary>
        public List<PieceStatus> StatusList { get; private set; }

        protected Piece(Point position, PieceSide side, IEnumerable<PieceStatus>? statusList = null) : base(position)
        {
            Side = side;
            StatusList = statusList?.ToList() ?? new List<PieceStatus>();
            Direction = side switch
            {
                // 向きは所属によって設定する
                PieceSide.Player => PieceDirection.Up,
                PieceSide.Enemy => PieceDirection.Down,
                _ => throw new System.NotImplementedException()
            };
        }

        /// <summary>
        /// 駒の向きを設定する
        /// </summary>
        /// <param name="position"></param>
        public void SetPosition(Point position) => Position = position;

        /// <summary>
        /// ステータスを追加する
        /// </summary>
        /// <param name="status"></param>
        public void AddStatus(PieceStatus status)
        {
            if (!StatusList.Contains(status))
            {
                StatusList.Add(status);

                // ステータスが削除されたらリストから削除する
                status.OnRemove += (status) => StatusList.Remove(status);
            }
        }

        /// <summary>
        /// 指定したピースが移動可能な座標を返す
        /// </summary>
        /// <param name="piece"></param>
        /// <returns></returns>
        public List<Point> GetMovablePoints(BoardModel board) => StatusList switch
        {
            { } s when s.Any(s => s is InSleep or Immobilized) => new List<Point>(),
            _ => JudgeMovablePoints(CalculateMovablePoints(), board),
        };

        /// <summary>
        /// 方向を加味して移動可能な座標を計算する
        /// </summary>
        /// <returns></returns>
        /// <exception cref="System.NotImplementedException"></exception>
        private List<Point> CalculateMovablePoints() => Direction switch
        {
            PieceDirection.Up => SpecificMovablePoints.Select(p => new Point(Position.X + p.X, Position.Y + p.Y)).ToList(),
            PieceDirection.Down => SpecificMovablePoints.Select(p => new Point(Position.X - p.X, Position.Y - p.Y)).ToList(),
            _ => throw new System.NotImplementedException()
        };

        /// <summary>
        /// ターンが変わった時の処理
        /// </summary>
        public void OnTurnChanged()
        {
            // ターンエンド時に効果を発揮するステータスを処理する
            StatusList.Where(s => s is ITurnChangeListener).Cast<ITurnChangeListener>().ToList().ForEach(s => s.OnTurnEnd());
            OnTurnChangedHook();
        }

        /// <summary>
        /// 派生クラスで実装する移動可能な座標
        /// </summary>
        /// <returns></returns>
        protected abstract List<Point> SpecificMovablePoints { get; }

        /// <summary>
        /// 実際にその座標に移動可能か判定する
        /// 派生クラスでオーバーライドすることで特殊な移動を実装可能
        /// </summary>
        /// <param name="idealMovablePoints"></param>
        /// <param name="board"></param>
        /// <returns></returns>
        protected virtual List<Point> JudgeMovablePoints(List<Point> idealMovablePoints, BoardModel board) =>
            idealMovablePoints.Where(p => !board.IsPositionOutsideBounds(p)).ToList();

        /// <summary>
        /// ターンが変わった時に追加で実行される処理
        /// 派生クラスでオーバーライドすることで特殊な処理を実装可能
        /// </summary>
        public virtual void OnTurnChangedHook() { }

        /// <summary>
        /// 指定した座標に移動を試みる
        /// 移動できなかった場合はfalseを返す
        /// </summary>
        /// <param name="point"></param>
        /// <param name="board"></param>
        /// <returns></returns>
        public virtual bool TryMove(Point point, BoardModel board)
        {
            if (!GetMovablePoints(board).Contains(point))
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

