using System.Collections.Generic;

namespace NoMoney.Assets.Scripts.Game.Objects.Pieces
{
    /// <summary>
    /// トロールの駒
    /// 2*2のマスを占有する
    /// </summary>
    public class Troll : Piece, IAbnormalShape
    {
        public Troll(BoardPoint position, PieceSide side, IEnumerable<PieceStatus> statusList = null) : base(position, side, statusList)
        {
        }

        protected override List<BoardPoint> MoveRange => new()
        {
            // 前後左右に進める
            new(0, -1),
            new(0, 1),
            new(-1, 0),
            new(1, 0)
        };

        public List<BoardPoint> ExtraPositions => new()
        {
            // NOTE: サイズが2*2で、左上を始点として(+1,0),(+1,+1),(0,+1)の3つのマスを占有する
            new(0, 0),
            new(1, 0),
            new(1, 1),
            new(0, 1)
        };
    }
}