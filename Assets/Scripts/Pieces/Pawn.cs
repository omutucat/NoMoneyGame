using System.Collections.Generic;

namespace NoMoney.Assets.Scripts.Pieces
{
    /// <summary>
    /// ポーンの駒
    /// </summary>
    public class Pawn : PieceBase
    {
        public Pawn(Point position) : base(position)
        {
        }

        protected override List<Point> SpecificMovablePoints
        {
            get
            {
                var points = new List<Point>
                //前三方向に進める
                {
                    new(Position.X, Position.Y + 1),
                    new(Position.X-1, Position.Y + 1),
                    new(Position.X+1, Position.Y + 1)
                };
                return points;
            }
        }
    }
}