using System;
using UnityEngine;

namespace Quoridorgame.View
{
    /// <summary>
    /// Монобех, отлавливающий наведение мыши и клик по нему
    /// </summary>
    public class SelectableMonoBehaviour : MonoBehaviour
    {
        /// <summary>
        /// True, если компонент реагирует на события наведения\выбора
        /// </summary>
        public bool Interactable { get; set; } = true;

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
            if (!Interactable) return;
            OnGetHighlighted(true);
            OnHighlighted?.Invoke(true);
        }

        private void OnMouseExit()
        {
            if (!Interactable) return;
            OnGetHighlighted(false);
            OnHighlighted?.Invoke(false);
        }

        private void OnMouseDown()
        {
            if (!Interactable) return;
            OnGetSelected();
            OnClicked?.Invoke();
        }
    }
}