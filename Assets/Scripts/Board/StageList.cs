using System.Collections.Generic;
using NoMoney.Assets.Scripts.Pieces;
using System.Linq;
using Unity.VisualScripting;

namespace NoMoney.Assets.Scripts.Board
{
    public enum StageName
    {
        Stage1,
    }

    public static class StageList
    {
        private static List<(StageName name, BoardModel board)> STAGES => new()
        {
            (StageName.Stage1, new BoardModel(
                size: new BoardSize(9, 9),
                objects: new List<BoardObject>
                {
                    new Pawn(new Point(0, 0), PieceSide.Enemy),
                    new Pawn(new Point(1, 0), PieceSide.Enemy),
                    new Pawn(new Point(2, 0), PieceSide.Enemy),
                    new Pawn(new Point(3, 0), PieceSide.Enemy),
                    new Pawn(new Point(4, 0), PieceSide.Enemy),
                    new Pawn(new Point(5, 0), PieceSide.Enemy),
                    new Pawn(new Point(6, 0), PieceSide.Enemy),
                    new Pawn(new Point(7, 0), PieceSide.Enemy),
                    new Pawn(new Point(8, 0), PieceSide.Enemy),
                    new Hero(new Point(1,1),PieceSide.Enemy),
                    new Troll(new Point(6,6),PieceSide.Player),
                    new Ghost(new Point(8,7),PieceSide.Player),
                    new Pawn(new Point(0, 8), PieceSide.Player),
                    new Pawn(new Point(1, 8), PieceSide.Player),
                    new Pawn(new Point(2, 8), PieceSide.Player),
                    new Pawn(new Point(3, 8), PieceSide.Player),
                    new Pawn(new Point(4, 8), PieceSide.Player),
                    new Pawn(new Point(5, 8), PieceSide.Player),
                    new Pawn(new Point(6, 8), PieceSide.Player),
                    new Pawn(new Point(7, 8), PieceSide.Player),
                    new Pawn(new Point(8, 8), PieceSide.Player),
                }
            ))
        };

        public static BoardModel? GetStage(StageName stageName)
        {
            var stage = STAGES.FirstOrDefault(s => s.name == stageName);
            return stage != default ? new BoardModel(stage.board.Size, stage.board.Objects) : null;
        }
    }
}
