using System.Collections.Generic;

namespace NoMoney.Assets.Scripts.Pieces
{
    /// <summary>
    /// 幽霊駒
    /// 他の駒に重なることが出来、重なっている駒を行動不能にする
    /// </summary>
    public class Ghost : Piece, IGhost
    {
        public Ghost(Point position, PieceSide side, IEnumerable<PieceStatus> statusList = null) : base(position, side, statusList)
        {
        }

        protected override List<Point> SpecificMovablePoints =>
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
    }
}