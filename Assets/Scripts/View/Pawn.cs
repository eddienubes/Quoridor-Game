using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

namespace View
{
    public class Pawn : HighlightedSelectable
    {
        private Transform _transform;
        public float Heigth { get; private set; }

        private void Awake()
        {
            _transform = transform;
            Heigth = GetComponentInChildren<CapsuleCollider>().height;
        }

        public void Jump(Vector3 position, float duration)
            => _transform.DOJump(position, 2, 1, duration);
    }
}