using UnityEngine;
using UnityEngine.SceneManagement;

namespace NoMoney.Assets.Scripts.UnityUtil
{
    /// <summary>
    /// シーン遷移を行う
    /// </summary>
    public class SceneMove : MonoBehaviour
    {
        [SerializeField] private string _SceneName;
        public void MoveScene() => SceneManager.LoadScene(_SceneName ?? "SampleScene");
    }
}