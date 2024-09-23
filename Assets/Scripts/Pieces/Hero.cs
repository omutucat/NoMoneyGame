using System.Collections.Generic;
using Unity.VisualScripting;

namespace NoMoney.Assets.Scripts.Pieces
{
    /// <summary>
    /// 勇者の駒
    /// 5ターン目まで動けない
    /// 盤面の端に到達すると、反対側の端にワープする
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
    }
}
