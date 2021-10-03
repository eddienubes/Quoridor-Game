using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace View
{
    public class FieldCreator : MonoBehaviour
    {
        [SerializeField] private Transform _root;
        [SerializeField] private Cell _cellPrefab;
        [SerializeField] private Wall _wallPrefab;
        [SerializeField] private Transform _fieldGoRoot;
        private Cell[,] _cells;

        private void Start()
        {
            CreateField(8, 8);
        }

        public void CreateField(int xSize, int ySize)
        {
            _cells = new Cell[xSize, ySize];

            var wallSample = Instantiate(_wallPrefab, _root);
            var wallWidth = wallSample.Width;
            Destroy(wallSample.gameObject);

            for (int x = 0; x < xSize; x++)
            {
                for (int y = 0; y < ySize; y++)
                {
                    var cellGo = Instantiate(_cellPrefab, _root);
                    var xCoord = (cellGo.Size.x * x) + (wallWidth * (x - 1));
                    var yCoord = (cellGo.Size.z * y) + (wallWidth * (y - 1));
                    cellGo.transform.localPosition = new Vector3(xCoord, 0, yCoord);
                    cellGo.name = $"Cell {x}-{y}";
                    _cells[x, y] = cellGo;
                }
            }

            RecalculateFieldSize(xSize, ySize, wallWidth);
        }

        /// <summary>
        /// Перерасчитывает размеры игрового поля
        /// </summary>
        /// <param name="xSize">Кол-во клеток по оси Х</param>
        /// <param name="ySize">Кол-во клеток по оси У</param>
        /// <param name="wallWidth">Ширина стенки</param>
        private void RecalculateFieldSize(int xSize, int ySize, float wallWidth)
        {
            var fieldRootXSize = (_cells[0, 0].Size.x * xSize) + (wallWidth * (xSize + 1));
            var fieldRootYSize = _fieldGoRoot.localScale.y;
            var fieldRootZSize = (_cells[0, 0].Size.z * ySize) + (wallWidth * (ySize + 1));

            _fieldGoRoot.localScale = new Vector3(fieldRootXSize, fieldRootYSize, fieldRootZSize) * 1.5f;

            var fieldRootXPos = 0f;
            if (xSize % 2 != 0)
                fieldRootXPos = _cells[Mathf.CeilToInt(xSize / 2), 0].Transform.position.x;
            else
            {
                var pos1 = _cells[xSize / 2, 0].Transform.position.x;
                var pos2 = _cells[(xSize / 2) - 1, 0].transform.position.x;
                fieldRootXPos = (pos1 + pos2) / 2;
            }

            var fieldRootZPos = 0f;
            if (ySize % 2 != 0)
                fieldRootZPos = _cells[0, Mathf.CeilToInt(ySize / 2)].Transform.position.z;
            else
            {
                var pos1 = _cells[0, ySize / 2].Transform.position.z;
                var pos2 = _cells[0, (ySize / 2) - 1].transform.position.z;
                fieldRootZPos = (pos1 + pos2) / 2;
            }

            _fieldGoRoot.transform.localPosition = new Vector3(fieldRootXPos, -2, fieldRootZPos);
        }
    }
}