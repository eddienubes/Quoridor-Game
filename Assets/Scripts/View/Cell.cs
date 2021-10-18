using UnityEngine;


namespace Quoridorgame.View
{
    using System;

    /// <summary>
    /// Отображение игровой клетки на сцене
    /// </summary>
    public class Cell : HighlightedSelectable
    {
        [SerializeField, Tooltip("Точка, на которой будут спавниться другие обьекты, \"Находящиеся\" на клетке")]
        private Transform _spawnPoint;

        [SerializeField]
        private WallPlaceHolder _verticalPlaceholder;

        [SerializeField]
        private WallPlaceHolder _horizontalPlaceholder;

        public Vector3 Size { get; private set; }
        public Transform Transform { get; private set; }
        public WallPlaceHolder VerticalPlaceholder => _verticalPlaceholder;
        public WallPlaceHolder HorizontalPlaceholder => _horizontalPlaceholder;

        /// <summary>
        /// Точка, на которой будут спавниться другие обьекты, "Находящиеся" на клетке
        /// </summary>
        public Transform SpawnPoint => _spawnPoint;

        public Vector2Int Coordinate { get; set; }

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