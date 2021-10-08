using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace View
{
    public class Wall : MonoBehaviour
    {
        public Vector3 Size { get; private set; }
        public Transform Transform { get; private set; }

        public float Width { get; private set; }

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
            Size = new Vector3(
                colliderSize.x * transformSize.x,
                colliderSize.y * transformSize.y,
                colliderSize.z * transformSize.z
            );
            Width = Size.z;
        }
    }
}