using System.Collections.Generic;
using NoMoney.Assets.Scripts.Game.Board;
using NoMoney.Assets.Scripts.Game.Objects;
using NoMoney.Assets.Scripts.Game.Objects.Pieces;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


namespace NoMoney.Assets.Scripts.Game.GameManager
{
    /// <summary>
    /// ゲームの状態を管理するクラス
    /// </summary>
    public partial class ComponentGameManager : MonoBehaviour, IBoardEventListener
    {
        /// <summary>
        /// 現在のゲームの状態
        /// </summary>
        private IGameState CurrentState { get; set; }
        /// <summary>
        /// 現在の盤面
        /// </summary>
        public BoardModel Board { get; private set; }
        /// <summary>
        /// 現在のターン
        /// </summary>
        public Turn Turn { get; private set; }

        /// <summary>
        /// 盤面の描画コンポーネント
        /// </summary>
        [SerializeField] private ComponentBoardPanel _BoardPanel;
        /// <summary>
        /// ターン描画コンポーネント
        /// </summary>
        [SerializeField] private ComponentTurn _TurnComponent;
        /// <summary>
        /// メッセージ表示用テキスト
        /// </summary>
        [SerializeField] private Text _MessageText;

        private Piece _SelectedPiece = null;
        /// <summary>
        /// 選択中の駒
        /// </summary>
        private Piece SelectedPiece
        {
            get => _SelectedPiece;
            set
            {
                // 駒が選択された時に移動可能なマスを更新
                _SelectedPiece = value;
                MovablePoints = value is null ? new List<BoardPoint>() : value.GetMovablePoints(Board);
            }
        }

        /// <summary>
        /// 移動可能なマス
        /// </summary>
        private List<BoardPoint> MovablePoints
        {
            // 移動可能マスが更新されたらパネルに反映
            set => _BoardPanel.SetMovableSquares(value);
        }

        /// <summary>
        /// 次のシーンへ遷移
        /// </summary>
        public void ToNextScene() => SceneManager.LoadScene("Result");

        // Stateパターンで運用される
        private void Start() => CurrentState = new StartState(this);

        private void Update() => CurrentState = CurrentState.Update();

        public void OnSquareClick(BoardPoint point) => CurrentState = CurrentState.OnClick(point);

        /// <summary>
        /// ゲームの状態を表すインターフェース
        /// </summary>
        private interface IGameState
        {
            IGameState Update();
            IGameState OnClick(BoardPoint point);
        }
    }
}