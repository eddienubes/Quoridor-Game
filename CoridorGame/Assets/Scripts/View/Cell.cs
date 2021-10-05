using System;
using Codice.Client.BaseCommands;
using UnityEngine;


namespace View
{
    public class Cell : MonoBehaviour
    {
        public Vector3 Size { get; private set; }
        public Transform Transform { get; private set; }

        public event Action OnClicked;

        private Material _material;

        private static readonly int IsSelected = Shader.PropertyToID("_isSelected");
        private static readonly int IsHighlighted = Shader.PropertyToID("_isHighlighted");


        #region UnityEvents

        private void OnMouseEnter()
        {
            SetHighlighted(true);
        }

        private void OnMouseExit()
        {
            SetHighlighted(false);
        }

        private void OnMouseDown()
        {
            SetHighlighted(true);
            OnClicked?.Invoke();
        }

        private void Awake()
        {
            Transform = transform;

            var transformSize = Transform.localScale;
            _material = GetComponent<Renderer>().material;
            var colliderSize = GetComponent<BoxCollider>().size;
            Size = new Vector3(
                colliderSize.x * transformSize.x,
                colliderSize.y * transformSize.y,
                colliderSize.z * transformSize.z
            );
        }

        #endregion

        public void SetSelected(bool isSelected)
        {
            _material.SetFloat(IsSelected, isSelected ? 1 : 0);
        }

        public void SetHighlighted(bool isHighlighted)
        {
            _material.SetFloat(IsHighlighted, isHighlighted ? 1 : 0);
        }
    }
}