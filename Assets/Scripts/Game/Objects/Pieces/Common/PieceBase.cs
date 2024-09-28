using System.Collections.Generic;
using System.Linq;
using NoMoney.Assets.Scripts.Game.Board;

namespace NoMoney.Assets.Scripts.Game.Objects.Pieces
{
    /// <summary>
    /// 駒
    /// </summary>
    public abstract class Piece : BoardObject, IAttackTarget
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

        public bool IsTouchable =>
            // NOTE: Untouchableになることはあるが、Untouchableでなくなる事はないとする。
            true && !StatusList.Any(s => s is Untouchable) && this is not IUntouchable;

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
        public bool TryMove(BoardPoint point, BoardModel board)
        {
            // TODO: 移動結果を単なるBool型でなくクラス化し、色んな情報を返せるようにしたい
            if (!GetReachablePoints(board).Contains(point))
            {
                return false;
            }

            var attackTarget = board.GetObjectsAt(point).FirstOrDefault(o => o is IAttackTarget) as IAttackTarget;

            switch (attackTarget)
            {
                case null:
                    Position = point;
                    return true;
                case { } when CanAttackTo(attackTarget):
                    AttackTo(board, attackTarget);
                    Position = point;
                    return true;
                default:
                    return false;
            }
        }

        protected virtual bool CanAttackTo(IAttackTarget target) => target.IsTouchable;

        protected virtual void AttackTo(BoardModel board, IAttackTarget target) => target.OnAttacked(board, this);

        /// <summary>
        /// 到達可能な座標を取得する
        /// </summary>
        /// <param name="board"></param>
        /// <returns></returns>
        public List<BoardPoint> GetReachablePoints(BoardModel board) => this switch
        {
            { } p when p.StatusList.Any(s => s is IMoveEffect) =>
                (p.StatusList.First(s => s is IMoveEffect) as IMoveEffect).GetAffectedReachablePoint(p, board),
            IExtraMove p => p.GetSpecificReachablePoint(board),
            _ => GetDefaultReachablePoints(board)
        };

        /// <summary>
        /// 向きや盤面の境界を考慮した移動可能な座標を取得する
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

        public void OnAttacked(BoardModel board, Piece attacker)
        {
            // NOTE: 攻撃されると少なくとも必ず破壊され、追加処理としてHookを呼び出す
            Destroy();
            OnAttackedHook(board, attacker);
        }

        protected virtual void OnAttackedHook(BoardModel board, BoardObject other) { }
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