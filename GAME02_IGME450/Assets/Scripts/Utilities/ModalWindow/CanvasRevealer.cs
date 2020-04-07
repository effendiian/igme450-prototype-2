using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;

/// <summary>
/// Interface containing show/hide canvas functions.
/// </summary>
[System.Serializable]
public class CanvasRevealer {

    /**************************
     * Fields / Properties
     *************************/
    
    /// <summary>
    /// Canvas object being manipulated.
    /// </summary>
    [SerializeField, Required]
    private GameObject canvasObject;

    /// <summary>
    /// Return true if non-null reference to canvas object exists.
    /// </summary>
    public bool HasCanvas => this.canvasObject != null;

    /**************************
     * Constructors
     *************************/
    
    /// <summary>
    /// Create an empty revealer.
    /// </summary>
    public CanvasRevealer() { this.canvasObject = null; }

    /// <summary>
    /// Construct a canvas revealer object.
    /// </summary>
    /// <param name="canvasRef">Reference canvas.</param>
    public CanvasRevealer(GameObject canvasRef)
    {
        if (canvasRef.GetComponent<Canvas>())
        {
            this.canvasObject = canvasRef;
        }
    }

    /**************************
     * Service Methods
     *************************/

    /// <summary>
    /// Set the canvas active flag.
    /// </summary>
    /// <param name="flag">Active status value.</param>
    private void SetCanvasActive(bool flag)
    {
        if (this.canvasObject)
        {
            this.canvasObject.SetActive(flag);
        }
    }

    /// <summary>
    /// Show canvas object.
    /// </summary>
    public void Show() => this.SetCanvasActive(true);

    /// <summary>
    /// Hide canvas object.
    /// </summary>
    public void Hide() => this.SetCanvasActive(false);




}

