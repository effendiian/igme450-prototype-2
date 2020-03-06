using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;
using MHO.Extensions;

public class ParticleCamera : MonoBehaviour
{

    [SerializeField, Required]
    private Camera particleCamera;

    [SerializeField, Required]
    private RenderTexture texture;

    public void Awake()
    {
        this.particleCamera = this.GetOrAddComponent<Camera>();
        particleCamera.targetTexture = texture;
    }

    /// <summary>
    /// Update the UI.
    /// </summary>
    public void Update()
    {
        particleCamera.Render();
        // particleCamera.targetTexture = null;
    }


}
