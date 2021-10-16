using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

namespace View
{
    /// <summary>
    /// Отображение пешки в юнити
    /// </summary>
    public class Pawn : HighlightedSelectable
    {
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
        public void Jump(Vector3 position, float duration)
            => _transform.DOJump(position, 2, 1, duration);
    }
}