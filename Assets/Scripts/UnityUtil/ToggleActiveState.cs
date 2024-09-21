using UnityEngine;

namespace NoMoney.Assets.Scripts.UnityUtil
{
    /// <summary>
    /// ゲームオブジェクトのアクティブ状態を切り替える
    /// </summary>
    public class ToggleActiveState : MonoBehaviour
    {
        [SerializeField] private GameObject _TargetObject;
        public void ToggleActive() => _TargetObject.SetActive(!_TargetObject.activeSelf);
    }

}