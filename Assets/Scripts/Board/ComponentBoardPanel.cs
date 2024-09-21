using System.Collections.Generic;
using NoMoney.Assets.Scripts.Board;
using NoMoney.Assets.Scripts.Pieces;
using UnityEngine;
using System.Linq;

namespace NoMoney.Assets.Scripts.Board
{
    /// <summary>
    /// 盤面を表示するためのコンポーネント
    /// </summary>
    public class ComponentBoardPanel : MonoBehaviour
    {
        private BoardModel Board { get; set; }

        [SerializeField] private GameObject _BoardSquarePrefab;

        // テスト用に外からサイズを指定できるように
        [SerializeField] private int _BoardWidth;
        [SerializeField] private int _BoardHeight;

        private void Awake()
        {
            // TODO: 本来は外部から受け取る
            var size = new BoardSize(_BoardWidth, _BoardHeight);
            var pieces = new List<PieceBase>
        {
            new Pawn(new Point(0, 0)),
            new Pawn(new Point(1, 0)),
        };
            Board = new BoardModel(size, pieces);
        }

        private void Start()
        {
            // 自身のWidthとHeightを取得
            var myRectTransform = GetComponent<RectTransform>();

            // 1マスのサイズを計算
            var squareWidth = myRectTransform.rect.width / Board.Size.Width;
            var squareHeight = myRectTransform.rect.height / Board.Size.Height;

            CreateBoardSquares(squareWidth, squareHeight);
            CreateBoardObjects(squareWidth, squareHeight);
        }

        /// <summary>
        /// 盤面に存在するオブジェクトを生成する
        /// </summary>
        /// <param name="squareWidth"></param>
        /// <param name="squareHeight"></param>
        private void CreateBoardObjects(float squareWidth, float squareHeight)
        {
            foreach (var piece in Board.Objects.Where(piece => piece is PieceBase))
            {
                ComponentPiece.Create(piece, this.transform, squareWidth, squareHeight);
            }
        }

        /// <summary>
        /// 盤面のマスを生成する
        /// </summary>
        /// <param name="squareWidth"></param>
        /// <param name="squareHeight"></param>
        private void CreateBoardSquares(float squareWidth, float squareHeight)
        {
            for (var x = 0; x < Board.Size.Width; x++)
            {
                for (var y = 0; y < Board.Size.Height; y++)
                {
                    var positionX = (x * squareWidth) + squareWidth / 2;
                    var positionY = (y * -squareHeight) - squareHeight / 2;
                    var position = new Vector3(positionX, positionY, 0);
                    var square = Instantiate(_BoardSquarePrefab, this.transform, false);

                    var squareRectTransform = square.GetComponent<RectTransform>();
                    squareRectTransform.anchoredPosition = position;
                    squareRectTransform.sizeDelta = new Vector2(squareWidth, squareHeight);
                }
            }
        }
    }
}