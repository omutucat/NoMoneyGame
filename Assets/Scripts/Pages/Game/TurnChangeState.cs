using NoMoney.Assets.Scripts.Pieces;

namespace NoMoney.Assets.Scripts.Pages.Game
{
    public partial class ComponentGameManager
    {
        private class TurnChangeState : IGameState
        {
            private ComponentGameManager _Manager;

            public TurnChangeState(ComponentGameManager manager) => _Manager = manager;

            public IGameState Update()
            {
                _Manager.Turn.ToNextPlayer();
                _Manager._TurnText.text = _Manager.Turn.Count.ToString();
                return new SelectState(_Manager);
            }

            public IGameState OnClick(Point point) => this;
        }

    }
}