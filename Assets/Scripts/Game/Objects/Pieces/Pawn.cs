using System.Collections.Generic;

namespace NoMoney.Assets.Scripts.Game.Objects.Pieces
{
    /// <summary>
    /// ポーンの駒
    /// </summary>
    public class Pawn : Piece
    {
        public Pawn(BoardPoint position, PieceSide side, IEnumerable<PieceStatus> statusList = null)
            : base(position, side, statusList) { }

        protected override List<BoardPoint> MoveRange =>
            new()
            // 前三方向に進める
            {
                new(0, -1),
                new(-1, -1),
                new(1, -1),
            };
    }
}
