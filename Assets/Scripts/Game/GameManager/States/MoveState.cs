
using System.Linq;
using NoMoney.Assets.Scripts.Game.Objects.Pieces;
using UnityEngine;

namespace NoMoney.Assets.Scripts.Game.GameManager
{
    public partial class ComponentGameManager
    {

        private class MoveState : IGameState
        {
            private ComponentGameManager _Manager;

            public MoveState(ComponentGameManager manager) => _Manager = manager;

            public IGameState Update() => this;

            public IGameState OnClick(Point point)
            {
                Debug.Log("MoveState OnClick triggered at " + point.ToDebugString());

                var clickedPiece = _Manager.Board.GetObjectsAt(point).FirstOrDefault(o => o is Piece) as Piece;

                if (clickedPiece is not null && clickedPiece.Side == _Manager.Turn.TurnPlayer)
                {
                    _Manager.SelectedPiece = clickedPiece;
                    return this;
                }

                switch (_Manager.Board.TryMovePiece(_Manager.SelectedPiece, point))
                {
                    case true:
                        _Manager.SelectedPiece = null;
                        _Manager._MessageText.text = "";
                        return new CalcState(_Manager);
                    case false:
                        _Manager._MessageText.text = "Invalid move";
                        return this;
                }
            }
        }

    }
}
