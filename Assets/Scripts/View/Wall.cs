using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace View
{
    public class Wall : MonoBehaviour
    {
        public Transform Transform { get; private set; }
        public float Width { get; private set; }
        
        /// <summary>
        /// True - стенка поставлена вертикально, false - Горизонтально
        /// </summary>
        public bool Vertical
        {
            get => Math.Abs(Transform.rotation.y - 90) < 0.001;
            set
            {
                var rot = Transform.rotation.eulerAngles;
                rot.y = value ? 90 : 0;
                Transform.rotation = Quaternion.Euler(rot);
            }
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