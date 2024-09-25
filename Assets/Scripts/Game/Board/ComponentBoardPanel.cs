using System.Collections.Generic;
using NoMoney.Assets.Scripts.Pieces;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using static System.Linq.Enumerable;

namespace NoMoney.Assets.Scripts.Board
{
    public struct SquareObject
    {
        public GameObject Square;
        public Point Point;
    }

    /// <summary>
    /// 盤面を表示するためのコンポーネント
    /// </summary>
    public class ComponentBoardPanel : MonoBehaviour
    {
        [SerializeField] private List<IBoardEventListener> _BoardEventListeners;
        private GameObject _PiecePrefab;
        private GameObject _BoardSquarePrefab;

        // テスト用に外からサイズを指定できるように
        [SerializeField] private int _BoardWidth;
        [SerializeField] private int _BoardHeight;

        private List<SquareObject> _SquareObjects = new();

        private void Awake()
        {
            _PiecePrefab = Resources.Load<GameObject>("Prefabs/Piece");
            _BoardSquarePrefab = Resources.Load<GameObject>("Prefabs/BoardSquare");
        }

        /// <summary>
        /// 盤面の初期化処理
        /// </summary>
        /// <param name="board"></param>
        public void Initialize(BoardModel board)
        {
            _BoardEventListeners = new List<IBoardEventListener>();
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
        /// リスナーを追加する
        /// </summary>
        /// <param name="listener"></param>
        public void AddListener(IBoardEventListener listener) => _BoardEventListeners.Add(listener);

        /// <summary>
        /// 盤面に存在するオブジェクトを生成する
        /// </summary>
        /// <param name="board"></param>
        /// <param name="squareWidth"></param>
        /// <param name="squareHeight"></param>
        private void CreateBoardObjects(BoardModel board, float squareWidth, float squareHeight)
        {
            foreach (var obj in board.Objects)
            {
                switch (obj)
                {
                    case Piece piece:
                        CreatePieceObject(piece, squareWidth, squareHeight);
                        break;
                    default:
                        break;
                }
            }
        }

        /// <summary>
        /// 駒を生成する
        /// </summary>
        /// <param name="piece"></param>
        /// <param name="squareWidth"></param>
        /// <param name="squareHeight"></param>
        private void CreatePieceObject(Piece piece, float squareWidth, float squareHeight)
        {
            var pieceObject = Instantiate(_PiecePrefab, transform, false);
            var component = pieceObject.GetComponent<ComponentPiece>();
            component.Initialize(piece, squareWidth, squareHeight);
        }

        /// <summary>
        /// 全てのマス目を生成する
        /// </summary>
        /// <param name="board"></param>
        /// <param name="squareWidth"></param>
        /// <param name="squareHeight"></param>
        private void CreateBoardSquares(BoardModel board, float squareWidth, float squareHeight)
        {
            var points = Range(0, board.Size.Width)
                .SelectMany(x => Range(0, board.Size.Height).Select(y => new Point(x, y)));

            foreach (var point in points)
            {
                CreateBoardSquare(point, squareWidth, squareHeight);
            }
        }

        /// <summary>
        /// マス目を生成する
        /// </summary>
        /// <param name="point"></param>
        /// <param name="squareWidth"></param>
        /// <param name="squareHeight"></param>
        private void CreateBoardSquare(Point point, float squareWidth, float squareHeight)
        {
            // 座標を計算してオブジェクトを生成
            var squareObj = Instantiate(_BoardSquarePrefab, transform, false);
            squareObj.GetComponent<ComponentSquare>().Initialize(point, squareWidth, squareHeight);

            _SquareObjects.Add(new SquareObject { Square = squareObj, Point = point });

            var button = squareObj.GetComponentInChildren<Button>();
            button.onClick.AddListener(() => OnSquareClicked(point));
        }

        /// <summary>
        /// マス目がクリックされた時の処理
        /// </summary>
        /// <param name="point"></param>
        private void OnSquareClicked(Point point)
        {
            Debug.Log($"Square Clicked: {point.ToDebugString()}");
            _BoardEventListeners?.ForEach(listener => listener.OnSquareClick(point));
        }

        /// <summary>
        /// マス目を移動可能状態に設定する
        /// </summary>
        /// <param name="points"></param>
        public void SetMovableSquares(List<Point> points)
        {
            ResetSquareStates();

            foreach (var point in points)
            {
                SetSquareState(point, SquareState.Movable);
            }
        }

        /// <summary>
        /// マス目の状態をリセットする
        /// </summary>
        public void ResetSquareStates()
        {
            foreach (var square in _SquareObjects)
            {
                SetSquareState(square.Point, SquareState.Normal);
            }
        }

        /// <summary>
        /// マス目の状態を設定する
        /// </summary>
        /// <param name="point"></param>
        /// <param name="state"></param>
        public void SetSquareState(Point point, SquareState state)
        {
            var square = _SquareObjects.FirstOrDefault(s => s.Point.Equals(point));
            if (square.Square == null)
            {
                return;
            }

            square.Square.GetComponent<ComponentSquare>().SetFrameState(state);
        }
    }
}