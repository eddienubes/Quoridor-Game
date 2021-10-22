using UnityEngine;

namespace Quoridorgame.View
{
    public class WallPlaceHolder : SelectableMonoBehaviour
    {
        [SerializeField]
        private Renderer _renderer;

        public Cell cellParent;

        private void Start()
        {
            OnGetHighlighted(false);
        }

        protected override void OnGetHighlighted(bool isHighlighted)
        {
            _renderer.enabled = isHighlighted;
        }
    }
}