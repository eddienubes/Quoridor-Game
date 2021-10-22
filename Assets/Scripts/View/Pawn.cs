using DG.Tweening;
using UnityEngine;

namespace Quoridorgame.View
{
    /// <summary>
    /// Отображение пешки в юнити
    /// </summary>
    public class Pawn : HighlightedSelectable
    {
        [Header("Animations")]
        [SerializeField]
        [Tooltip("Длительность анимации перемещения")]
        private float _jumpAnimationTime = 2;

        private Transform _transform;

        /// <summary>
        /// Высота пешки
        /// </summary>
        public float Heigth { get; private set; }

        private void Awake()
        {
            _transform = transform;
            Heigth = GetComponentInChildren<CapsuleCollider>().height;
        }

        /// <summary>
        /// Переместить пешку на координату
        /// </summary>
        /// <param name="position"></param>
        /// <param name="duration">Длинна анимации</param>
        public void Jump(Vector3 position)
            => _transform.DOJump(position, 2, 1, _jumpAnimationTime);
    }
}