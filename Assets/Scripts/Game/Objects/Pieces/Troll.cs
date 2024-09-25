using System.Collections.Generic;

namespace NoMoney.Assets.Scripts.Game.Objects.Pieces
{
    /// <summary>
    /// トロールの駒
    /// 2*2のマスを占有する
    /// </summary>
    public class Troll : Piece, IAbnormalShape
    {
        //サイズが2*2で、左上を始点として(+1,0),(+1,+1),(0,+1)の3つのマスを占有する
        private static readonly List<BoardPoint> POSITIONS = new()
        {
            new(0, 0),
            new(1, 0),
            new(1, 1),
            new(0, 1)
        };

        public Troll(BoardPoint position, PieceSide side, IEnumerable<PieceStatus> statusList = null) : base(position, side, statusList)
        {
        }

        protected override List<BoardPoint> MoveRange
        {
            get
            {
                var points = new List<BoardPoint>
                //前後左右に進める
                {
                    new(0, -1),
                    new(0, 1),
                    new(-1, 0),
                    new(1, 0)
                };

                return points;
            }
        }

        public List<BoardPoint> ExtraPositions => POSITIONS;
    }
}