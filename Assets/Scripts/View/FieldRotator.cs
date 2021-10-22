using DG.Tweening;
using UnityEngine;

namespace Quoridorgame.View
{
    public class FieldRotator : MonoBehaviour
    {
        [SerializeField] private Transform _gameFieldRoot;
        [SerializeField] private float _onWheelRotationSpeed = 1;
        [SerializeField] private float _wheelRotationTime = 1;
        private void Update()
        {
            _gameFieldRoot.Rotate(0,Input.GetAxis("Mouse ScrollWheel") * _onWheelRotationSpeed,0);
        }

        public void SetRotation(float degree)
        {
            _gameFieldRoot.DORotate(Vector3.up * degree, _wheelRotationTime);
        }
    }
}