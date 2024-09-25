using NoMoney.Assets.Scripts.Game.Objects.Pieces;
using UnityEngine;
using UnityEngine.UI;

namespace NoMoney.Assets.Scripts.Game.GameManager
{
    public class ComponentTurn : MonoBehaviour
    {
        [SerializeField] private Text _TurnCountText;
        [SerializeField] private GameObject _PlayerTurnBackground;
        [SerializeField] private GameObject _EnemyTurnBackground;

        public void OnTurnChanged(Turn turn)
        {
            _TurnCountText.text = turn.Count.ToString();

            switch (turn.TurnPlayer)
            {
                case PieceSide.Player:
                    _PlayerTurnBackground.SetActive(true);
                    _EnemyTurnBackground.SetActive(false);
                    break;
                case PieceSide.Enemy:
                    _PlayerTurnBackground.SetActive(false);
                    _EnemyTurnBackground.SetActive(true);
                    break;
            }
        }
    }
}