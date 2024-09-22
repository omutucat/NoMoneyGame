using NoMoney.Assets.Scripts.Pieces;

namespace NoMoney.Assets.Scripts.Board
{
    public interface IBoardEventListener
    {
        void OnSquareClick(Point point);
    }
}