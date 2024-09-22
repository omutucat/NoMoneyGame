using System.Collections.Generic;

namespace NoMoney.Assets.Scripts.Pieces
{
    /// <summary>
    /// 幽霊駒
    /// 他の駒に重なることが出来、重なっている駒を行動不能にする
    /// </summary>
    public class Ghost : PieceBase, IGhost
    {
        public Ghost(Point position) : base(position)
        {
        }

        protected override List<Point> SpecificMovablePoints
        {
            get
            {
                var points = new List<Point>
                    // 前三方向と、前2マス先に進める
                    // 0 x 0
                    // x x x
                    // 0 m 0
                    // 0 0 0
                    // x: 進める位置
                    // m: 駒の位置
                    {
                        new(Position.X, Position.Y + 1),
                        new(Position.X - 1, Position.Y + 1),
                        new(Position.X + 1, Position.Y + 1),
                        new(Position.X, Position.Y + 2)
                    };
                return points;
            }
        }
    }
}