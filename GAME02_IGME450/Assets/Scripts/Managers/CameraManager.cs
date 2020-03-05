using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using MHO.Extensions;

/// <summary>
/// Manage reference to Main and UI Cameras.
/// </summary>
public class CameraManager : Manager<CameraManager>
{

    /// <summary>
    /// MainCamera reference.
    /// </summary>
    private static Camera MainCamera => Instance.GetMainCamera();

    /// <summary>
    /// UICamera reference.
    /// </summary>
    private static Camera UICamera => Instance.GetUICamera();

    /// <summary>
    /// Main Camera reference.
    /// </summary>
    [SerializeField, Tooltip("Main Camera reference.")]
    private GameObject mainCamera;

    /// <summary>
    /// UI Camera reference.
    /// </summary>
    [SerializeField, Tooltip("UI Camera reference.")]
    private GameObject uiCamera;

    /// <summary>
    /// Allow inheritance but reject constructor calls for this class.
    /// </summary>
    protected CameraManager() { }

    /// <summary>
    /// Setup the Manager class.
    /// </summary>
    protected override void Setup()
    {
        this.SetupMainCamera();
        this.SetupUICamera();
    }

    /// <summary>
    /// Main Camera reference setup.
    /// </summary>
    private void SetupMainCamera()
    {
        mainCamera = this.GetCameraObject(mainCamera, "MainCamera");
        GetMainCamera();
        Debug.Log($"Initializing camera [{this.mainCamera}].");

        GameObject[] cameras = GameObject.FindGameObjectsWithTag("MainCamera");
        if (cameras.Length > 0)
        {
            for (int i = cameras.Length; i >= 0; i--)
            {
                Camera c = cameras[i].GetComponent<Camera>();
                c.enabled = false;
            }
        }
    }

    /// <summary>
    /// UI Camera reference setup.
    /// </summary>
    private void SetupUICamera()
    {
        uiCamera = this.GetCameraObject(uiCamera, "UICamera");
        GetUICamera();
        Debug.Log($"Initializing [{this.uiCamera}].");

        GameObject[] cameras = GameObject.FindGameObjectsWithTag("UICamera");
        if (cameras.Length > 0)
        {
            for (int i = cameras.Length; i >= 0; i--)
            {
                Camera c = cameras[i].GetComponent<Camera>();
                c.enabled = false;
            }
        }

    }

    /// <summary>
    /// Search for camera with specified tag.
    /// </summary>
    /// <param name="camera">Camera to check against</param>
    /// <param name="tag">Tag to look for.</param>
    private GameObject GetCameraObject(GameObject camera, string tag)
    {
        if (camera && camera.CompareTag(tag)) 
        { 
            // Return if it already exists.
            return camera;
        }
        else
        {
            GameObject cameraObject = camera;
            if (!cameraObject || !cameraObject.CompareTag(tag))
            {
                cameraObject = GameObject.FindGameObjectWithTag(tag);
                if (!cameraObject)
                {
                    cameraObject = new GameObject($"Camera {tag}") { tag = tag };
                    cameraObject.transform.SetParent(Manager.transform);
                }
            }

            // Return camera object.
            return cameraObject;
        }
    }

    /// <summary>
    /// Get reference to the camera component.
    /// </summary>
    /// <returns>Returns the main camera component.</returns>
    public Camera GetMainCamera()
    {
        this.mainCamera = this.GetCameraObject(this.mainCamera, "MainCamera");     
        if (!mainCamera)
        {
            Debug.LogError("Main camera not loaded. Returning null.");
            return null;
        }
        return mainCamera.GetOrAddComponent<Camera>();
    }

    /// <summary>
    /// Get reference to the camera component.
    /// </summary>
    /// <returns>Returns the ui camera component.</returns>
    public Camera GetUICamera()
    {
        this.uiCamera = this.GetCameraObject(this.uiCamera, "UICamera");
        if (!uiCamera)
        {
            Debug.LogError("UI camera not loaded. Returning null.");
            return null;
        }
        return uiCamera.GetOrAddComponent<Camera>();
    }


}
