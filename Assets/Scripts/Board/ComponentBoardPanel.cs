using System;
using System.Collections;
using System.Collections.Generic;
using NoMoney.Assets.Scripts.Board;
using UnityEngine;
using UnityEngine.UI;

#pragma warning disable IDE0059 // 値の不必要な代入

public class ComponentBoardPanel : MonoBehaviour
{
    private BoardModel Board { get; set; }

    [SerializeField] private GameObject _BoardSquarePrefab;

    private void Awake()
    {
        var size = new BoardSize(9, 9);
        var objects = new ObjectList();
        Board = new BoardModel(size, objects);
    }

    // Start is called before the first frame update
    private void Start()
    {
        CreateBoardObjects();
    }

    private void CreateBoardObjects()
    {
        var squareWidth = 900f / Board.Size.Width;
        var squareHeight = 900f / Board.Size.Height;

        for (var x = 0; x < Board.Size.Width; x++)
        {
            for (var y = 0; y < Board.Size.Height; y++)
            {
                var positionX = (x * squareWidth) + 50;
                var positionY = (y * -squareHeight) - 50;
                var position = new Vector3(positionX, positionY, 0);
                var square = Instantiate(_BoardSquarePrefab, this.transform, false);
                square.GetComponent<RectTransform>().anchoredPosition = position;
            }
        }
    }

    // Update is called once per frame
    private void Update()
    {

    }
}
