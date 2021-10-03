using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace View
{
    [RequireComponent(typeof(BoxCollider))]
    public class Wall : MonoBehaviour
    {
        public Vector3 Size { get; private set; }
        public Transform Transform { get; private set; }
        
        public float Width{ get; private set; }
        private void Awake()
        {
            Transform = transform;
            var transformSize = Transform.localScale;
            var colliderSize = GetComponent<BoxCollider>().size;
            Size = new Vector3(
                colliderSize.x * transformSize.x,
                colliderSize.y * transformSize.y,
                colliderSize.z * transformSize.z
            );
            Width = Size.z;
        }
    }
}