using System;
using UnityEngine;

namespace View
{
    /// <summary>
    /// Монобех, отлавливающий наведение мыши и клик по нему
    /// </summary>
    public class SelectableMonoBehaviour : MonoBehaviour
    {
        /// <summary>
        /// Событие нажания на обьект, для подписки извне
        /// </summary>
        public event Action OnClicked;

        /// <summary>
        /// Событие наведения на обьект, для подписки извне
        /// </summary>
        public event Action<bool> OnHighlighted;

        /// <summary>
        /// Вызывается при наведении на обьект, переопределяется в наследниках, чтоб не засорять события лишней логикой
        /// </summary>
        /// <param name="isHighlighted">true если на обьект навели, false - если убрали курсор</param>
        protected virtual void OnGetHighlighted(bool isHighlighted)
        {
        }
        /// <summary>
        /// Вызывается при клике на обьект, переопределяется в наследниках, чтоб не засорять события лишней логикой
        /// </summary>
        protected virtual void OnGetSelected()
        {
        }

        private void OnMouseEnter()
        {
            OnGetHighlighted(true);
            OnHighlighted?.Invoke(true);
        }

        private void OnMouseExit()
        {
            OnGetHighlighted(false);
            OnHighlighted?.Invoke(false);
        }

        private void OnMouseDown()
        {
            OnGetSelected();
            OnClicked?.Invoke();
        }
    }
}