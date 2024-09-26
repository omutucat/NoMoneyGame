using System.Collections.Generic;
using NoMoney.Assets.Scripts.Game.Board;

namespace NoMoney.Assets.Scripts.Game.Objects.Pieces
{
    public interface IExtraMove
    {
        List<BoardPoint> GetSpecificReachablePoint(BoardModel board);
    }
}