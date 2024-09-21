using System.Collections.Generic;

namespace NoMoney.Assets.Scripts.Pieces
{
    public class Troll : PieceBase
    {
        //サイズが2×2で、左上を始点として(+1,0),(+1,+1),(0,+1)の3つのマスを占有する
        private static readonly List<Point> POSITIONS = new List<Point>
        {
            new Point(0, 0),
            new Point(1, 0),
            new Point(1, 1),
            new Point(0, 1)
        };
        public Troll(Point position) : base(PieceType.Troll, position, new Abnormal(POSITIONS))
        {
            
        }

        public override List<Point> MoveablePoints
        {
            get
            {
                var points = new List<Point>
                //前後左右に進める
                {
                    new Point(Position.X, Position.Y + 1),
                    new Point(Position.X, Position.Y - 1),
                    new Point(Position.X - 1, Position.Y),
                    new Point(Position.X + 1, Position.Y)
                };

                return points;
            }
        }
    }
}