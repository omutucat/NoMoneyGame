using System.Linq;
using NoMoney.Assets.Scripts.Game.Objects;
using NoMoney.Assets.Scripts.Game.Objects.Pieces;
using UnityEngine;

namespace NoMoney.Assets.Scripts.Game.GameManager
{
    public partial class ComponentGameManager
    {
        /// <summary>
        /// 駒の移動待ち状態
        /// </summary>
        private class MoveState : IGameState
        {
            private ComponentGameManager _Manager;

            public MoveState(ComponentGameManager manager) => _Manager = manager;

            public IGameState Update() => this;

            public IGameState OnClick(BoardPoint point)
            {
                Debug.Log("MoveState OnClick triggered at " + point.ToDebugString());

                // TODO: この形の書き方結構見かけるので、どこかで関数化する
                var clickedPiece = _Manager.Board.GetObjectsAt(point).FirstOrDefault(o => o is Piece) as Piece;

                if (clickedPiece is not null && clickedPiece.Side == _Manager.Turn.TurnPlayer)
                {
                    _Manager.SelectedPiece = clickedPiece;
                    return this;
                }

                switch (_Manager.SelectedPiece.TryMove(point, _Manager.Board))
                {
                    // TODO: TryMoveの返り値の表現力を高めたら、なんで移動できなかったのかをより詳しく区別出来そう
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