namespace NoMoney.Assets.Scripts.Pages.Game
{
    public class Turn
    {
        //初期化時にturn = 0
        private int turn = 0;
        public bool IsUserTurn() { return turn % 2 == 0; }

        public void OnTurnEnd()
        {
            turn++;
        }
    }
}