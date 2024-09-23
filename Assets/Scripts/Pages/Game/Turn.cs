using NoMoney.Assets.Scripts.Pieces;

namespace NoMoney.Assets.Scripts.Pages.Game
{
    public class Turn
    {
        //初期化時にturn = 0
        private int turn = 0;
        private bool IsUserTurn() { return turn % 2 == 0; }

        // IsUserTurnならPieceSide.Player else Enemy
        public PieceSide GameSide()
        {
            if (IsUserTurn())
            {
                return PieceSide.Player;
            }
            else
            {
                return PieceSide.Enemy;
            }
        }

        public void OnTurnEnd()
        {
            turn++;
        }
    }
}