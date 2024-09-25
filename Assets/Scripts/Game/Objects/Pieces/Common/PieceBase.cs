using System.Collections.Generic;
using System.Linq;
using NoMoney.Assets.Scripts.Game.Board;

namespace NoMoney.Assets.Scripts.Game.Objects.Pieces
{
    /// <summary>
    /// 駒
    /// </summary>
    public abstract class Piece : BoardObject
    {
        /// <summary>
        /// 向き
        /// </summary>
        public PieceDirection Direction { get; protected set; }

        /// <summary>
        /// 所属
        /// </summary>
        public PieceSide Side { get; protected set; }

        /// <summary>
        /// 移動範囲
        /// </summary>
        /// <returns></returns>
        protected abstract List<BoardPoint> MoveRange { get; }

        /// <summary>
        /// 状態(バフ・デバフなど)のリスト
        /// </summary>
        public List<PieceStatus> StatusList { get; private set; }

        protected Piece(BoardPoint position, PieceSide side, IEnumerable<PieceStatus>? statusList = null) : base(position)
        {
            Side = side;
            StatusList = statusList?.ToList() ?? new List<PieceStatus>();
            Direction = side switch
            {
                // NOTE: 向きは所属によって設定する
                PieceSide.Player => PieceDirection.Up,
                PieceSide.Enemy => PieceDirection.Down,
                _ => throw new System.NotImplementedException()
            };
        }

        /// <summary>
        /// ステータスを追加する
        /// </summary>
        /// <param name="status"></param>
        public void AddStatus(PieceStatus status)
        {
            if (!StatusList.Contains(status))
            {
                // NOTE: 同じステータスは重複しない
                StatusList.Add(status);

                // NOTE: ステータスが解消された時に自動でリストから削除されるよう設定する
                status.OnRemove += (status) => StatusList.Remove(status);
            }
        }

        /// <summary>
        /// 指定した座標に移動を試みる
        /// 移動できなかった場合はfalseを返す
        /// </summary>
        /// <param name="point"></param>
        /// <param name="board"></param>
        /// <returns></returns>
        public virtual bool TryMove(BoardPoint point, BoardModel board)
        {
            // TODO: 移動結果を単なるBool型でなくクラス化し、色んな情報を返せるようにしたい
            if (!GetMovablePoints(board).Contains(point))
            {
                return false;
            }

            var objects = board.GetObjectsAt(point);

            // TODO: 攻撃時や被攻撃時の処理をより抽象化したい
            // Unbreakableなオブジェクトをここで判定するのではなく、相手の駒の判定関数を使用できるようにしたり
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
            Position = point;
            return true;
        }

        /// <summary>
        /// 指定したピースが移動可能な座標を返す
        /// </summary>
        /// <param name="piece"></param>
        /// <returns></returns>
        public List<BoardPoint> GetMovablePoints(BoardModel board) => StatusList switch
        {
            // TODO: 移動不可の判定をここではなく、ステータス側で行うようにしたい
            { } s when s.Any(s => s is InSleep or Immobilized) => new List<BoardPoint>(),
            _ => JudgeMovablePoints(GetReachablePoints(), board),
        };

        /// <summary>
        /// 方向を加味して移動可能な座標を計算する
        /// </summary>
        /// <returns></returns>
        /// <exception cref="System.NotImplementedException"></exception>
        private List<BoardPoint> GetReachablePoints() => Direction switch
        {
            PieceDirection.Up => MoveRange.Select(p => new BoardPoint(Position.X + p.X, Position.Y + p.Y)).ToList(),
            PieceDirection.Down => MoveRange.Select(p => new BoardPoint(Position.X - p.X, Position.Y - p.Y)).ToList(),
            _ => throw new System.NotImplementedException()
        };

        /// <summary>
        /// ターンが変わった時の処理
        /// </summary>
        public void OnTurnChanged()
        {
            StatusList.Where(s => s is ITurnChangeListener).Cast<ITurnChangeListener>().ToList().ForEach(s => s.OnTurnEnd());

            // NOTE: hookを派生クラスでオーバーライドすることで追加の処理を実行可能
            OnTurnChangedHook();
        }

        /// <summary>
        /// 実際にその座標に移動可能か判定する
        /// 派生クラスでオーバーライドすることで特殊な移動を実装可能
        /// </summary>
        /// <param name="reachablePoints"></param>
        /// <param name="board"></param>
        /// <returns></returns>
        protected virtual List<BoardPoint> JudgeMovablePoints(List<BoardPoint> reachablePoints, BoardModel board) =>
            reachablePoints.Where(p => !board.IsPositionOutsideBounds(p)).ToList();

        /// <summary>
        /// ターンが変わった時に追加で実行される処理
        /// 派生クラスでオーバーライドすることで特殊な処理を実装可能
        /// </summary>
        public virtual void OnTurnChangedHook() { }

    }

    /// <summary>
    /// 駒の向き
    /// </summary>
    public enum PieceDirection
    {
        Up,
        Down,
        // Left,
        // Right
    }

    /// <summary>
    /// 駒の所属
    /// </summary>
    public enum PieceSide
    {
        // TODO: 所属の情報ってここじゃないところに書くべきかも
        Player,
        Enemy
    }
}