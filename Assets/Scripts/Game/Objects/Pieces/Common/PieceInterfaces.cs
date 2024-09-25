using System;
using System.Collections.Generic;
using Unity.VisualScripting;

namespace NoMoney.Assets.Scripts.Game.Objects.Pieces
{
    public interface IAbnormalShape
    {
        public List<Point> ExtraPositions { get; }
    }

    public interface ITeleportable
    {
    }

    public interface IUnbreakable
    {
    }
}
