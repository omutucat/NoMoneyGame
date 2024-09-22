using System;
using System.Collections.Generic;
using NoMoney.Assets.Scripts;
using NoMoney.Assets.Scripts.Board;
using NoMoney.Assets.Scripts.Pieces;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;
using Point = NoMoney.Assets.Scripts.Pieces.Point;

namespace NoMoney.Assets.Pages.Game
{
    public class ComponentGameManager : MonoBehaviour, IBoardEventListener
    {
        private IState _CurrentState;
        public BoardModel Board { get; private set; }
        private List<Point> MovablePoints = new List<Point>();
        private Piece _SelectedPiece = null;
        public void MoveScene() => SceneManager.LoadScene("Result");

        [SerializeField] private ComponentBoardPanel _BoardPanel;
        [SerializeField] private int _BoardWidth = 8;
        [SerializeField] private int _BoardHeight = 8;

        private interface IState
        {
            IState Update();
            IState OnClick(Point point);
            bool IsAcceptClick { get; }
        }

        private class StartState : IState
        {
            private ComponentGameManager _manager;

            public StartState(ComponentGameManager manager)
            {
                _manager = manager;
            }

            public bool IsAcceptClick => false;

            public IState Update()
            {
                // 初期化処理
                return new SelectState(_manager);
            }

            public IState OnClick(Point point)
            {
                Debug.Log("StartState OnClick triggered at " + point.ToDebugString());
                return this;
            }
        }

        private class SelectState : IState
        {
            private ComponentGameManager _manager;

            public SelectState(ComponentGameManager manager)
            {
                _manager = manager;
            }

            public bool IsAcceptClick => true;

            public IState Update() => this;

            public IState OnClick(Point point)
            {
                Debug.Log("SelectState OnClick triggered at " + point.ToDebugString());
                // クリックされた座標の駒を取得
                var obj = _manager.Board.GetMovablePiecesAt(point);
                // 駒が無ければ何もしない
                if (obj == null)
                {
                    Debug.Log("No piece found at " + point.ToDebugString());
                    return this;
                }
                // 駒があれば移動可能なマスを着色
                foreach (var movablePoint in _manager.Board.GetMovablePoints(obj))
                {
                    _manager.MovablePoints.Add(movablePoint);
                    _manager._BoardPanel.ChangeSquareTextureMovable(movablePoint);
                }
                _manager._SelectedPiece = obj;
                Debug.Log("Movable points: " + string.Join(", ", _manager.MovablePoints));
                return new MoveState(_manager);
            }
        }

        private class MoveState : IState
        {
            private ComponentGameManager _manager;

            public MoveState(ComponentGameManager manager)
            {
                _manager = manager;
            }

            public bool IsAcceptClick => true;

            public IState Update() => this;

            public IState OnClick(Point point)
            {
                Debug.Log("MoveState OnClick triggered at " + point.ToDebugString());
                //移動可能なマスでなければ何もしない
                if (!_manager.MovablePoints.Contains(point))
                {
                    Debug.Log("Invalid move: " + point.ToDebugString() + " is not in movable points");
                    return this;
                }
                // TODO 移動処理
                _manager.Board.MovePiece(_manager._SelectedPiece, point);
                Debug.Log("Valid move to " + point);


                // 移動が完了したら新しいStateを返す
                return new CalcState(_manager);
            }
        }

        private class CalcState : IState
        {
            private ComponentGameManager _manager;

            public CalcState(ComponentGameManager manager)
            {
                _manager = manager;
            }

            public bool IsAcceptClick => false;

            public IState Update()
            {
                switch (_manager.Board.JudgeGameState())
                {
                    case GameStatus.Draw:
                        return new EndState(_manager);
                    case GameStatus.Win:
                        return new EndState(_manager);
                    case GameStatus.Lose:
                        return new EndState(_manager);
                    case GameStatus.Playing:
                        return new SelectState(_manager);
                    default:
                        return new SelectState(_manager);
                }
            }

            public IState OnClick(Point point)
            {
                Debug.Log("CalcState OnClick triggered at " + point.ToDebugString());
                return this;
            }
        }

        private class EndState : IState
        {
            private ComponentGameManager _manager;

            public EndState(ComponentGameManager manager)
            {
                _manager = manager;
            }

            public bool IsAcceptClick => false;

            public IState Update()
            {
                _manager.MoveScene();
                return this;
            }

            public IState OnClick(Point point)
            {
                Debug.Log("EndState OnClick triggered at " + point.ToDebugString());
                return this;
            }
        }


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