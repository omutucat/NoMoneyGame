using System.Collections.Generic;
using NoMoney.Assets.Scripts.Game.Objects;
using UnityEngine;

namespace NoMoney.Assets.Scripts.Game.Board
{
    public class ComponentSquare : MonoBehaviour
    {
        private BoardPoint Position { get; set; }
        private float SquareWidth { get; set; }
        private float SquareHeight { get; set; }
        private RectTransform MyTransform { get; set; }
        private List<GameObject> Frames { get; set; } = new List<GameObject>();
        private bool IsInitialized { get; set; } = false;

        private void Start()
        {
            MyTransform = GetComponent<RectTransform>();
            MyTransform.sizeDelta = new Vector2(SquareWidth, SquareHeight);

            Frames.Add(transform.Find("Frame").Find("Selected").gameObject);
            Frames.Add(transform.Find("Frame").Find("Movable").gameObject);

            PositionUpdate();
        }

        public void Initialize(BoardPoint position, float squareWidth, float squareHeight)
        {
            if (IsInitialized)
            {
                return;
            }

            Position = position;
            SquareWidth = squareWidth;
            SquareHeight = squareHeight;
            IsInitialized = true;
        }

        private void PositionUpdate()
        {
            var positionX = (Position.X * SquareWidth) + SquareWidth / 2;
            var positionY = (Position.Y * -SquareHeight) - SquareHeight / 2;
            var positionVector = new Vector3(positionX, positionY, 0);
            MyTransform.anchoredPosition = positionVector;
        }

        public void SetFrameState(SquareState state)
        {
            foreach (var frame in Frames)
            {
                frame.SetActive(false);
            }

            if (state == SquareState.Normal)
            {
                return;
            }

            Frames[(int)state].SetActive(true);
        }
    }

    public enum SquareState
    {
        Normal = -1,
        Selected = 0,
        Movable = 1,
    }
}
