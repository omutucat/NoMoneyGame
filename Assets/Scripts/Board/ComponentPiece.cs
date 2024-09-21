using System;
using UnityEngine;
using UnityEngine.UI;
using NoMoney.Assets.Scripts.Pieces;

namespace NoMoney.Assets.Scripts.Board
{
    public class ComponentPiece : MonoBehaviour
    {
        private PieceBase _Piece;
        private float _SquareWidth;
        private float _SquareHeight;

        private void Start()
        {
            // ピースのサイズを設定
            var rectTransform = GetComponent<RectTransform>();
            rectTransform.sizeDelta = new Vector2(_SquareWidth, _SquareHeight);

            // テクスチャの設定
            var texture = PieceTextures.PieceTexture(_Piece.Type);
            var image = GetComponent<Image>();
            image.sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));

            // 位置の更新
            PositionUpdate();
        }

        private void PositionUpdate()
        {
            var position = _Piece.Position;
            var positionX = (position.X * _SquareWidth) + _SquareWidth / 2;
            var positionY = (position.Y * -_SquareHeight) - _SquareHeight / 2;
            var positionVector = new Vector3(positionX, positionY, 0);
            GetComponent<RectTransform>().anchoredPosition = positionVector;
        }

        public static ComponentPiece Create(PieceBase piece, Transform parent, float squareWidth, float squareHeight)
        {
            var prefab = Resources.Load<GameObject>("Prefabs/Piece");
            var pieceObject = Instantiate(prefab, parent, false);
            var component = pieceObject.AddComponent<ComponentPiece>();
            component._Piece = piece;
            component._SquareWidth = squareWidth;
            component._SquareHeight = squareHeight;
            return component;
        }
    }
}
