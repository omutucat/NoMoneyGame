using System.Collections.Generic;
using NoMoney.Assets.Scripts.Game.Board;

namespace NoMoney.Assets.Scripts.Game.Objects.Pieces
{
    public abstract class PieceStatus
    {
        public delegate void RemoveStatusEventHandler(PieceStatus sender);

        public event RemoveStatusEventHandler OnRemove;

        // NOTE: ステータスの解消時にイベントが呼ばれる
        public void Remove() => OnRemove?.Invoke(this);
    }

    /// <summary>
    /// 移動不可状態
    /// </summary>
    public class Immobilized : PieceStatus, IMoveEffect
    {
        public List<BoardPoint> GetAffectedReachablePoint(Piece piece, BoardModel board) => new();
    }

    /// <summary>
    /// 眠り状態
    /// </summary>
    public class InSleep : PieceStatus, ITurnChangeEffect, IMoveEffect
    {
        /// <summary>
        /// 残りターン数
        /// </summary>
        public int TurnCount { get; private set; }

        public InSleep(int turnCount) => TurnCount = turnCount;

        /// <summary>
        /// 睡眠残りターン数を減らす
        /// </summary>
        public void DecreaseTurnCount()
        {
            TurnCount--;
            if (TurnCount == 0)
            {
                Remove();
            }
        }

        public void OnTurnEnd() => DecreaseTurnCount();
        public List<BoardPoint> GetAffectedReachablePoint(Piece piece, BoardModel board) => new();
    }

    public class Untouchable : PieceStatus
    {
    }

    public interface IMoveEffect
    {
        /// <summary>
        /// 効果適用後の移動可能な座標を返す
        /// </summary>
        /// <param name="piece"></param>
        /// <param name="board"></param>
        /// <returns></returns>
        List<BoardPoint> GetAffectedReachablePoint(Piece piece, BoardModel board);
    }

    /// <summary>
    /// ターン変更時の処理を受け取るインターフェース
    /// ターン変更時に効果を発揮するステータスはこのインターフェースを実装する
    /// </summary>
    public interface ITurnChangeEffect
    {
        void OnTurnEnd();
    }
}