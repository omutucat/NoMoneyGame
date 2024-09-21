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

        private void Start()
        {
            // ピースのサイズを設定
            var rectTransform = GetComponent<RectTransform>();
            rectTransform.sizeDelta = new Vector2(_SquareWidth, _SquareHeight);

            // テクスチャの設定
            var texture = PieceTextures.PieceTexture(_Piece);
            var image = GetComponent<Image>();
            image.sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));

            // 位置の更新
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
            GetComponent<RectTransform>().anchoredPosition = positionVector;
        }

        /// <summary>
        /// 駒を生成する
        /// </summary>
        /// <param name="piece">駒インスタンス</param>
        /// <param name="parent">親オブジェクトのtransform</param>
        /// <param name="squareWidth">一マスの幅</param>
        /// <param name="squareHeight">一マスの高さ</param>
        /// <returns></returns>
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
