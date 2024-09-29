using System.Linq;
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
        public readonly struct SizeScale
        {
            public float WidthRatio { get; }
            public float HeightRatio { get; }

            public SizeScale(float widthRatio, float heightRatio)
            {
                WidthRatio = widthRatio;
                HeightRatio = heightRatio;
            }
        }

        /// <summary>
        /// 追加の位置情報
        /// </summary>
        public List<BoardPoint> ExtraPositions { get; }

        /// <summary>
        /// オブジェクトのサイズ倍率
        /// グラフィックの描画の都合上必要となる
        /// </summary>
        public SizeScale Scale
        {
            get
            {
                // ExtraPositionsの中から最大のXとYを取得
                var maxX = ExtraPositions.Max(p => p.X);
                var maxY = ExtraPositions.Max(p => p.Y);
                return new SizeScale(maxX + 1, maxY + 1);
            }
        }
    }


    /// <summary>
    /// 攻撃されないオブジェクトのインターフェース
    /// </summary>
    public interface IUntouchable
    {
    }

    /// <summary>
    /// 攻撃対象となるオブジェクトのインターフェース
    /// </summary>
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