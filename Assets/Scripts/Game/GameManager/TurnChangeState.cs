using NoMoney.Assets.Scripts.Game.Objects.Pieces;

namespace NoMoney.Assets.Scripts.Game.GameManager
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
                return new SelectState(_Manager);
            }

            public IGameState OnClick(Point point) => this;
        }

    }
}