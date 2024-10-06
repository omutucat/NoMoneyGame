using NoMoney.Assets.Scripts.Game.Objects.Pieces;

namespace NoMoney.Assets.Scripts.Game.GameManager
{
    public class Turn
    {
        /// <summary>
        /// ターンが変わった時に発火する処理のシグネチャ
        /// </summary>
        /// <param name="turn"></param>
        public delegate void TurnChangedHandler(Turn turn);

        /// <summary>
        /// ターンが変わった時のイベント
        /// </summary>
        public event TurnChangedHandler OnTurnChanged;

        /// <summary>
        /// 経過ターン数
        /// </summary>
        public int Count { get; private set; } = 1;

        /// <summary>
        /// ターンプレイヤー
        /// </summary>
        public PieceSide TurnPlayer { get; private set; } = PieceSide.Player;

        /// <summary>
        /// 次のプレイヤーにターンを渡す
        /// </summary>
        public void ToNextPlayer()
        {
            // NOTE: 両プレイヤーの処理が終わったらターンカウントが進む
            TurnPlayer = TurnPlayer == PieceSide.Player ? PieceSide.Enemy : PieceSide.Player;
            Count += TurnPlayer == PieceSide.Player ? 1 : 0;
            OnTurnChanged?.Invoke(this);
        }
    }
}
