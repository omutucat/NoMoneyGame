using System;
using NoMoney.Assets.Scripts.Game.Board;
using NoMoney.Assets.Scripts.Game.Objects;

namespace NoMoney.Assets.Scripts.Game.GameManager
{
    public partial class ComponentGameManager
    {
        /// <summary>
        /// ゲームの勝敗判定を行う状態
        /// </summary>
        private class CalcState : IGameState
        {
            private ComponentGameManager _Manager;

            public CalcState(ComponentGameManager manager) => _Manager = manager;

            public IGameState Update()
            {
                switch (_Manager.Board.JudgeGameState())
                {
                    case GameStatus.Win or GameStatus.Lose or GameStatus.Draw:
                        return new EndState(_Manager);
                    case GameStatus.Playing:
                        _Manager.Turn.ToNextPlayer();
                        return new SelectState(_Manager);
                    default:
                        throw new Exception("Invalid game state");
                }
            }

            public IGameState OnClick(BoardPoint point) => this;
        }

    }
}