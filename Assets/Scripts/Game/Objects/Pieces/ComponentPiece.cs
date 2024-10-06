using UnityEngine;
using UnityEngine.UI;

namespace NoMoney.Assets.Scripts.Game.Objects.Pieces
{
    /// <summary>
    /// 駒を表示するためのコンポーネント
    /// </summary>
    public class ComponentPiece : MonoBehaviour
    {
        private Piece Piece { get; set; }
        private float SquareWidth { get; set; }
        private float SquareHeight { get; set; }
        private float MyWidth { get; set; }
        private float MyHeight { get; set; }
        private RectTransform _MyTransform;
        private bool _IsInitialized = false;

        private void Awake() => _MyTransform = GetComponent<RectTransform>();

        private void Start()
        {
            // ピースのサイズを設定
            // 異形オブジェクトの場合倍率をかける
            (MyWidth, MyHeight) = Piece switch
            {
                IAbnormalShape ap => (
                    ap.Scale.WidthRatio * SquareWidth,
                    ap.Scale.HeightRatio * SquareHeight
                ),
                _ => (SquareWidth, SquareHeight),
            };

            _MyTransform.sizeDelta = new Vector2(MyWidth, MyHeight);

            // テクスチャの設定
            var texture = PieceTextures.PieceTexture(Piece);
            var image = GetComponent<Image>();
            image.sprite = Sprite.Create(
                texture,
                new Rect(0, 0, texture.width, texture.height),
                new Vector2(0.5f, 0.5f)
            );

            // 敵か味方かに応じて色を変更
            // TODO: 色を固定値でなくどこかに定義する
            if (Piece.Side == PieceSide.Enemy)
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
            var position = Piece.Position;
            var positionX = (position.X * SquareWidth) + MyWidth / 2;
            var positionY = (position.Y * -SquareHeight) - MyHeight / 2;
            var positionVector = new Vector3(positionX, positionY, 0);
            _MyTransform.anchoredPosition = positionVector;
        }

        public void Initialize(Piece piece, float squareWidth, float squareHeight)
        {
            if (_IsInitialized)
            {
                return;
            }

            Piece = piece;
            SquareWidth = squareWidth;
            SquareHeight = squareHeight;
            _IsInitialized = true;

            // 駒の破壊時イベントを登録
            Piece.OnDestroy += DestroyMyself;
        }

        private void DestroyMyself(BoardObject sender) => Destroy(gameObject);

        public static void Create(
            Piece piece,
            float squareWidth,
            float squareHeight,
            Transform parent
        )
        {
            var pieceObject = Instantiate(SystemManager.PiecePrefab, parent, false);
            var component = pieceObject.GetComponent<ComponentPiece>();
            component.Initialize(piece, squareWidth, squareHeight);
        }
    }
}
