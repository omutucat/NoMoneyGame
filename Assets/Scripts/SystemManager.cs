using NoMoney.Assets.Scripts.Game.Board;
using UnityEngine;

namespace NoMoney.Assets.Scripts
{
    public class SystemManager : MonoBehaviour
    {
        public static StageName SelectedStage { get; set; } = StageName.Stage1;
        public static SystemManager? Instance { get; private set; }

        private static GameObject? PIECE_PREFAB;
        public static GameObject? PiecePrefab
        {
            get
            {
                if (PIECE_PREFAB == null)
                {
                    PIECE_PREFAB = Resources.Load<GameObject>("Prefabs/Piece");
                }
                return PIECE_PREFAB;
            }
        }

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject);
            }
        }
    }
}