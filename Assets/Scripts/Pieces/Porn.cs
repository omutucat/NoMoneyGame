using System.Collections.Generic;

namespace NoMoney.Assets.Scripts.Pieces
{
    public class Porn : PieceBase
    {
        public Porn(Point position) : base(PieceType.Porn, position)
        {
            
        }

        public override List<Point> MoveablePoints
        {
            get
            {
                var points = new List<Point>
                //前三方向に進める
                {
                    new Point(Position.X, Position.Y + 1),
                    new Point(Position.X-1, Position.Y + 1),
                    new Point(Position.X+1, Position.Y + 1)
                };

                return points;
            }
        }
    }
}