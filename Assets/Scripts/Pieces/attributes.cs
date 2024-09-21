using System.Collections.Generic;

namespace NoMoney.Assets.Scripts.Pieces
{
    public enum AttributeType
    {
        AbnormalShape,
        Teleportable,
        GhostState,
        Immobilized
    }
    
    public class ImmobilizedAttribute : PieceAttribute
    {
        public ImmobilizedAttribute() : base(AttributeType.Immobilized) { }
    }

    public abstract class PieceAttribute
    {
        public AttributeType Type { get; }

        protected PieceAttribute(AttributeType type)
        {
            Type = type;
        }
    }

    public class AbnormalShapeAttribute : PieceAttribute
    {
        public List<Point> AnotherPositions { get; }

        public AbnormalShapeAttribute(List<Point> anotherPositions) 
            : base(AttributeType.AbnormalShape)
        {
            AnotherPositions = anotherPositions;
        }
    }

    public class TeleportableAttribute : PieceAttribute
    {
        public TeleportableAttribute() : base(AttributeType.Teleportable) { }
    }

    public class GhostStateAttribute : PieceAttribute
    {
        public GhostStateAttribute() : base(AttributeType.GhostState) { }
    }
}