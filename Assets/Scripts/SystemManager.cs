using NoMoney.Assets.Scripts.Game.Board;
using UnityEngine;

namespace NoMoney.Assets.Scripts
{
    public class SystemManager : MonoBehaviour
    {
        public static StageName CurrentStage { get; set; } = StageName.Stage1;
        public static SystemManager? Instance { get; private set; }

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
