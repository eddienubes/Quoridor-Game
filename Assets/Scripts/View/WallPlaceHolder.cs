using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Quoridorgame.View
{
    public class WallPlaceHolder : SelectableMonoBehaviour
    {
        [SerializeField] private Renderer _renderer;

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