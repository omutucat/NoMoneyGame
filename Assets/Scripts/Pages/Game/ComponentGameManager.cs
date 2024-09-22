using System.Collections.Generic;
using NoMoney.Assets.Scripts.Board;
using NoMoney.Assets.Scripts.Pieces;
using UnityEngine;
using Point = NoMoney.Assets.Scripts.Pieces.Point;

namespace NoMoney.Assets.Pages.Game
{
    public class ComponentGameManager : MonoBehaviour, IBoardEventListener
    {
        private IState _CurrentState;
        public BoardModel Board { get; private set; }
        private List<Point> MovablePoints = new List<Point>();
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

            public IState OnClick(Point point) => this;
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
                // クリックされた座標の駒を取得
                var obj = _manager.Board.GetMovablePiecesAt(point);
                // 駒が無ければ何もしない
                if (obj == null)
                    return this;
                // 駒があれば移動可能なマスを着色
                foreach(var movablePoint in _manager.Board.GetMovablePoints(obj))
                { 
                    _manager.MovablePoints.Add(movablePoint);
                    _manager._BoardPanel.ChangeSquareTextureMovable(movablePoint);
                }
                
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
                //移動可能なマスでなければ何もしない
                if(!_manager.MovablePoints.Contains(point)) return this;
                // TODO 移動処理
                
                
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
                // 計算処理、勝敗判定、ターン切り替え
                return new SelectState(_manager);
            }

            public IState OnClick(Point point) => this;
        }

        private void Awake()
        {
            // TODO: 本来は外部から受け取る
            var size = new BoardSize(_BoardWidth, _BoardHeight);
            var pieces = new List<BoardObject>
            {
                new Pawn(new Point(0, 0)),
                new Pawn(new Point(1, 0)),
            };
            Board = new BoardModel(size, pieces);

            _BoardPanel.Initialize(Board);

            _CurrentState = new StartState(this);
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
                _CurrentState = _CurrentState.OnClick(point);
            }
        }
    }
}