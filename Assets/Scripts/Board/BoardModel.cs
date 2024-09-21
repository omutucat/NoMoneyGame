using System.Collections.Generic;
using NoMoney.Assets.Scripts.Pieces;

namespace NoMoney.Assets.Scripts.Board
{
    public class BoardSize
    {
        private int _Width;
        private int _Height;
        public int Width
        {
            get => _Width;
            set
            {
                if (value < 1)
                {
                    throw new System.ArgumentOutOfRangeException();
                }
                _Width = value;
            }
        }
        public int Height
        {
            get => _Height;
            set
            {
                if (value < 1)
                {
                    throw new System.ArgumentOutOfRangeException();
                }
                _Height = value;
            }
        }

        public BoardSize(int width, int height)
        {
            Width = width;
            Height = height;
        }
    }

    public class ObjectList
    {
        public List<PieceBase> Pieces { get; }

        public ObjectList()
        {
            Pieces = new List<PieceBase>();
        }
    }

    public class BoardModel
    {
        public ObjectList Objects { get; }
        public BoardSize Size { get; }

        public BoardModel(BoardSize size, ObjectList objects)
        {
            Size = size;
            Objects = objects;
        }
    }
}
