using System;
using NoMoney.Assets.Scripts.Game.Board;
using NoMoney.Assets.Scripts.Game.Objects.Pieces;
using NoMoney.Assets.Scripts.Game.Objects.Pieces;
using UnityEngine;

namespace NoMoney.Assets.Scripts.Game.GameManager
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
                _Manager.Turn.OnTurnChanged += turn => _Manager._MessageText.text = turn.TurnPlayer == PieceSide.Player ? "Player's turn" : "Enemy's turn";
                _Manager.Turn.OnTurnChanged += _Manager._TurnComponent.OnTurnChanged;

                // 描画を更新するために一回手動でOnTurnChangedを呼ぶ
                _Manager._MessageText.text = "Player's turn";
                _Manager._TurnComponent.OnTurnChanged(_Manager.Turn);

                // 駒のOnTurnChangedは呼ばれたくないので、描画更新後に登録
                _Manager.Turn.OnTurnChanged += _Manager.Board.OnTurnChanged;
                Debug.Log("Game initialized with StartState");

                return new SelectState(_Manager);
            }

            public IGameState OnClick(Point point) => this;
        }
    }
}