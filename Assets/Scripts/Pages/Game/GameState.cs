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
            bool IsAcceptClick { get; }
        }

        private class StartState : IGameState
        {
            private ComponentGameManager _Manager;

            public StartState(ComponentGameManager manager) => _Manager = manager;

            public bool IsAcceptClick => false;

            public IGameState Update()
            {
                // 初期化処理
                var currentStage = SystemManager.CurrentStage;
                _Manager.Board = StageList.GetStage(currentStage) ?? throw new Exception("Stage not found");

                _Manager._BoardPanel.Initialize(_Manager.Board);
                _Manager._BoardPanel.AddListener(_Manager);
                Debug.Log("Game initialized with StartState");

                return new SelectState(_Manager);
            }

            public IGameState OnClick(Point point) => this;
        }

        private class SelectState : IGameState
        {
            private ComponentGameManager _Manager;

            public SelectState(ComponentGameManager manager) => _Manager = manager;

            public bool IsAcceptClick => true;

            public IGameState Update() => this;

            public IGameState OnClick(Point point)
            {
                Debug.Log("SelectState OnClick triggered at " + point.ToDebugString());
                // クリックされた座標の駒を取得
                var obj = _Manager.Board.GetMovablePiecesAt(point);
                // 駒が無ければ何もしない
                if (obj == null)
                {
                    Debug.Log("No piece found at " + point.ToDebugString());
                    return this;
                }
                // 駒があれば移動可能なマスを着色
                foreach (var movablePoint in _Manager.Board.GetMovablePoints(obj))
                {
                    _Manager._MovablePoints.Add(movablePoint);
                    _Manager._BoardPanel.ChangeSquareTextureMovable(movablePoint);
                }
                _Manager._SelectedPiece = obj;
                Debug.Log("Movable points: " + string.Join(", ", _Manager._MovablePoints));
                return new MoveState(_Manager);
            }
        }

        private class MoveState : IGameState
        {
            private ComponentGameManager _Manager;

            public MoveState(ComponentGameManager manager)
            {
                _Manager = manager;
            }

            public bool IsAcceptClick => true;

            public IGameState Update() => this;

            public IGameState OnClick(Point point)
            {
                Debug.Log("MoveState OnClick triggered at " + point.ToDebugString());
                //別の味方駒をクリックした時なら、Movableを出し直してMoveに戻る
                var obj = _Manager.Board.GetMovablePiecesAt(point);
                if (obj != null && obj != _Manager._SelectedPiece)
                {
                    _Manager._SelectedPiece = obj;
                    foreach (var movablePoint in _Manager._MovablePoints)
                    {
                        //脱色
                        _Manager._BoardPanel.RemoveSquareTexture(movablePoint);
                    }
                    //移動リストを消す
                    _Manager._MovablePoints = new List<Point>();
                    foreach (var movablePoint in _Manager.Board.GetMovablePoints(obj))
                    {
                        _Manager._MovablePoints.Add(movablePoint);
                        //着色
                        _Manager._BoardPanel.ChangeSquareTextureMovable(movablePoint);
                    }
                    return new MoveState(_Manager);
                }
                //移動可能なマスでなければ何もしない
                if (!_Manager._MovablePoints.Contains(point))
                {
                    Debug.Log("Invalid move: " + point.ToDebugString() + " is not in movable points");
                    return this;
                }
                _Manager.Board.MovePiece(_Manager._SelectedPiece, point);
                foreach (var movablePoint in _Manager._MovablePoints)
                {
                    //脱色
                    _Manager._BoardPanel.RemoveSquareTexture(movablePoint);
                }
                _Manager._MovablePoints = new List<Point>();
                Debug.Log("Valid move to " + point);


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

            public bool IsAcceptClick => false;

            public IGameState Update() => _Manager.Board.JudgeGameState() switch
            {
                GameStatus.Win => new EndState(_Manager),
                GameStatus.Lose => new EndState(_Manager),
                GameStatus.Draw => new EndState(_Manager),
                GameStatus.Playing => new SelectState(_Manager),
                _ => throw new Exception("Invalid game state"),
            };

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

            public bool IsAcceptClick => false;

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
