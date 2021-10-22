using System;
using System.Collections;
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
        public CameraMoveData(Vector3 pos, Vector3 rot) => (rotation, position) = (rot, pos);
    }

    [SerializeField, MinAttribute(0)] protected float _cameraTransitionTime = 5;
    protected Transform _mainCameraTransform;
    private IEnumerator<CameraMoveData> _cameraMoveDatas;
    private LoopedList<CameraMoveData> _cameraMoveDataList;
    protected abstract List<CameraMoveData> CameraMoveDatas { get; }
    public virtual void Init()
    {
        if (_mainCameraTransform != default)
        {
            Reset();
        }
        else
        {
            _mainCameraTransform = Camera.main.transform;
            _cameraMoveDataList = new LoopedList<CameraMoveData>(CameraMoveDatas);
            _cameraMoveDatas =_cameraMoveDataList .GetEnumerator();
            RotateCamera();
        }
    }

    public void Reset()
    {
        SetData(_cameraMoveDataList[0]);
    }

    public void RotateCamera() 
    {
        _cameraMoveDatas.MoveNext();
        SetData(_cameraMoveDatas.Current);
    }

    protected void SetData(CameraMoveData cameraMoveData)
    {
        _mainCameraTransform.DOLocalMove(cameraMoveData.position, _cameraTransitionTime);
        _mainCameraTransform.DORotate(cameraMoveData.rotation, _cameraTransitionTime);
    }
}