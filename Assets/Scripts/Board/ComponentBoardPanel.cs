using System.Collections.Generic;
using NoMoney.Assets.Scripts.Board;
using NoMoney.Assets.Scripts.Pieces;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

namespace NoMoney.Assets.Scripts.Board
{
    /// <summary>
    /// 盤面を表示するためのコンポーネント
    /// </summary>
    public class ComponentBoardPanel : MonoBehaviour
    {
        [SerializeField] private GameObject _BoardSquarePrefab;

        // テスト用に外からサイズを指定できるように
        [SerializeField] private int _BoardWidth;
        [SerializeField] private int _BoardHeight;

        private void Awake()
        {

        }

        private void Start()
        {
        }

        public void Initialize(BoardModel board)
        {
            // 盤面の初期化処理
            // 自身のWidthとHeightを取得
            var myRectTransform = GetComponent<RectTransform>();

            // 1マスのサイズを計算
            var squareWidth = myRectTransform.rect.width / board.Size.Width;
            var squareHeight = myRectTransform.rect.height / board.Size.Height;

            CreateBoardSquares(board, squareWidth, squareHeight);
            CreateBoardObjects(board, squareWidth, squareHeight);
        }

        /// <summary>
        /// 盤面に存在するオブジェクトを生成する
        /// </summary>
        /// <param name="squareWidth"></param>
        /// <param name="squareHeight"></param>
        private void CreateBoardObjects(BoardModel board, float squareWidth, float squareHeight)
        {
            foreach (var piece in board.Objects.Where(piece => piece is PieceBase))
            {
                ComponentPiece.Create(piece, this.transform, squareWidth, squareHeight);
            }
        }

        /// <summary>
        /// 盤面のマスを生成する
        /// </summary>
        /// <param name="squareWidth"></param>
        /// <param name="squareHeight"></param>
        private void CreateBoardSquares(BoardModel board, float squareWidth, float squareHeight)
        {
            for (var x = 0; x < board.Size.Width; x++)
            {
                for (var y = 0; y < board.Size.Height; y++)
                {
                    var positionX = (x * squareWidth) + squareWidth / 2;
                    var positionY = (y * -squareHeight) - squareHeight / 2;
                    var position = new Vector3(positionX, positionY, 0);
                    var square = Instantiate(_BoardSquarePrefab, this.transform, false);

                    var squareRectTransform = square.GetComponent<RectTransform>();
                    squareRectTransform.anchoredPosition = position;
                    squareRectTransform.sizeDelta = new Vector2(squareWidth, squareHeight);

                    var button = square.GetComponentInChildren<Button>();
                    var point = new Point(x, y);
                    button.onClick.AddListener(() => OnSquareClicked(point));
                }
            }
        }

        private void OnSquareClicked(Point point)
        {
            Debug.Log($"Square Clicked: {point.ToDebugString()}");
        }
    }
}