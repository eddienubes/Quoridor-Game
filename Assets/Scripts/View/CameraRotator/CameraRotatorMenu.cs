using UnityEngine;

namespace Quoridorgame.View
{
    class CameraRotatorMenu : MonoBehaviour
    {
        [SerializeField]
        private float speed = 2F;

        public void Update()
        {
            transform.Rotate(0, speed * Time.deltaTime, 0);
        }
    }
}