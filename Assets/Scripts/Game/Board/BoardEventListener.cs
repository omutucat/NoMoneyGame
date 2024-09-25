using NoMoney.Assets.Scripts.Game.Objects.Pieces;

namespace NoMoney.Assets.Scripts.Game.Board
{
    public interface IBoardEventListener
    {
        void OnSquareClick(Point point);
    }
}