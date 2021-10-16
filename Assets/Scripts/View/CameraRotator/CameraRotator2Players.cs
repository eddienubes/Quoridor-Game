using System;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using LoopedList;


namespace Quoridorgame.View
{
    public class CameraRotator2Players : CameraRotatorBase
    {
        protected override List<CameraMoveData> CameraMoveDatas
        {
            get
            {
                var player1Position = _mainCameraTransform.localPosition;
                var player1Rotation = _mainCameraTransform.eulerAngles;

                var player2Position = player1Position;
                player2Position.z = player2Position.z * -1;
                var player2Rotation = player1Rotation;
                player2Rotation.y += 180;

                return new List<CameraMoveData>
                {
                    new CameraMoveData(player1Position, player1Rotation),
                    new CameraMoveData(player2Position, player2Rotation)
                };
            }
        }
    }
}