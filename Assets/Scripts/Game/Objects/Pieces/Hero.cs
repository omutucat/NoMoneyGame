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
    public class Hero : Piece
    {
        private int _TurnCount = 0;
        private const int IMMOBILE_TURNS = 5;

        public Hero(BoardPoint position, PieceSide side, IEnumerable<PieceStatus> statusList = null)
            : base(position, side, statusList) => AddStatus(new InSleep(IMMOBILE_TURNS));

        protected override List<BoardPoint> MoveRange =>
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

        protected override List<BoardPoint> JudgeMovablePoints(List<BoardPoint> reachablePoints, BoardModel board) =>
            //TODO : このタイプの移動、もっといい感じに共通化したい
            reachablePoints.Select(p =>
                {
                    var x = (p.X + board.Size.Width) % board.Size.Width;
                    var y = p.Y;
                    return new BoardPoint(x, y);
                }
            ).ToList();
    }
}