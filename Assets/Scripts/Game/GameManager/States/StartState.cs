using System;
using NoMoney.Assets.Scripts.Game.Board;
using NoMoney.Assets.Scripts.Game.Objects;
using NoMoney.Assets.Scripts.Game.Objects.Pieces;
using UnityEngine;

namespace NoMoney.Assets.Scripts.Game.GameManager
{
    public partial class ComponentGameManager
    {
        /// <summary>
        /// ゲームの初期状態
        /// 各リソースの初期化や初期描画を行う
        /// </summary>
        private class StartState : IGameState
        {
            private ComponentGameManager _Manager;

            public StartState(ComponentGameManager manager) => _Manager = manager;

            public IGameState Update()
            {
                // 前画面で選択されたステージを取得
                var stageName = SystemManager.SelectedStage;
                _Manager.Board = StageList.GetStage(stageName) ?? throw new Exception("Stage not found");

                // 各コンポーネントの初期化
                // データの更新に合わせて動作できるようにイベントを登録していく
                _Manager._BoardPanel.Initialize(_Manager.Board);
                _Manager._BoardPanel.AddListener(_Manager);

                _Manager.Turn = new Turn();
                _Manager.Turn.OnTurnChanged += turn => _Manager._MessageText.text = turn.TurnPlayer == PieceSide.Player ? "Player's turn" : "Enemy's turn";
                _Manager.Turn.OnTurnChanged += _Manager._TurnComponent.OnTurnChanged;

                // NOTE: 描画を更新するために一回手動でOnTurnChangedを呼ぶ
                _Manager._MessageText.text = "Player's turn";
                _Manager._TurnComponent.OnTurnChanged(_Manager.Turn);

                // ターン更新時に発生する処理を実装するために登録
                // NOTE: 駒のOnTurnChangedは呼ばれたくないので、描画更新後に登録
                _Manager.Turn.OnTurnChanged += _Manager.Board.OnTurnChanged;
                Debug.Log("Game initialized with StartState");

                // 初期化が終わればすぐにゲーム開始する
                return new SelectState(_Manager);
            }

            public IGameState OnClick(BoardPoint point) => this;
        }
    }
}