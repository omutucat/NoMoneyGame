using System.Collections.Generic;

namespace NoMoney.Assets.Scripts.Pieces
{
    /// <summary>
    /// ポーンの駒
    /// </summary>
    public class Pawn : Piece
    {
        public Pawn(Point position) : base(position)
        {
        }

        protected override List<Point> SpecificMovablePoints => new()
        //前三方向に進める
        {
            new(0, 1),
            new(-1, 1),
            new(1, 1)
        };
    }
}