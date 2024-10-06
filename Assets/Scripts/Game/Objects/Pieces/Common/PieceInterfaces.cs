using System.Collections.Generic;
using NoMoney.Assets.Scripts.Game.Board;

namespace NoMoney.Assets.Scripts.Game.Objects.Pieces
{
    /// <summary>
    /// 特殊な移動を行う駒のインターフェース
    /// </summary>
    public interface IExtraMove
    {
        /// <summary>
        /// 特殊な移動を行う駒の移動可能なマスを取得する
        /// </summary>
        /// <param name="board"></param>
        /// <returns></returns>
        List<BoardPoint> GetSpecificReachablePoint(BoardModel board);
    }
}
