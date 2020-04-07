using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugCameraController : MonoBehaviour
{

    public bool _ignore = false;
    public Camera _debugCamera;

    /// <summary>
    /// Remove debug camera if it exists.
    /// </summary>
    public void Awake()
    {
        if (_debugCamera && !_ignore)
        {
            Destroy(_debugCamera.gameObject);
        }
    }

}
