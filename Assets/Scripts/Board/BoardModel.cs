using System.Linq;
using System.Collections.Generic;
using NoMoney.Assets.Scripts.Pieces;

namespace NoMoney.Assets.Scripts.Board
{
    public enum GameStatus
    {
        Playing,
        Win,
        Lose,
        Draw
    }

    /// <summary>
    /// 盤面を表すクラス
    /// </summary>
    public class BoardModel
    {
        public List<BoardObject> Objects { get; }
        public BoardSize Size { get; }

        public BoardModel(BoardSize size, List<BoardObject> objects)
        {
            Size = size;
            Objects = objects;

            if (!IsValidBoard())
            {
                throw new System.ArgumentException("Invalid board");
            }

            Objects.Where(o => o is Piece).Cast<Piece>().ToList().ForEach(p => p.OnDestroy += OnDestroyPiece);
        }

        /// <summary>
        /// ボードが正当かチェックする。オブジェクトが重なっていないか、座標が範囲内かを確認する
        /// </summary>
        /// <returns></returns>
        public bool IsValidBoard()
        {
            // 不正な座標に存在するオブジェクトがあるか
            var isExistIllegalPosition = Objects.Any(o => IsPositionOutsideBounds(o.Position));

            // 重なっているオブジェクトがあるか
            // NOTE: Pointは構造体なので同値性比較ができる
            var isExistOverlapping = Objects.GroupBy(o => o.Position).Any(g => g.Count() > 1);

            return !isExistIllegalPosition && !isExistOverlapping;
        }

        /// <summary>
        /// 指定した場所に存在する移動可能な駒を返す
        /// </summary>
        /// <param name="point"></param>
        /// <returns></returns>
        public Piece GetMovablePiecesAt(Point point)
        {
            var pieces = Objects.Where(o => o.Position.Equals(point) && o is Piece).Cast<Piece>().ToList();
            return pieces.FirstOrDefault(piece => GetMovablePoints(piece).Count > 0);
        }

        public void OnDestroyPiece(BoardObject sender) => Objects.Remove(sender);

        /// <summary>
        /// 指定した座標に存在するオブジェクトを全て返す
        /// </summary>
        /// <param name="point"></param>
        /// <returns></returns>
        public List<BoardObject> GetObjectsAt(Point point) =>
            Objects.Where(o => o.Position.Equals(point)).ToList();

        /// <summary>
        /// 指定したピースが移動可能な座標を返す
        /// </summary>
        /// <param name="piece"></param>
        /// <returns></returns>
        public List<Point> GetMovablePoints(Piece piece) => piece switch
        {
            Piece p when p.StatusList.Any(s => s is Immobilized or InSleep) => new List<Point>(),
            { } tp and ITeleportable => GetMovablePointsTeleportable(tp),
            { } pc => pc.MoveablePoints.Where(p => !IsPositionOutsideBounds(p)).ToList(),
            _ => new List<Point>()
        };

        /// <summary>
        /// 各こまのターンエンド処理をする
        /// </summary>
        public void OnTurnChanged(PieceSide side) =>
            // 指定したサイドの駒のターンエンド処理をする
            Objects.Where(o => o is Piece piece && piece.Side == side).Cast<Piece>().ToList()
                .ForEach(p => p.OnTurnEnd());

        /// <summary>
        /// テレポート可能な駒の移動可能な座標を返す
        /// </summary>
        /// <param name="piece"></param>
        /// <returns></returns>
        private List<Point> GetMovablePointsTeleportable(Piece piece)
        {
            // ITeleportableは盤面の端を超えると逆の端に移動出来る
            var points = piece.MoveablePoints.Select(p =>
            {
                var x = (p.X + Size.Width) % Size.Width;
                var y = (p.Y + Size.Height) % Size.Height;
                return new Point(x, y);
            }).ToList();

            return points;
        }

        /// <summary>
        /// ゲームの状態を判定する
        /// </summary>
        /// <returns></returns>
        public GameStatus JudgeGameState()
        {
            var pieces = Objects.Where(o => o is Piece).Cast<Piece>().ToList();

            // プレイヤーの駒が上端に到達すれば勝利
            bool isWin(List<Piece> pieces) => pieces.Any(piece => piece.Side == PieceSide.Player && piece.Position.Y == 0);

            // 敵の駒が下端に到達すれば敗北
            bool isLose(List<Piece> pieces) => pieces.Any(piece => piece.Side == PieceSide.Enemy && piece.Position.Y == Size.Height - 1);

            // 駒が全て消えれば引き分け
            bool isDraw(List<Piece> pieces) => pieces.Count == 0;

            return pieces switch
            {
                { } when isWin(pieces) => GameStatus.Win,
                { } when isLose(pieces) => GameStatus.Lose,
                { } when isDraw(pieces) => GameStatus.Draw,
                _ => GameStatus.Playing
            };
        }

        public bool TryMovePiece(Piece piece, Point point) => piece.TryMove(point, this);

        /// <summary>
        /// 指定した座標が盤面の範囲外かを返す
        /// </summary>
        /// <param name="point"></param>
        /// <returns></returns>
        private bool IsPositionOutsideBounds(Point point) =>
            point.X < 0 || point.X >= Size.Width || point.Y < 0 || point.Y >= Size.Height;

    }

    public class BoardSize
    {
        private int _Width;
        private int _Height;
        public int Width
        {
            get => _Width;
            private set => _Width = value switch
            {
                < 1 => throw new System.ArgumentOutOfRangeException(),
                _ => value
            };
        }
        public int Height
        {
            get => _Height;
            private set => _Height = value switch
            {
                < 1 => throw new System.ArgumentOutOfRangeException(),
                _ => value
            };
        }

        public BoardSize(int width, int height)
        {
            Width = width;
            Height = height;
        }
    }
}
