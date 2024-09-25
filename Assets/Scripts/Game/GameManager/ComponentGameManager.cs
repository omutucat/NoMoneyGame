using System.Collections.Generic;
using NoMoney.Assets.Scripts.Board;
using NoMoney.Assets.Scripts.Pieces;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Point = NoMoney.Assets.Scripts.Pieces.Point;

namespace NoMoney.Assets.Scripts.Pages.Game
{
    public partial class ComponentGameManager : MonoBehaviour, IBoardEventListener
    {
        private IGameState CurrentState { get; set; }
        public BoardModel Board { get; private set; }
        [SerializeField] private ComponentBoardPanel _BoardPanel;
        [SerializeField] private Text _MessageText;
        [SerializeField] private ComponentTurn _TurnComponent;
        private Piece _SelectedPiece = null;
        public Turn Turn { get; private set; }

        private Piece SelectedPiece
        {
            get => _SelectedPiece;
            set
            {
                // 駒が選択された時に移動可能なマスを更新
                _SelectedPiece = value;
                MovablePoints = value is null ? new List<Point>() : value.GetMovablePoints(Board);
            }
        }

        private List<Point> MovablePoints
        {
            // 移動可能マスが更新されたらパネルに反映
            set => _BoardPanel.SetMovableSquares(value);
        }

        public void MoveScene() => SceneManager.LoadScene("Result");

        private void Start() => CurrentState = new StartState(this);

        private void Update() => CurrentState = CurrentState.Update();

        public void OnSquareClick(Point point) => CurrentState = CurrentState.OnClick(point);

        private interface IGameState
        {
            IGameState Update();
            IGameState OnClick(Point point);
        }
    }
}