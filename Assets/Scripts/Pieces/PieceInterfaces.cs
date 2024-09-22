using System.Collections.Generic;

namespace NoMoney.Assets.Scripts.Pieces
{
    public interface IAbnormalShape
    {
        public List<Point> ExtraPositions { get; }
    }

    public interface IGhost
    {
    }

    public interface ITeleportable
    {
    }
}
