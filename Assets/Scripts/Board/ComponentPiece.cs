using UnityEngine;
using UnityEngine.UI;
using NoMoney.Assets.Scripts.Pieces;

namespace NoMoney.Assets.Scripts.Board
{
    /// <summary>
    /// 駒を表示するためのコンポーネント
    /// </summary>
    public class ComponentPiece : MonoBehaviour
    {
        private PieceBase _Piece;
        private float _SquareWidth;
        private float _SquareHeight;
        private RectTransform _MyTransform;
        private bool _IsInitialized;

        private void Start()
        {
            // ピースのサイズを設定
            _MyTransform = GetComponent<RectTransform>();
            _MyTransform.sizeDelta = new Vector2(_SquareWidth, _SquareHeight);

            // テクスチャの設定
            var texture = PieceTextures.PieceTexture(_Piece);
            var image = GetComponent<Image>();
            image.sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));

            // 位置の更新
            PositionUpdate();
        }

        private void Update()
        {
            PositionUpdate();
        }

        /// <summary>
        /// 位置の計算
        /// </summary>
        private void PositionUpdate()
        {
            var position = _Piece.Position;
            var positionX = (position.X * _SquareWidth) + _SquareWidth / 2;
            var positionY = (position.Y * -_SquareHeight) - _SquareHeight / 2;
            var positionVector = new Vector3(positionX, positionY, 0);
            _MyTransform.anchoredPosition = positionVector;
        }

        public void Initialize(PieceBase piece, float squareWidth, float squareHeight)
        {
            if (_IsInitialized)
            {
                return;
            }

            _Piece = piece;
            _SquareWidth = squareWidth;
            _SquareHeight = squareHeight;
            _IsInitialized = true;
        }
    }
}
