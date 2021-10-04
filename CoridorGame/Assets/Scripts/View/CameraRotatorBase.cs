using System;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using LoopedList;

public abstract class CameraRotatorBase : MonoBehaviour
{
    [Serializable]
    protected struct CameraMoveData
    {
        public Vector3 rotation { get; private set; }
        public Vector3 position { get; private set; }
        public CameraMoveData(Vector3 rot, Vector3 pos) => (rotation, position) = (rot, pos);
    }

    [SerializeField, MinAttribute(0)] protected float _cameraTransitionTime = 5;
    protected Transform _mainCameraTransform;
    private IEnumerator<CameraMoveData> _cameraMoveDatas;

    protected abstract List<CameraMoveData> CameraMoveDatas { get; }
    private void Awake()
    {
        _mainCameraTransform = Camera.main.transform;
        _cameraMoveDatas = new LoopedList<CameraMoveData>(CameraMoveDatas).GetEnumerator();
    }

    public void RotateCamera()
    {
        SetData(_cameraMoveDatas.Current);
        _cameraMoveDatas.MoveNext();
    }

    protected void SetData(CameraMoveData cameraMoveData)
    {
        _mainCameraTransform.DOLocalMove(cameraMoveData.position, _cameraTransitionTime);
        _mainCameraTransform.DORotate(cameraMoveData.rotation, _cameraTransitionTime);
    }
}