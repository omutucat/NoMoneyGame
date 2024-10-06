using System.Collections.Generic;
using System.Linq;
using NoMoney.Assets.Scripts.Game.Objects;
using NoMoney.Assets.Scripts.Game.Objects.Pieces;
using UnityEngine;
using UnityEngine.UI;
using static System.Linq.Enumerable;

namespace NoMoney.Assets.Scripts.Game.Board
{
    public struct SquareObject
    {
        public GameObject Square;
        public BoardPoint Point;
    }

    /// <summary>
    /// 盤面を表示するためのコンポーネント
    /// </summary>
    public class ComponentBoardPanel : MonoBehaviour
    {
        private List<IBoardEventListener> _BoardEventListeners = new();
        private GameObject _PiecePrefab;
        private GameObject _BoardSquarePrefab;

        // テスト用に外からサイズを指定できるように
        [SerializeField]
        private int _BoardWidth;

        [SerializeField]
        private int _BoardHeight;

        private List<SquareObject> _SquareObjects = new();

        private void Awake() =>
            _BoardSquarePrefab = Resources.Load<GameObject>("Prefabs/BoardSquare");

        /// <summary>
        /// 盤面の初期化処理
        /// </summary>
        /// <param name="board"></param>
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
                        ComponentPiece.Create(piece, squareWidth, squareHeight, transform);
                        break;
                    default:
                        break;
                }
            }
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
                .SelectMany(x => Range(0, board.Size.Height).Select(y => new BoardPoint(x, y)));

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
        private void CreateBoardSquare(BoardPoint point, float squareWidth, float squareHeight)
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
        private void OnSquareClicked(BoardPoint point)
        {
            Debug.Log($"Square Clicked: {point.ToDebugString()}");
            _BoardEventListeners?.ForEach(listener => listener.OnSquareClick(point));
        }

        /// <summary>
        /// マス目を移動可能状態に設定する
        /// </summary>
        /// <param name="points"></param>
        public void SetMovableSquares(List<BoardPoint> points)
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
        public void SetSquareState(BoardPoint point, SquareState state)
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
