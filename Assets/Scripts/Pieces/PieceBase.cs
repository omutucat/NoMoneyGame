using System.Collections.Generic;
using System.Linq;

namespace NoMoney.Assets.Scripts.Pieces
{
    /// <summary>
    /// 駒の基底クラス
    /// </summary>
    public abstract class PieceBase : BoardObject
    {
        /// <summary>
        /// 駒の属性
        /// </summary>
        protected List<PieceStatus> StatusList { get; }

        /// <summary>
        /// 派生クラスで実装する移動可能な座標
        /// </summary>
        /// <returns></returns>
        protected abstract List<Point> SpecificMovablePoints { get; }

        /// <summary>
        /// 移動可能な座標
        /// </summary>
        public List<Point> MoveablePoints => StatusList switch
        {
            // 駒がImmobilizedの場合は移動できない
            { } when StatusList.Any(s => s == PieceStatus.Immobilized) => new List<Point>(),
            _ => SpecificMovablePoints
        };

        protected PieceBase(Point position, IEnumerable<PieceStatus>? statusList = null)
        {
            Position = position;
            StatusList = statusList?.ToList() ?? new List<PieceStatus>();
        }

        public void SetPosition(Point position) => Position = position;

        public bool IsContainStatus(PieceStatus status) => StatusList.Contains(status);

        public virtual void OnTurnEnd()
        {
        }
    }
}

