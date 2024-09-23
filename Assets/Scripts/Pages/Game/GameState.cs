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

                // クリックされた座標の駒を取得
                var clickedPiece = _Manager.Board.GetMovablePiecesAt(point);

                if (clickedPiece == _Manager.SelectedPiece)
                {
                    // 選択中の駒と同じなら何もしない
                    return this;
                }

                if (clickedPiece is not null)
                {
                    // 選択中の駒と違う駒がクリックされた場合
                    _Manager.SelectedPiece = clickedPiece;

                    return new MoveState(_Manager);
                }

                //移動可能なマスでなければ何もしない
                if (!_Manager.MovablePoints.Contains(point))
                {
                    Debug.Log("Invalid move: " + point.ToDebugString() + " is not in movable points");
                    return this;
                }

                //移動先に敵駒がいたら破壊する
                _Manager.Board.DestroyPieceAt(point,_Manager.turn.GameSide());
                
                _Manager.Board.MovePiece(_Manager.SelectedPiece, point);
                Debug.Log("Valid move to " + point);

                _Manager.SelectedPiece = null;

                // 移動が完了したら新しいStateを返す
                return new CalcState(_Manager);
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
