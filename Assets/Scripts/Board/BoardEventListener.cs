using NoMoney.Assets.Scripts.Pieces;

namespace NoMoney.Assets.Scripts.Board
{
    public interface BoardEventListener
    {
        void OnSquareClick(Point point);
    }
}