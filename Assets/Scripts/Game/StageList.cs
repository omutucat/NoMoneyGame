using System.Collections.Generic;
using System.Linq;
using NoMoney.Assets.Scripts.Game.Objects;
using NoMoney.Assets.Scripts.Game.Objects.Pieces;

namespace NoMoney.Assets.Scripts.Game.Board
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
                    new Pawn(new BoardPoint(0, 0), PieceSide.Enemy),
                    new Pawn(new BoardPoint(1, 0), PieceSide.Enemy),
                    new Pawn(new BoardPoint(2, 0), PieceSide.Enemy),
                    new Pawn(new BoardPoint(3, 0), PieceSide.Enemy),
                    new Pawn(new BoardPoint(4, 0), PieceSide.Enemy),
                    new Pawn(new BoardPoint(5, 0), PieceSide.Enemy),
                    new Pawn(new BoardPoint(6, 0), PieceSide.Enemy),
                    new Pawn(new BoardPoint(7, 0), PieceSide.Enemy),
                    new Pawn(new BoardPoint(8, 0), PieceSide.Enemy),
                    new Hero(new BoardPoint(1,1),PieceSide.Enemy),
                    new Troll(new BoardPoint(6,6),PieceSide.Player),
                    new Ghost(new BoardPoint(8,7),PieceSide.Player),
                    new Pawn(new BoardPoint(0, 8), PieceSide.Player),
                    new Pawn(new BoardPoint(1, 8), PieceSide.Player),
                    new Pawn(new BoardPoint(2, 8), PieceSide.Player),
                    new Pawn(new BoardPoint(3, 8), PieceSide.Player),
                    new Pawn(new BoardPoint(4, 8), PieceSide.Player),
                    new Pawn(new BoardPoint(5, 8), PieceSide.Player),
                    new Pawn(new BoardPoint(6, 8), PieceSide.Player),
                    new Pawn(new BoardPoint(7, 8), PieceSide.Player),
                    new Pawn(new BoardPoint(8, 8), PieceSide.Player),
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