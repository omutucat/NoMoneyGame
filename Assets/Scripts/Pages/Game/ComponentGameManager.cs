using System.Drawing;
using UnityEngine;

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

        private void Start()
        {
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