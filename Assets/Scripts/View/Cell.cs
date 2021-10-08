using System;
using Codice.Client.BaseCommands;
using UnityEngine;


namespace View
{
    public class Cell : HighlightedSelectable
    {
        [SerializeField] private Transform _spawnPoint;

        public Vector3 Size { get; private set; }
        public Transform Transform { get; private set; }
        public Transform SpawnPoint => _spawnPoint;

        #region UnityEvents

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

        #endregion
    }
}