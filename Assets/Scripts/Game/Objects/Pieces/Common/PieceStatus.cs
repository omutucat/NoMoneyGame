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
    public class Immobilized : PieceStatus
    {
        // TODO: もっとうまい方法で移動不可状態を表現したいね
    }

    /// <summary>
    /// 眠り状態
    /// </summary>
    public class InSleep : PieceStatus, ITurnChangeListener
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
    }

    /// <summary>
    /// ターン変更時の処理を受け取るインターフェース
    /// ターン変更時に効果を発揮するステータスはこのインターフェースを実装する
    /// </summary>
    public interface ITurnChangeListener
    {
        void OnTurnEnd();
    }
}