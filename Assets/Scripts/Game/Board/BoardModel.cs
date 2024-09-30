using System.Collections.Generic;
using System.Linq;
using NoMoney.Assets.Scripts.Game.GameManager;
using NoMoney.Assets.Scripts.Game.Objects;
using NoMoney.Assets.Scripts.Game.Objects.Pieces;

namespace NoMoney.Assets.Scripts.Game.Board
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
        public List<BoardObject> Objects { get; } = new();
        public BoardSize Size { get; }
        public IEnumerable<Piece> Pieces => Objects.Where(o => o is Piece).Cast<Piece>();

        public BoardModel(BoardSize size, List<BoardObject> objects)
        {
            Size = size;
            objects.ForEach(o => AddObject(o));
        }

        /// <summary>
        /// オブジェクトを追加する
        /// </summary>
        /// <param name="obj"></param>
        /// <exception cref="System.ArgumentException"></exception>
        public void AddObject(BoardObject obj)
        {
            // TODO: 異形オブジェクトの場合の処理を追加する
            if (!IsInsidePoint(obj.Position))
            {
                throw new System.ArgumentException("Invalid position");
            }

            if (Objects.Any(o => o.Position.Equals(obj.Position)))
            {
                throw new System.ArgumentException("Object already exists");
            }

            Objects.Add(obj);
            obj.OnDestroy += OnDestroyObj;
        }

        /// <summary>
        /// オブジェクトの削除時に呼ばれる処理
        /// </summary>
        /// <param name="sender"></param>
        public void OnDestroyObj(BoardObject sender) => Objects.Remove(sender);

        /// <summary>
        /// 指定した座標に存在するオブジェクトを全て返す
        /// </summary>
        /// <param name="point"></param>
        /// <returns></returns>
        public IEnumerable<BoardObject> GetObjectsAt(BoardPoint point) =>
            Objects.Where(obj => obj switch
            {
                IAbnormalShape bigObj => bigObj.ExtraPositions.Contains(point) || bigObj.Position == point,
                _ => obj.Position == point
            });

        /// <summary>
        /// 指定した座標に存在する駒を返す
        /// 存在しない場合はnullを返す
        /// </summary>
        /// <param name="point"></param>
        /// <returns></returns>
        public Piece? FindPieceAt(BoardPoint point) =>
            GetObjectsAt(point).FirstOrDefault(o => o is Piece) as Piece;

        /// <summary>
        /// 各こまのターンエンド処理をする
        /// </summary>
        public void OnTurnChanged(Turn turn) =>
            Objects.Where(o => o is Piece piece && piece.Side == turn.TurnPlayer).Cast<Piece>().ToList()
                .ForEach(p => p.OnTurnChanged());

        /// <summary>
        /// ゲームの状態を判定する
        /// </summary>
        /// <returns></returns>
        public GameStatus JudgeGameState()
        {
            // プレイヤーの駒が上端に到達すれば勝利
            var isWin = Pieces.Any(piece => piece.Side == PieceSide.Player && piece.Position.Y == 0);

            // 敵の駒が下端に到達すれば敗北
            var isLose = Pieces.Any(piece => piece.Side == PieceSide.Enemy && piece.Position.Y == Size.Height - 1);

            // 駒が全て消えれば引き分け
            var isDraw = Pieces.Count() == 0;

            return Pieces switch
            {
                { } when isWin => GameStatus.Win,
                { } when isLose => GameStatus.Lose,
                { } when isDraw => GameStatus.Draw,
                _ => GameStatus.Playing
            };
        }

        /// <summary>
        /// 指定した座標が盤面の範囲内かを返す
        /// </summary>
        /// <param name="point"></param>
        /// <returns></returns>
        public bool IsInsidePoint(BoardPoint point) =>
            point.X >= 0 || point.X < Size.Width || point.Y >= 0 || point.Y < Size.Height;
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