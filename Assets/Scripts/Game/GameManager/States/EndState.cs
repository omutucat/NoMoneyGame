using NoMoney.Assets.Scripts.Game.Objects;

namespace NoMoney.Assets.Scripts.Game.GameManager
{
    public partial class ComponentGameManager
    {
        /// <summary>
        /// ゲーム終了状態
        /// </summary>
        private class EndState : IGameState
        {
            private ComponentGameManager _Manager;

            public EndState(ComponentGameManager manager) => _Manager = manager;

            public IGameState Update()
            {
                _Manager.ToNextScene();
                return this;
            }

            public IGameState OnClick(BoardPoint point) => this;
        }
    }
}
