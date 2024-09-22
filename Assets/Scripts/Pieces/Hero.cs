﻿using System.Collections.Generic;

namespace NoMoney.Assets.Scripts.Pieces
{
    /// <summary>
    /// 勇者の駒
    /// 5ターン目まで動けない
    /// 盤面の端に到達すると、反対側の端にワープする
    /// </summary>
    public class Hero : PieceBase, ITeleportable
    {
        private int _TurnCount = 0;
        private const int IMMOBILE_TURNS = 5;

        public Hero(Point position) : base(position, new List<PieceStatus> { PieceStatus.Immobilized })
        {
        }

        protected override List<Point> SpecificMovablePoints =>
            // 8方向全てに進める
            new()
            {
                new(1, 0),
                new(-1, 0),
                new(0, 1),
                new(0, -1),
                new(1, 1),
                new(1, -1),
                new(-1, 1),
                new(-1, -1),
            };

        public override void OnTurnEnd()
        {
            if (_TurnCount < IMMOBILE_TURNS)
            {
                _TurnCount++;
                StatusList.Remove(PieceStatus.Immobilized);
            }
        }
    }
}
