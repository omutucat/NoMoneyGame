using System.Collections.Generic;

namespace NoMoney.Assets.Scripts.Pieces
{
    public class Ghost : PieceBase
    {
        public Ghost(Point position) : base(PieceType.Ghost, position,
            new PieceAttribute[]
            {
                new GhostStateAttribute()
            })
        {

        }

        public override List<Point> MoveablePoints
        {
            get
            {
                var points = new List<Point>
                    //前三方向と、前2マス先に進める
                    {
                        new Point(Position.X, Position.Y + 1),
                        new Point(Position.X-1, Position.Y + 1),
                        new Point(Position.X+1, Position.Y + 1),
                        new Point(Position.X, Position.Y + 2)
                    };
                return points;
            }
        }
        
        public override void OnTurnEnd()
        {
            
        }
    }
}