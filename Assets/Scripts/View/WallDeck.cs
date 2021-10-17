using System.Collections.Generic;
using UnityEngine;

namespace Quoridorgame.View
{
    /// <summary>
    /// "Стопка" стенок у конкретного игрока
    /// Служит для стартвого их расположения и хранения
    /// </summary>
    public class WallDeck : MonoBehaviour
    {
        [SerializeField] private Wall _wallPrefab;
        [SerializeField] private Transform _wallsRoot;

        /// <summary>
        /// Неиспользованные стенки текущей колоды
        /// </summary>
        public Stack<Wall> FreeWalls { get; private set; } = new Stack<Wall>();

        /// <summary>
        /// Все стенки текущей колоды
        /// </summary>
        public Stack<Wall> AllWalls { get; private set; } = new Stack<Wall>();

        /// <summary>
        /// Возвращает очередную незанятую стенку
        /// </summary>
        /// <returns></returns>
        public Wall GetWall() => FreeWalls.Pop();

        /// <summary>
        /// Откат колоды до начального состояния
        /// </summary>
        public void ResetDeck()
        {
            foreach (var wall in AllWalls)
                Destroy(wall.gameObject);
            AllWalls.Clear();
            FreeWalls.Clear();
        }

        /// <summary>
        /// Добавить стенки в колоду
        /// </summary>
        /// <param name="wallsCount">кол-во добавляемых стенок</param>
        public void AddWalls(int wallsCount)
        {
            for (int i = 0; i < wallsCount; i++)
            {
                var wallGo = Instantiate(_wallPrefab, _wallsRoot, true);
                wallGo.Transform.localPosition =
                    Vector3.right * (i * wallGo.Width * 3 - (wallGo.Width* 3 / 2f * (wallsCount-1)));
                wallGo.Transform.localRotation = Quaternion.Euler(0, 90, 0);
                FreeWalls.Push(wallGo);
                AllWalls.Push(wallGo);
            }
        }
    }
}