using System.Collections.Generic;

namespace NoMoney.Assets.Scripts.Pieces
{
    /// <summary>
    /// トロールの駒
    /// 2×2のマスを占有する
    /// </summary>
    public class Troll : PieceBase, IAbnormalShape
    {
        //サイズが2×2で、左上を始点として(+1,0),(+1,+1),(0,+1)の3つのマスを占有する
        private static readonly List<Point> POSITIONS = new()
        {
            new(0, 0),
            new(1, 0),
            new(1, 1),
            new(0, 1)
        };

        public Troll(Point position) : base(position)
        {
        }

        protected override List<Point> SpecificMovablePoints
        {
            get
            {
                var points = new List<Point>
                //前後左右に進める
                {
                    new(Position.X, Position.Y + 1),
                    new(Position.X, Position.Y - 1),
                    new(Position.X - 1, Position.Y),
                    new(Position.X + 1, Position.Y)
                };

                return points;
            }
        }

        public List<Point> ExtraPositions => POSITIONS;
    }
}