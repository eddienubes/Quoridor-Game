using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

namespace Quoridorgame.View
{
    public class Wall : MonoBehaviour
    {
        public Transform Transform { get; private set; }
        public float Width { get; private set; }

        [Header("Animations")] [SerializeField] [Tooltip("Длительность анимации перемещения")]
        private float _jumpAnimationTime = 2;

        [SerializeField]
        [Tooltip("Часть длительности от общего времени, которое стенка будет двигаться вертикально вниз ")]
        [Range(0, 1)]
        private float _verticalMovementTimePart = 0.2f;

        [SerializeField] [Tooltip("Высота, с которой стенка будет двигаться вертикально вниз ")] [Range(0, 1)]
        private float _verticalMovementHeight = 1.4f;

        /// <summary>
        /// True - стенка поставлена вертикально, false - Горизонтально
        /// </summary>
        public bool IsVertical
        {
            get => Math.Abs(Transform.rotation.y - 90) < 0.001;
            set => Transform.rotation = AxisRotation(value);
        }

        /// <summary>
        /// </summary>
        /// <param name="isVertical">true если стенка должна стоять вертикально</param>
        /// <returns>Поворот стенки в зависимости от того, стоит она вертикально, или горизонтально</returns>
        private Quaternion AxisRotation(bool isVertical)
        {
            var rot = Transform.rotation.eulerAngles;
            rot.y = isVertical ? 90 : 0;
            return Quaternion.Euler(rot);
        }

        /// <summary>
        /// Переместить стенку анимированно
        /// </summary>
        /// <param name="position">Позиция</param>
        /// <param name="isVertical">true, если стенку нужно установить в вертикальное положение</param>
        /// <param name="duration">Длительность анимации</param>
        public void Jump(Vector3 position, bool isVertical)
        {
            var jumpDuration = _jumpAnimationTime * _verticalMovementTimePart;
            DOTween.Sequence()
                .Append(Transform.DOJump(position + Vector3.up * _verticalMovementHeight, 2, 1, jumpDuration))
                .Insert(0, Transform.DORotate(AxisRotation(isVertical).eulerAngles, jumpDuration))
                .onComplete = () => { Transform.DOMoveY(position.y, _jumpAnimationTime - jumpDuration); };
        }

        private void Awake()
        {
            Transform = transform;
            var transformSize = Transform.localScale;
            var colliderSize = GetComponentInChildren<BoxCollider>().size;
            Width = colliderSize.z * transformSize.z;
        }
    }
}