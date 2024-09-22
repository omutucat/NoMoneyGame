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
        // TODO: Pieceではなく盤面に存在するあらゆるオブジェクトを持てるようにする
        public List<BoardObject> Objects { get; }
        public BoardSize Size { get; }

        public BoardModel(BoardSize size, List<BoardObject> objects)
        {
            Size = size;
            Objects = objects;
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
            // NOTE: IGhostは重なっても問題ないので除外する
            var isExistOverlapping = Objects.GroupBy(o => o.Position).Any(g => g.Count(o => o is not IGhost) > 1);

            return !isExistIllegalPosition && !isExistOverlapping;
        }

        /// <summary>
        /// 指定した場所に存在する移動可能な駒を返す
        /// </summary>
        /// <param name="point"></param>
        /// <returns></returns>
        public PieceBase GetMovablePiecesAt(Point point)
        {
            var pieces = Objects.Where(o => o.Position.Equals(point) && o is PieceBase).Cast<PieceBase>().ToList();
            return pieces.FirstOrDefault(piece => GetMovablePoints(piece).Count > 0);
        }

        /// <summary>
        /// 指定した座標に存在するオブジェクトを全て返す
        /// </summary>
        /// <param name="point"></param>
        /// <returns></returns>
        private List<BoardObject> GetObjectsAt(Point point) =>
            Objects.Where(o => o.Position.X == point.X && o.Position.Y == point.Y).ToList();

        /// <summary>
        /// 指定したピースが移動可能な座標を返す
        /// </summary>
        /// <param name="piece"></param>
        /// <returns></returns>
        public List<Point> GetMovablePoints(PieceBase piece) => piece switch
        {
            { } tp and ITeleportable => GetMovablePointsTeleportable(tp),
            { } ghost and IGhost => ghost.MoveablePoints.Where(p => !IsPositionOutsideBounds(p)).ToList(),
            { } pc => pc.MoveablePoints.Where(p => !IsPositionOutsideBounds(p) && GetObjectsAt(p).Count == 0).ToList()
        };

        /// <summary>
        /// テレポート可能な駒の移動可能な座標を返す
        /// </summary>
        /// <param name="piece"></param>
        /// <returns></returns>
        private List<Point> GetMovablePointsTeleportable(PieceBase piece)
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
            var pieces = Objects.Where(o => o is PieceBase).Cast<PieceBase>().ToList();

            // プレイヤーの駒が上端に到達すれば勝利
            bool isWin(List<PieceBase> pieces) => pieces.Any(piece => piece.Side == PieceSide.Player && piece.Position.Y == 0);

            // 敵の駒が下端に到達すれば敗北
            bool isLose(List<PieceBase> pieces) => pieces.Any(piece => piece.Side == PieceSide.Enemy && piece.Position.Y == Size.Height - 1);

            // 駒が全て消えれば引き分け
            bool isDraw(List<PieceBase> pieces) => pieces.Count == 0;

            return pieces switch
            {
                { } when isWin(pieces) => GameStatus.Win,
                { } when isLose(pieces) => GameStatus.Lose,
                { } when isDraw(pieces) => GameStatus.Draw,
                _ => GameStatus.Playing
            };
        }

        /// <summary>
        /// 駒を動かす
        /// </summary>
        /// <param name="piece"></param>
        /// <param name="point"></param>
        /// <exception cref="ArgumentException"></exception>
        public void MovePiece(PieceBase piece, Point point)
        {
            if (!GetMovablePoints(piece).Contains(point))
            {
                throw new System.ArgumentException("Invalid move");
            }

            piece.SetPosition(point);
        }

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
            private set
            {
                if (value < 1)
                {
                    throw new System.ArgumentOutOfRangeException();
                }
                _Width = value;
            }
        }
        public int Height
        {
            get => _Height;
            private set
            {
                if (value < 1)
                {
                    throw new System.ArgumentOutOfRangeException();
                }
                _Height = value;
            }
        }

        public BoardSize(int width, int height)
        {
            Width = width;
            Height = height;
        }
    }
}
