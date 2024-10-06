using System.Collections.Generic;
using System.Linq;
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

        public BoardPoint Position { get; }

        /// <summary>
        /// 追加の形状情報
        /// </summary>
        public List<BoardPoint> ExtraShapePoints { get; }

        /// <summary>
        /// 追加の形状に伴う位置情報
        /// </summary>
        public List<BoardPoint> ExtraPositions
        {
            get
            {
                var positions = new List<BoardPoint>();
                foreach (var p in ExtraShapePoints)
                {
                    positions.Add(new BoardPoint(Position.X + p.X, Position.Y + p.Y));
                }
                return positions;
            }
        }

        /// <summary>
        /// オブジェクトのサイズ倍率
        /// グラフィックの描画の都合上必要となる
        /// </summary>
        public SizeScale Scale
        {
            get
            {
                // 追加の形状点の中から最大のXとYを取得
                var maxX = ExtraShapePoints.Max(p => p.X);
                var maxY = ExtraShapePoints.Max(p => p.Y);
                return new SizeScale(maxX + 1, maxY + 1);
            }
        }
    }

    /// <summary>
    /// 攻撃されないオブジェクトのインターフェース
    /// </summary>
    public interface IUntouchable { }

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
