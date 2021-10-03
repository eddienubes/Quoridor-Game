using System;
using UnityEngine;


namespace View
{
    public class Cell : MonoBehaviour
    {
        public Vector3 Size { get; private set; }
        public Transform Transform { get; private set; }

        public Action OnClicked;
        private void OnMouseEnter()
        {
            GetComponent<Renderer>().material.SetColor("_Color", Color.red);
        }

        private void OnMouseExit()
        {
            GetComponent<Renderer>().material.SetColor("_Color", Color.blue);
        }

        private void OnMouseDown()
        {
            GetComponent<Renderer>().material.SetColor("_Color", Color.yellow);
            OnClicked?.Invoke();
        }

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
        }
    }
}