using System.Collections.Generic;
using System.Linq;
using NoMoney.Assets.Scripts.Game.Board;

namespace NoMoney.Assets.Scripts.Game.Objects.Pieces
{
    /// <summary>
    /// 勇者の駒
    /// 5ターン目まで動けない
    /// 盤面の左右端に到達すると、反対側の端にワープする
    /// </summary>
    public class Hero : Piece, ITeleportable
    {
        private int _TurnCount = 0;
        private const int IMMOBILE_TURNS = 5;

        public Hero(Point position, PieceSide side, IEnumerable<PieceStatus> statusList = null) : base(position, side, statusList)
        {
            AddStatus(new InSleep(IMMOBILE_TURNS));
        }

        protected override List<Point> SpecificMovablePoints =>
            // 8方向全てに進める
            new()
            {
                new(1, 0),
                new(-1, 0),
                new(0, 1),
                new(0, -1),
                new(1, 1),
                new(1, -1),
                new(-1, 1),
                new(-1, -1),
            };

        protected override List<Point> JudgeMovablePoints(List<Point> idealMovablePoints, BoardModel board) =>
            idealMovablePoints.Select(p =>
                {
                    var x = (p.X + board.Size.Width) % board.Size.Width;
                    var y = p.Y;
                    return new Point(x, y);
                }
            ).ToList();
    }
}