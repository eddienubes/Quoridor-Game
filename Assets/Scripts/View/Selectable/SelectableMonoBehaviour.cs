using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Quoridorgame.View
{
    /// <summary>
    /// Монобех, отлавливающий наведение мыши и клик по нему
    /// </summary>
    public class SelectableMonoBehaviour : MonoBehaviour
    {
        private EventSystem _cachedEventSystem;
        /// <summary>
        /// EventSystem, который ищется в сцене только один раз
        /// Обернул в свойство для того, чтобы не прописывать Awake (и перегружать) его в каждом наследнике
        /// </summary>
        private EventSystem _eventSystem
        {
            get
            {
                if (_cachedEventSystem == null)
                    _cachedEventSystem = FindObjectOfType<EventSystem>();
                return _cachedEventSystem;
            }
        }

        /// <summary>
        /// True, если компонент реагирует на события наведения\выбора
        /// </summary>
        public bool Interactable { get; set; } = true;

        /// <summary>
        /// Событие нажания на обьект, для подписки извне
        /// </summary>
        public event Action<SelectableMonoBehaviour> OnClicked;

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
            if (_eventSystem.IsPointerOverGameObject()) return;

            OnGetHighlighted(true);
            OnHighlighted?.Invoke(true);
        }

        private void OnMouseExit()
        {
            if (!Interactable) return;
            if (_eventSystem.IsPointerOverGameObject()) return;

            OnGetHighlighted(false);
            OnHighlighted?.Invoke(false);
        }

        private void OnMouseDown()
        {
            if (!Interactable) return;
            if (_eventSystem.IsPointerOverGameObject()) return;

            OnGetSelected();
            OnClicked?.Invoke(this);
        }
    }
}