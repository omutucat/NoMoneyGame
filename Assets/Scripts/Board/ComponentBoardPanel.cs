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
}
