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

            var canMove = objects switch
            {
                { } o when o.Any(o => o.IsUntouchable) => false,
                { } o when o.Any(o => o is Piece p && p.Side == Side) => false,
                _ => true
            };

            if (!canMove)
            {
                return false;
            }

            objects.ForEach(o => o.OnCollided(board, this));
            Position = point;
            return true;
        }

        public List<BoardPoint> GetMovablePoints(BoardModel board) => this switch
        {
            { } p when p.StatusList.Any(s => s is IMoveEffect) =>
                (p.StatusList.First(s => s is IMoveEffect) as IMoveEffect).GetAffectedReachablePoint(p, board),
            IExtraMove p => p.GetSpecificReachablePoint(board),
            _ => GetDefaultReachablePoints(board)
        };

        /// <summary>
        /// 方向を加味して移動可能な座標を計算する
        /// </summary>
        /// <returns></returns>
        /// <exception cref="System.NotImplementedException"></exception>
        public List<BoardPoint> GetDefaultReachablePoints(BoardModel board) =>
            MoveRange.Select(p => Direction switch
                {
                    PieceDirection.Up => new BoardPoint(Position.X + p.X, Position.Y + p.Y),
                    PieceDirection.Down => new BoardPoint(Position.X - p.X, Position.Y - p.Y),
                    _ => throw new System.NotImplementedException()
                }
            ).Where(p => board.IsInsidePoint(p)).ToList();

        public override bool IsUntouchable => false || StatusList.Any(s => s is Untouchable) || this is IUntouchable;

        /// <summary>
        /// ターンが変わった時の処理
        /// </summary>
        public void OnTurnChanged()
        {
            StatusList.Where(s => s is ITurnChangeEffect).Cast<ITurnChangeEffect>().ToList().ForEach(s => s.OnTurnEnd());

            // NOTE: hookを派生クラスでオーバーライドすることで追加の処理を実行可能
            OnTurnChangedHook();
        }

        /// <summary>
        /// ターンが変わった時に追加で実行される処理
        /// 派生クラスでオーバーライドすることで特殊な処理を実装可能
        /// </summary>
        protected virtual void OnTurnChangedHook() { }
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