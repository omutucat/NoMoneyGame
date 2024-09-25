using System.Linq;
using NoMoney.Assets.Scripts.Game.Objects.Pieces;
using UnityEngine;

namespace NoMoney.Assets.Scripts.Game.GameManager
{
    public partial class ComponentGameManager
    {
        private class SelectState : IGameState
        {
            private ComponentGameManager _Manager;

            public SelectState(ComponentGameManager manager) => _Manager = manager;

            public IGameState Update() => this;

            public IGameState OnClick(Point point)
            {
                Debug.Log("SelectState OnClick triggered at " + point.ToDebugString());

                // 駒が無ければ何もしない
                if (_Manager.Board.GetObjectsAt(point).FirstOrDefault(o => o is Piece) is not Piece obj)
                {
                    Debug.Log("No piece found at " + point.ToDebugString());
                    return this;
                }

                // 敵の駒を押したときは何もしない
                if (obj.Side != _Manager.Turn.TurnPlayer)
                {
                    Debug.Log("SelectState OnClick triggered at " + point.ToDebugString());
                    _Manager._MessageText.text = "It's not yours";
                    return this;
                }

                _Manager.SelectedPiece = obj;

                _Manager._MessageText.text = "Select a square or piece";

                return new MoveState(_Manager);
            }
        }
    }

}
