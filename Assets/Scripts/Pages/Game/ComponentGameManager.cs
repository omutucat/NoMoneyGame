using System.Collections.Generic;
using NoMoney.Assets.Scripts.Board;
using NoMoney.Assets.Scripts.Pieces;
using UnityEngine;
using Point = NoMoney.Assets.Scripts.Pieces.Point;

namespace NoMoney.Assets.Pages.Game
{
    public interface IState
    {
        IState Update();
        IState OnClick(Point point);
        bool IsAcceptClick { get; }
    }

    public class Start : IState
    {
        public bool IsAcceptClick => false;

        public IState Update()
        {
            // 初期化処理
            return new Select();
        }

        public IState OnClick(Point point) => this;
    }

    public class Select : IState
    {
        public bool IsAcceptClick => true;

        public IState Update() => this;

        public IState OnClick(Point point)
        {
            // 選択処理
            // 選択が完了したら新しいStateを返す
            return new Move();
        }
    }

    public class Move : IState
    {
        public bool IsAcceptClick => true;

        public IState Update() => this;

        public IState OnClick(Point point)
        {
            // 移動処理
            // 移動が完了したら新しいStateを返す
            return new Calc();
        }
    }

    public class Calc : IState
    {
        public bool IsAcceptClick => false;

        public IState Update()
        {
            // 計算処理、勝敗判定、ターン切り替え
            return new Select();
        }

        public IState OnClick(Point point) => this;
    }

    public class ComponentGameManager : MonoBehaviour
    {
        private IState _CurrentState;
        private BoardModel Board { get; set; }
        [SerializeField] private ComponentBoardPanel _BoardPanel;
        [SerializeField] private int _BoardWidth = 8;
        [SerializeField] private int _BoardHeight = 8;

        private void Start()
        {
            // TODO: 本来は外部から受け取る
            var size = new BoardSize(_BoardWidth, _BoardHeight);
            var pieces = new List<PieceBase>
            {
                new Pawn(new Point(0, 0)),
                new Pawn(new Point(1, 0)),
            };
            Board = new BoardModel(size, pieces);

            _BoardPanel.Initialize(Board);

            _CurrentState = new Start();
        }

        private void Update()
        {
            _CurrentState = _CurrentState.Update();
        }

        public void OnButtonClicked(Point point)
        {
            if (_CurrentState.IsAcceptClick)
            {
                _CurrentState = _CurrentState.OnClick(point);
            }
        }

        public bool CanAcceptClicks => _CurrentState.IsAcceptClick;
    }
}