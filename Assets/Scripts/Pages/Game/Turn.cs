using NoMoney.Assets.Scripts.Pieces;

namespace NoMoney.Assets.Scripts.Pages.Game
{
    public class Turn
    {
        //初期化時にturn = 0
        private int _Turn = 0;
        private bool IsUserTurn() => _Turn % 2 == 0;

        // IsUserTurnならPieceSide.Player else Enemy
        public PieceSide GameSide() => IsUserTurn() ? PieceSide.Player : PieceSide.Enemy;

        public void OnTurnEnd() => _Turn++;
    }
}