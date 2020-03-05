using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugCameraController : MonoBehaviour
{

    public Camera _debugCamera;

    /// <summary>
    /// Remove debug camera if it exists.
    /// </summary>
    public void Awake()
    {
        if (_debugCamera)
        {
            Destroy(_debugCamera.gameObject);
        }
    }

}
