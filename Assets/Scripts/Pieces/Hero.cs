using System.Collections.Generic;

namespace NoMoney.Assets.Scripts.Pieces
{
    /// <summary>
    /// 勇者の駒
    /// 5ターン目まで動けない
    /// 盤面の端に到達すると、反対側の端にワープする
    /// </summary>
    public class Hero : PieceBase, ITeleportable
    {
        private int _TurnCount = 0;
        private const int IMMOBILE_TURNS = 5;

        public Hero(Point position) : base(position, new List<PieceStatus> { PieceStatus.Immobilized })
        {
        }

        protected override List<Point> SpecificMovablePoints
        {
            get
            {
                if (_TurnCount < IMMOBILE_TURNS)
                {
                    return new List<Point>(); // 動けないので空のリストを返す
                }
                else
                {
                    // 8方向全てに進める
                    return new List<Point>
                    {
                        new(Position.X + 1, Position.Y),
                        new(Position.X - 1, Position.Y),
                        new(Position.X, Position.Y + 1),
                        new(Position.X, Position.Y - 1),
                        new(Position.X + 1, Position.Y + 1),
                        new(Position.X + 1, Position.Y - 1),
                        new(Position.X - 1, Position.Y + 1),
                        new(Position.X - 1, Position.Y - 1),
                    };
                }
            }
        }

        public override void OnTurnEnd()
        {
            if (_TurnCount < IMMOBILE_TURNS)
            {
                _TurnCount++;
                StatusList.Remove(PieceStatus.Immobilized);
            }
        }
    }
}
