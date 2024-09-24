using System.Collections.Generic;
using NoMoney.Assets.Scripts.Pieces;

namespace NoMoney.Assets.Scripts.Pages.Game
{
    public class Turn
    {
        public int Count = 1;

        public delegate void TurnChangedHandler(Turn turn);

        public event TurnChangedHandler OnTurnChanged;

        public PieceSide TurnPlayer { get; private set; } = PieceSide.Player;

        public void ToNextPlayer()
        {
            // 敵の行動が終わればターンを進める
            TurnPlayer = TurnPlayer == PieceSide.Player ? PieceSide.Enemy : PieceSide.Player;
            Count += TurnPlayer == PieceSide.Player ? 1 : 0;
            OnTurnChanged?.Invoke(this);
        }
    }
}