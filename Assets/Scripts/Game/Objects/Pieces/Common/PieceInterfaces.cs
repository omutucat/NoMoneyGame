using System.Collections.Generic;

namespace NoMoney.Assets.Scripts.Game.Objects.Pieces
{
    public interface IAbnormalShape
    {
        public List<BoardPoint> ExtraPositions { get; }
    }

    public interface IUnbreakable
    {
    }
}