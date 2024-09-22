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
        private IGameState _CurrentState;
        public BoardModel Board { get; private set; }
        private List<Point> _MovablePoints = new List<Point>();
        private Piece _SelectedPiece = null;
        public void MoveScene() => SceneManager.LoadScene("Result");

        [SerializeField] private ComponentBoardPanel _BoardPanel;

        private void Start()
        {
            _BoardPanel.AddListener(this);
        }

        private void Update()
        {
            _CurrentState = _CurrentState.Update();
        }

        public void OnSquareClick(Point point)
        {
            _CurrentState = _CurrentState.OnClick(point);

            Debug.Log(_CurrentState.IsAcceptClick switch
            {
                true => "Click accepted at " + point,
                false => "Click not accepted at " + point
            });
        }
    }
}