using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace View
{
    class CameraRotatorMenu : MonoBehaviour
    {
        [SerializeField] private float speed = 2F;
        public void Update()
        {
            transform.Rotate(0, speed * Time.deltaTime, 0);
        }
    }
}