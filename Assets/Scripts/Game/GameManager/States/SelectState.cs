using System.Linq;
using NoMoney.Assets.Scripts.Game.Objects;
using NoMoney.Assets.Scripts.Game.Objects.Pieces;
using UnityEngine;

namespace NoMoney.Assets.Scripts.Game.GameManager
{
    public partial class ComponentGameManager
    {
        /// <summary>
        /// 駒の選択待ち状態
        /// </summary>
        private class SelectState : IGameState
        {
            private ComponentGameManager _Manager;

            public SelectState(ComponentGameManager manager) => _Manager = manager;

            public IGameState Update() => this;

            public IGameState OnClick(BoardPoint point)
            {
                Debug.Log("SelectState OnClick triggered at " + point.ToDebugString());


                if (_Manager.Board.GetObjectsAt(point).FirstOrDefault(o => o is Piece) is not Piece clickedPiece)
                {
                    return this;
                }

                if (clickedPiece.Side != _Manager.Turn.TurnPlayer)
                {
                    _Manager._MessageText.text = "It's not yours";
                    return this;
                }

                _Manager.SelectedPiece = clickedPiece;
                _Manager._MessageText.text = "Select a square or piece";

                return new MoveState(_Manager);
            }
        }
    }

}