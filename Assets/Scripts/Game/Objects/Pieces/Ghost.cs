using System.Collections.Generic;
using System.Linq;
using NoMoney.Assets.Scripts.Game.Board;

namespace NoMoney.Assets.Scripts.Game.Objects.Pieces
{
    /// <summary>
    /// 幽霊駒
    /// 他の駒に衝突すると、その駒をImmobilized状態にして自身は破壊される
    /// </summary>
    public class Ghost : Piece
    {
        public Ghost(BoardPoint position, PieceSide side, IEnumerable<PieceStatus> statusList = null) : base(position, side, statusList)
        {
        }

        protected override List<BoardPoint> MoveRange =>
        // 前三方向と、前2マス先に進める
        // 0 x 0
        // x x x
        // 0 m 0
        // 0 0 0
        // x: 進める位置
        // m: 駒の位置
        new()
        {
            new(0, -1),
            new(-1, -1),
            new(1, -1),
            new(0, -2)
        };

        protected override bool CanAttackTo(IAttackTarget target) =>
            // NOTE: 駒ならUntouchableでも攻撃できるが、駒以外のオブジェクトには攻撃できない
            target is Piece;

        protected override void AttackTo(BoardModel board, IAttackTarget target)
        {
            if (target is Piece piece)
            {
                piece.AddStatus(new Immobilized());
                Destroy();
            }
        }
    }
}