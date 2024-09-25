namespace NoMoney.Assets.Scripts.Pieces
{
    public abstract class PieceStatus
    {
        public delegate void RemoveStatusEventHandler(PieceStatus sender);

        public event RemoveStatusEventHandler OnRemove;

        public void Remove() => OnRemove?.Invoke(this);
    }

    public class Immobilized : PieceStatus
    {
    }

    public class InSleep : PieceStatus, ITurnChangeListener
    {
        public int TurnCount { get; private set; }

        public InSleep(int turnCount) => TurnCount = turnCount;

        public void DecreaseTurnCount()
        {
            TurnCount--;
            if (TurnCount == 0)
            {
                Remove();
            }
        }

        public void OnTurnEnd() => DecreaseTurnCount();
    }

    public interface ITurnChangeListener
    {
        void OnTurnEnd();
    }
}