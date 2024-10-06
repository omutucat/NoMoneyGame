using NoMoney.Assets.Scripts.Game.Objects;

namespace NoMoney.Assets.Scripts.Game.Board
{
    public interface IBoardEventListener
    {
        void OnSquareClick(BoardPoint point);
    }
}
