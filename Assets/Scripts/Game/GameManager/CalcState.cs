using System;
using NoMoney.Assets.Scripts.Board;
using NoMoney.Assets.Scripts.Pieces;
using UnityEngine;

namespace NoMoney.Assets.Scripts.Pages.Game
{
    public partial class ComponentGameManager
    {
        private class CalcState : IGameState
        {
            private ComponentGameManager _Manager;

            public CalcState(ComponentGameManager manager) => _Manager = manager;

            public IGameState Update() => _Manager.Board.JudgeGameState() switch
            {
                GameStatus.Win or GameStatus.Lose or GameStatus.Draw => new EndState(_Manager),
                GameStatus.Playing => new TurnChangeState(_Manager),
                _ => throw new Exception("Invalid game state"),
            };

            public IGameState OnClick(Point point)
            {
                Debug.Log("CalcState OnClick triggered at " + point.ToDebugString());
                return this;
            }
        }

    }
}