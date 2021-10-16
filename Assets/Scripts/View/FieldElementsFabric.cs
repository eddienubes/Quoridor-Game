using System;
using UnityEngine;

namespace View
{
    public class FieldElementsFabric : MonoBehaviour
    {
        [SerializeField] private Transform _root;
        [SerializeField] private Cell _cellPrefab;
        [SerializeField] private Wall _wallPrefab;
        [SerializeField] private Pawn _pawnPrefab;
        [SerializeField] private Transform _fieldGoRoot;

        [SerializeField] private CameraRotatorBase _cameraRotator;
        private Cell[,] _cells = new Cell[0, 0];

        //TODO: сделано для стартовой демки, выпилить после создания контроллера
        private void Start()
        {
            CreateField(9, 9);
            _cameraRotator.Init();
            SpawnPawn(4, 0);
            SpawnPawn(4, 8);
            SpawnWall(1, 2, 2, 3, true);
            SpawnWall(4, 2, 5, 3, false);
        }
        //TODO: сделано для стартовой демки, выпилить после создания контроллера
        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                _cameraRotator.RotateCamera();
            }
        }
        
        /// <summary>
        /// Создания игрового поля 
        /// </summary>
        /// <param name="xSize">кол-во клеток по оси Х</param>
        /// <param name="ySize">кол-во клеток по оси У</param>
        /// <returns>Сгенерированное поле</returns>
        public Cell[,] CreateField(int xSize, int ySize)
        {
            foreach (var cell in _cells)
                Destroy(cell.gameObject);
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
            return _cells;
        }
        /// <summary>
        /// Спавн пешки
        /// </summary>
        /// <param name="x">Координата клетки, на которой нужно заспавнить пешку по оси Х</param>
        /// <param name="y">Координата клетки, на которой нужно заспавнить пешку по оси У</param>
        /// <returns></returns>
        /// <exception cref="ArgumentException">Бросается при невалидных координатах</exception>
        public Pawn SpawnPawn(int x, int y)
        {
            if (!IsCoordsValid(x, y))
                throw new ArgumentException("Pawn coordinates are incorrect!");

            var pawnGo = Instantiate(_pawnPrefab, _cells[x, y].SpawnPoint, true);
            pawnGo.transform.localPosition = Vector3.zero;
            return pawnGo;
        }
        
        /// <summary>
        /// Спавнит стенку по выбранным координатам клетки
        /// </summary>
        /// <param name="x1">Координата Х первой клетки</param>
        /// <param name="y1">Координата У первой клетки</param>
        /// <param name="x2">Координата Х второй клетки</param>
        /// <param name="y2">Координата У второй клетки</param>
        /// <param name="vertical">true если стенка должна стоять вертикально</param>
        /// <returns>Заспавненную стенку</returns>
        /// <remarks>
        /// <para>Клетки нужно задавать "по диагонали" </para>
        /// <para> .|О     0|.    .||О     0||. </para>
        /// <para> ===     ===    ---      ---  </para>
        /// <para> О|.     .|0    О||.     .||0 </para>
        ///
        /// <para>где . - клетка, 0 - клетка, координату которой нужно указать, == || стенка, - | просто разделение между сеткой   </para>
        /// </remarks>
        /// <exception cref="ArgumentException">Бросается, если координаты сетки были заданы неверно</exception>
        public Wall SpawnWall(int x1, int y1, int x2, int y2, bool vertical)
        {
            if (!IsCoordsValid(x1, y1) || !IsCoordsValid(x2, y2))
                throw new ArgumentException("Pawn coordinates are incorrect!");

            var wallGo = Instantiate(_wallPrefab, _fieldGoRoot, true);

            var cell1Transform = _cells[x1, y1].SpawnPoint.position;
            var cell2Transform = _cells[x2, y2].SpawnPoint.position;

            wallGo.Transform.position = (cell1Transform + cell2Transform) / 2;
            wallGo.Vertical = vertical;
            return wallGo;
        }

        private bool IsCoordsValid(int x, int y) =>
            _cells.GetLength(1) > y && _cells.GetLength(0) > x && x >= 0 && y >= 0;
        
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

            var position = Camera.main.transform.position;
            Camera.main.transform.position = new Vector3(position.x,
                position.y * Mathf.Max(fieldRootZSize, fieldRootXSize), position.z);
        }
    }
}