using UnityEngine;

namespace Quoridorgame.View
{
    /// <summary>
    /// Монобех, меняющий подсветку при наведении или клике 
    /// Работает с шейдером Custom/SelectableShader
    /// </summary>
    [RequireComponent(typeof(Collider))]
    public class HighlightedSelectable : SelectableMonoBehaviour
    {
        private static readonly int IsSelected = Shader.PropertyToID("_isSelected");
        private static readonly int IsHighlighted = Shader.PropertyToID("_isHighlighted");
        
        [SerializeField] private Renderer renderTarget;

        protected override void OnGetHighlighted(bool isHighlighted) => SetHighlighted(isHighlighted);
        protected override void OnGetSelected() => SetSelected(true);

        public void SetSelected(bool isSelected)
        {
            renderTarget.material.SetFloat(IsSelected, isSelected ? 1 : 0);
        }

        public void SetHighlighted(bool isHighlighted)
        {
            renderTarget.material.SetFloat(IsHighlighted, isHighlighted ? 1 : 0);
        }
    }
}