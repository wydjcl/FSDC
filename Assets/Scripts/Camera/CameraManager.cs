using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : SingletonMono<CameraManager>
{
    public Camera mainCamera;
    public Camera cardCamera;
    public CameraFollowTarget cameraFollowTarget;
}
