using UnityEngine;
using UnityEngine.UI;

namespace NoMoney.Assets.Scripts.Game.Objects.Pieces
{
    /// <summary>
    /// 駒を表示するためのコンポーネント
    /// </summary>
    public class ComponentPiece : MonoBehaviour
    {
        private Piece _Piece;
        private float _SquareWidth;
        private float _SquareHeight;
        private RectTransform _MyTransform;
        private bool _IsInitialized = false;

        private void Start()
        {
            // ピースのサイズを設定
            _MyTransform = GetComponent<RectTransform>();
            _MyTransform.sizeDelta = new Vector2(_SquareWidth, _SquareHeight);

            // テクスチャの設定
            var texture = PieceTextures.PieceTexture(_Piece);
            var image = GetComponent<Image>();
            image.sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));

            // 敵か味方かに応じて色を変更
            // TODO: 色を固定値でなくどこかに定義する
            if (_Piece.Side == PieceSide.Enemy)
            {
                image.color = new Color(1, 0, 0, 1);
            }

            // 位置の更新
            PositionUpdate();
        }

        private void Update() => PositionUpdate();

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

        public void Initialize(Piece piece, float squareWidth, float squareHeight)
        {
            if (_IsInitialized)
            {
                return;
            }

            _Piece = piece;
            _SquareWidth = squareWidth;
            _SquareHeight = squareHeight;
            _IsInitialized = true;

            // 駒の破壊時イベントを登録
            _Piece.OnDestroy += DestroyMyself;
        }

        private void DestroyMyself(BoardObject sender) => Destroy(gameObject);
    }
}