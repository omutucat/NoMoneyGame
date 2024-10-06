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
    public class Hero : Piece, IExtraMove
    {
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

        public List<BoardPoint> GetSpecificReachablePoint(BoardModel board) =>
            MoveRange
                .Select(p =>
                    Direction switch
                    {
                        PieceDirection.Up => new BoardPoint(
                            (Position.X + p.X + board.Size.Width) % board.Size.Width,
                            Position.Y + p.Y
                        ),
                        PieceDirection.Down => new BoardPoint(
                            (Position.X - p.X + board.Size.Width) % board.Size.Width,
                            Position.Y - p.Y
                        ),
                        _ => throw new System.NotImplementedException(),
                    }
                )
                .Where(p => board.IsInsidePoint(p))
                .ToList();
    }
}
