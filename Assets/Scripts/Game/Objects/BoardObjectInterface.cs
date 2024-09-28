using System.Collections.Generic;
using NoMoney.Assets.Scripts.Game.Board;
using NoMoney.Assets.Scripts.Game.Objects.Pieces;

namespace NoMoney.Assets.Scripts.Game.Objects
{
    /// <summary>
    /// 特殊な形状を持つオブジェクトのインターフェース
    /// </summary>
    public interface IAbnormalShape
    {
        /// <summary>
        /// 追加の位置情報
        /// </summary>
        public List<BoardPoint> ExtraPositions { get; }
    }

    /// <summary>
    /// 攻撃されないオブジェクトのインターフェース
    /// </summary>
    public interface IUntouchable
    {
    }

    public interface IAttackTarget
    {
        /// <summary>
        /// 攻撃対象に取れるかどうか
        /// </summary>
        public bool IsTouchable { get; }

        /// <summary>
        /// 攻撃されたときの処理
        /// </summary>
        /// <param name="board"></param>
        /// <param name="attacker"></param>
        public void OnAttacked(BoardModel board, Piece attacker);
    }
}