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
        [SerializeField] private int _BoardWidth = 8;
        [SerializeField] private int _BoardHeight = 8;

        private void Awake()
        {
            // TODO: 本来は外部から受け取る
            var currentStage = SystemManager.CurrentStage;
            Board = StageList.GetStage(currentStage) ?? throw new Exception("Stage not found");

            _BoardPanel.Initialize(Board);
            _CurrentState = new StartState(this);
            Debug.Log("Game initialized with StartState");
        }

        private void Start()
        {
            _BoardPanel.AddListener(this);
        }

        private void Update()
        {
            _CurrentState = _CurrentState.Update();
        }

        public bool CanAcceptClicks => _CurrentState.IsAcceptClick;
        public void OnSquareClick(Point point)
        {
            if (_CurrentState.IsAcceptClick)
            {
                Debug.Log("Click accepted at " + point);
                _CurrentState = _CurrentState.OnClick(point);
            }
            else
            {
                Debug.Log("Click not accepted at " + point);
            }
        }
    }
}