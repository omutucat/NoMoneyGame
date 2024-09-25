using System;

namespace NoMoney.Assets.Scripts.Game.Objects
{
    /// <summary>
    /// 盤面上のオブジェクト
    /// </summary>
    public abstract class BoardObject
    {
        /// <summary>
        /// オブジェクトが破壊される時に呼び出すイベント
        /// </summary>
        /// <param name="sender"></param>
        public delegate void DestroyEventHandler(BoardObject sender);

        /// <summary>
        /// オブジェクトが破壊される時に発火するイベント
        /// </summary>
        public event DestroyEventHandler OnDestroy;

        /// <summary>
        /// オブジェクトの位置
        /// </summary>
        public BoardPoint Position { get; protected set; }

        protected BoardObject(BoardPoint position) => Position = position;

        /// <summary>
        /// 破壊される時に呼び出すメソッド
        /// OnDestroyイベントを発火する
        /// </summary>
        public virtual void Destroy() => OnDestroy?.Invoke(this);
    }
}