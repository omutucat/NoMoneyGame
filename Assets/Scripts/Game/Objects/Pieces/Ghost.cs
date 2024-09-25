using System.Collections.Generic;
using NoMoney.Assets.Scripts.Game.Board;
using System.Linq;

namespace NoMoney.Assets.Scripts.Game.Objects.Pieces
{
    /// <summary>
    /// 幽霊駒
    /// 他の駒に衝突すると、その駒をImmobilized状態にして自身は破壊される
    /// </summary>
    public class Ghost : Piece
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

        public override bool TryMove(Point point, BoardModel board)
        {
            if (!GetMovablePoints(board).Contains(point))
            {
                return false;
            }

            var objectsInPoint = board.GetObjectsAt(point);

            switch (objectsInPoint)
            {
                case { } when objectsInPoint.Count == 0:
                    SetPosition(point);
                    break;
                case { } when objectsInPoint.Any(o => o is Piece piece && piece.Side != Side):
                    objectsInPoint.ForEach(o =>
                    {
                        if (o is Piece piece)
                        {
                            piece.StatusList.Add(new Immobilized());
                        }
                    });
                    Destroy();
                    break;
                default:
                    return false;
            }

            return true;
        }
    }
}