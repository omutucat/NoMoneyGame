using System;
using NoMoney.Assets.Scripts.Board;
using NoMoney.Assets.Scripts.Pieces;
using UnityEngine;

namespace NoMoney.Assets.Scripts.Pages.Game
{
    public partial class ComponentGameManager
    {

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
                _Manager.Turn = new Turn();
                _Manager.Turn.OnTurnChanged += side => _Manager._MessageText.text = side == PieceSide.Player ? "Player's turn" : "Enemy's turn";
                _Manager.Turn.OnTurnChanged += side => _Manager._TurnText.text = _Manager.Turn.Count.ToString();
                _Manager.Turn.OnTurnChanged += _Manager.Board.OnTurnChanged;
                _Manager._TurnText.text = _Manager.Turn.Count.ToString();
                _Manager._MessageText.text = "Player's turn";
                Debug.Log("Game initialized with StartState");

                return new SelectState(_Manager);
            }

            public IGameState OnClick(Point point) => this;
        }
    }
}