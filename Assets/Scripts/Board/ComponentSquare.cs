using NoMoney.Assets.Scripts.Pieces;
using UnityEngine;

namespace NoMoney.Assets.Scripts.Board
{
    public class ComponentSquare : MonoBehaviour
    {
        private Point _Position;
        private float _SquareWidth;
        private float _SquareHeight;
        private RectTransform _MyTransform;
        private bool _IsInitialized = false;

        private void Start()
        {
            _MyTransform = GetComponent<RectTransform>();
            _MyTransform.sizeDelta = new Vector2(_SquareWidth, _SquareHeight);
            PositionUpdate();
        }

        public void Initialize(Point position, float squareWidth, float squareHeight)
        {
            if (_IsInitialized)
            {
                return;
            }

            _Position = position;
            _SquareWidth = squareWidth;
            _SquareHeight = squareHeight;
            _IsInitialized = true;
        }

        private void PositionUpdate()
        {
            var positionX = (_Position.X * _SquareWidth) + _SquareWidth / 2;
            var positionY = (_Position.Y * -_SquareHeight) - _SquareHeight / 2;
            var positionVector = new Vector3(positionX, positionY, 0);
            _MyTransform.anchoredPosition = positionVector;
        }
    }
}
