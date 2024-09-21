using System;
using System.Collections.Generic;
using NoMoney.Assets.Scripts.Board;
using NoMoney.Assets.Scripts.Pieces;
using UnityEngine;

public class ComponentBoardPanel : MonoBehaviour
{
    private BoardModel Board { get; set; }

    [SerializeField] private GameObject _BoardSquarePrefab;

    // テスト用に外からサイズを指定できるように
    [SerializeField] private int _BoardWidth;
    [SerializeField] private int _BoardHeight;

    private void Awake()
    {
        var size = new BoardSize(_BoardWidth, _BoardHeight);
        var pieces = new List<PieceBase>
        {
            new Porn(new Point(0, 0)),
            new Porn(new Point(1, 0)),
        };
        var objects = new ObjectList(pieces);
        Board = new BoardModel(size, objects);
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

    private void CreateBoardObjects(float squareWidth, float squareHeight)
    {
        foreach (var piece in Board.Objects.Pieces)
        {
            ComponentPiece.Create(piece, this.transform, squareWidth, squareHeight);
        }
    }

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

    // ボードが正当かチェックする。オブジェクトが重なっていないか、座標が範囲内かを確認する
    public bool IsvalidBoard()
    {
        foreach (var obj in Board.Objects.Pieces)
        {
            // 座標が範囲内か
            if (obj.Position.X < 0 || obj.Position.X >= Board.Size.Width)
            {
                return false;
            }
            if (obj.Position.Y < 0 || obj.Position.Y >= Board.Size.Height)
            {
                return false;
            }
            // その場所に他のオブジェクトが存在していないか Ghostは重なっても良い
            if (obj.Type == PieceType.Ghost)
            {
                continue;
            }
            var objects = GetObjectsAt(obj.Position);
            if (objects.Count > 1)
            {
                foreach (var other in objects)
                {
                    if (other != obj && other.Type != PieceType.Ghost)
                    {
                        return false;
                    }
                }
            }
        }

        return true;
    }

    // その場所に存在するオブジェクトを全て返す
    private List<PieceBase> GetObjectsAt(Point point)
    {
        var objects = new List<PieceBase>();
        foreach (var obj in Board.Objects.Pieces)
        {
            if (obj.Position.X == point.X && obj.Position.Y == point.Y)
            {
                objects.Add(obj);
            }
        }

        return objects;
    }

    // TODO オブジェクトの移動可能な場所を返す
    public List<Point> GetMovablePoints(PieceBase piece)
    {
        var points = new List<Point>();
        foreach (var point in piece.MoveablePoints)
        {
            if (point.X < 0 || point.X >= Board.Size.Width)
            {
                continue;
            }
            if (point.Y < 0 || point.Y >= Board.Size.Height)
            {
                continue;
            }
            var objects = GetObjectsAt(point);
            if (objects.Count == 0)
            {
                points.Add(point);
            }
            else
            {
                foreach (var obj in objects)
                {
                    if (obj.Type == PieceType.Ghost)
                    {
                        points.Add(point);
                    }
                }
            }
        }

        return points;
    }

    // Update is called once per frame
    private void Update()
    {

    }
}
