using System;
using System.Collections.Generic;
using NoMoney.Assets.Scripts.Board;
using NoMoney.Assets.Scripts.Pieces;
using UnityEngine;

namespace NoMoney.Assets.Scripts.Pages.Game
{
    public partial class ComponentGameManager
    {
        private interface IGameState
        {
            IGameState Update();
            IGameState OnClick(Point point);
        }

        private class StartState : IGameState
        {
            private ComponentGameManager _Manager;

            public StartState(ComponentGameManager manager) => _Manager = manager;

            public IGameState Update()
            {
                // 初期化処理
                var currentStage = SystemManager.CurrentStage;
                _Manager.Board = StageList.GetStage(currentStage) ?? throw new Exception("Stage not found");

                _Manager._BoardPanel.Initialize(_Manager.Board);
                _Manager._BoardPanel.AddListener(_Manager);
                _Manager.turn = new Turn();
                Debug.Log("Game initialized with StartState");

                return new SelectState(_Manager);
            }

            public IGameState OnClick(Point point) => this;
        }

        private class SelectState : IGameState
        {
            private ComponentGameManager _Manager;

            public SelectState(ComponentGameManager manager) => _Manager = manager;

            public IGameState Update() => this;

            public IGameState OnClick(Point point)
            {
                Debug.Log("SelectState OnClick triggered at " + point.ToDebugString());

                // クリックされた座標の駒を取得
                var obj = _Manager.Board.GetMovablePiecesAt(point);

                // 駒が無ければ何もしない
                if (obj is null)
                {
                    Debug.Log("No piece found at " + point.ToDebugString());
                    return this;
                }

                // 敵の駒を押したときは何もしない
                if (obj.Side != _Manager.turn.GameSide())
                {
                    Debug.Log("SelectState OnClick triggered at " + point.ToDebugString());
                    return this;
                }

                _Manager.SelectedPiece = obj;

                return new MoveState(_Manager);
            }
        }

        private class MoveState : IGameState
        {
            private ComponentGameManager _Manager;

            public MoveState(ComponentGameManager manager) => _Manager = manager;

            public IGameState Update() => this;

            public IGameState OnClick(Point point)
            {
                Debug.Log("MoveState OnClick triggered at " + point.ToDebugString());

                var clickedPiece = _Manager.Board.GetMovablePiecesAt(point);

                if (clickedPiece is not null && clickedPiece.Side == _Manager.turn.GameSide())
                {
                    _Manager.SelectedPiece = clickedPiece;
                    return this;
                }

                switch (_Manager.Board.TryMovePiece(_Manager.SelectedPiece, point))
                {
                    case true:
                        _Manager.SelectedPiece = null;
                        return new CalcState(_Manager);
                    case false:
                        return this;
                }
            }
        }

        private class CalcState : IGameState
        {
            private ComponentGameManager _Manager;

            public CalcState(ComponentGameManager manager)
            {
                _Manager = manager;
            }

            public IGameState Update()
            {
                _Manager.turn.OnTurnEnd();
                _Manager.Board.OnTurnEnd();
                return _Manager.Board.JudgeGameState() switch
                {
                    GameStatus.Win or GameStatus.Lose or GameStatus.Draw => new EndState(_Manager),
                    GameStatus.Playing => new SelectState(_Manager),
                    _ => throw new Exception("Invalid game state"),
                };
            }

            public IGameState OnClick(Point point)
            {
                Debug.Log("CalcState OnClick triggered at " + point.ToDebugString());
                return this;
            }
        }

        private class EndState : IGameState
        {
            private ComponentGameManager _Manager;

            public EndState(ComponentGameManager manager)
            {
                _Manager = manager;
            }

            public IGameState Update()
            {
                _Manager.MoveScene();
                return this;
            }

            public IGameState OnClick(Point point)
            {
                Debug.Log("EndState OnClick triggered at " + point.ToDebugString());
                return this;
            }
        }
    }
}
