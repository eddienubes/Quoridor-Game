using UnityEngine;

namespace View
{
    /// <summary>
    /// Монобех, меняющий подсветку при наведении или клике 
    /// Работает с шейдером Custom/SelectableShader
    /// </summary>
    [RequireComponent(typeof(Renderer))]
    public class HighlightedSelectable : SelectableMonoBehaviour
    {
        private static readonly int IsSelected = Shader.PropertyToID("_isSelected");
        private static readonly int IsHighlighted = Shader.PropertyToID("_isHighlighted");
        
        private Material _material;
        private void Start()
        {
            _material = GetComponent<Renderer>().material;
        }
        protected override void OnGetHighlighted(bool isHighlighted) => SetHighlighted(isHighlighted);
        protected override void OnGetSelected() => SetSelected(true);

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