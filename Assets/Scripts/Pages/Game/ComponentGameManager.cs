using System;
using System.Collections.Generic;
using NoMoney.Assets.Scripts.Board;
using NoMoney.Assets.Scripts.Pieces;
using UnityEngine;
using UnityEngine.SceneManagement;
using Point = NoMoney.Assets.Scripts.Pieces.Point;

namespace NoMoney.Assets.Scripts.Pages.Game
{
    public partial class ComponentGameManager : MonoBehaviour, IBoardEventListener
    {
        private IGameState CurrentState { get; set; }
        public BoardModel Board { get; private set; }
        [SerializeField] private ComponentBoardPanel _BoardPanel;
        private List<Point> _MovablePoints = new();
        private Piece _SelectedPiece = null;
        public Turn turn { get; private set; }

        private Piece SelectedPiece
        {
            get => _SelectedPiece;
            set
            {
                // 駒が選択された時に移動可能なマスを更新
                _SelectedPiece = value;
                MovablePoints = value is null ? new List<Point>() : Board.GetMovablePoints(value);
            }
        }

        private List<Point> MovablePoints
        {
            set
            {
                _MovablePoints = value;

                // 移動可能マスが更新されたらパネルに反映
                _BoardPanel.SetMovableSquares(value);
            }
        }

        public void MoveScene() => SceneManager.LoadScene("Result");

        private void Start() => CurrentState = new StartState(this);

        private void Update() => CurrentState = CurrentState.Update();

        public void OnSquareClick(Point point) => CurrentState = CurrentState.OnClick(point);
    }
}