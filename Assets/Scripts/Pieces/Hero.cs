using System.Collections.Generic;

namespace NoMoney.Assets.Scripts.Pieces
{
    public class Hero : PieceBase
    {
        private int _turnCount = 0;
        private const int ImmobileTurns = 5;

        
        public Hero(Point position) : base(PieceType.Hero, position)
        {

        }

        public override List<Point> MoveablePoints
        {
            get
            {
                if (_turnCount < ImmobileTurns)
                {
                    return new List<Point>(); // 動けないので空のリストを返す
                }
                else
                {
                    // 8方向全てに進める
                    return new List<Point>
                    {
                        new Point(Position.X + 1, Position.Y),
                        new Point(Position.X - 1, Position.Y),
                        new Point(Position.X, Position.Y + 1),
                        new Point(Position.X, Position.Y - 1),
                        new Point(Position.X + 1, Position.Y + 1),
                        new Point(Position.X + 1, Position.Y - 1),
                        new Point(Position.X - 1, Position.Y + 1),
                        new Point(Position.X - 1, Position.Y - 1),
                    };
                }
            }
        }

        public override void OnTurnEnd()
        {
            _turnCount++;
        }
        
    }
}
