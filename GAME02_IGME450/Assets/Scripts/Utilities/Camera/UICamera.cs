using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;

/// <summary>
/// Tags and moves UICamera reference properly.
/// </summary>
[RequireComponent(typeof(Camera))]
public class UICamera : MonoBehaviour
{
    /// <summary>
    /// Reference to the camera.
    /// </summary>
    [SerializeField, ReadOnly]
    private Camera cameraRef;

    /// <summary>
    /// Prepare camera on startup.
    /// </summary>
    private void Awake()
    {
        // Grab the camera from this component if null.
        cameraRef = cameraRef ?? gameObject.GetComponent<Camera>();

        // Tag the camera.
        cameraRef.tag = "UICamera";

        // Initialize the manager class.
        CameraManager.Instance.DoNothing();
    }
}
