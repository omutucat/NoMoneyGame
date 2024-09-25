using NoMoney.Assets.Scripts.Game.Objects.Pieces;
using UnityEngine;

namespace NoMoney.Assets.Scripts.Game.GameManager
{
    public partial class ComponentGameManager
    {
        private class EndState : IGameState
        {
            private ComponentGameManager _Manager;

            public EndState(ComponentGameManager manager)
            {
                _Manager = manager;
            }

            public IGameState Update()
            {
                _Manager.MoveScene();
                return this;
            }

            public IGameState OnClick(Point point)
            {
                Debug.Log("EndState OnClick triggered at " + point.ToDebugString());
                return this;
            }
        }
    }
}