using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;
using MHO.Extensions;

/// <summary>
/// A sparkle effect displays a potential burst on play.
/// </summary>
[RequireComponent(typeof(ParticleSystem))]
public class SparkleEffect : MonoBehaviour, IVisualEffect
{

    /////////////////////////////////////
    // Fields / Properties
    /////////////////////////////////////

    /// <summary>
    /// Debug mode flag. Show 
    /// </summary>
    [SerializeField]
    protected bool debug = false;

    /// <summary>
    /// Debug icon image for the sparkle effect.
    /// </summary>
    protected Sprite debugIcon = null;

    /// <summary>
    /// Reference to the debug image renderer.
    /// </summary>
    [SerializeField, ReadOnly]
    protected SpriteRenderer debugRenderer = null;

    /// <summary>
    /// Particle system.
    /// </summary>
    [SerializeField, Required]
    private ParticleSystem particles = null;

    /// <summary>
    /// Reference to the particle system.
    /// </summary>
    public ParticleSystem Particles => (this.particles) ? this.particles : this.particles = this.GetOrAddComponent<ParticleSystem>();

    /////////////////////////////////////
    // MonoBehaviour Methods
    /////////////////////////////////////

    /// <summary>
    /// Initialize the SparkleEffect references.
    /// </summary>
    void Start()
    {
#if UNITY_EDITOR
        // Set to debug mode, when running from the editor.
        this.debug = true;
#else
        // Set debug mode to false, if not running from the editor.
        this.debug = false;
#endif

        // Debug mode init.
        if (this.debug)
        {
            Debug.Log("Creating SparkleEffect.");
            if (this.debugIcon)
            {
                debugRenderer = this.GetOrAddComponent<SpriteRenderer>();
                debugRenderer.sprite = this.debugIcon;
            }
        }

        // Initialize the particle system.
        this.particles = this.particles ?? this.GetOrAddComponent<ParticleSystem>();

    }

    /////////////////////////////////////
    // IVisualEffect Methods
    /////////////////////////////////////

    /// <summary>
    /// Place visual effect at specified location. If parent is non-null, it will be made a child of the parent object.
    /// </summary>
    /// <param name="parent">Parent to assign.</param>
    /// <param name="localPosition">Local position to place object.</param>
    public void Place(GameObject parent, Vector3? localPosition = null) 
    {
        if (parent)
        {
            // Set parent as input object if reference is non-null.
            parent.transform.SetParent(this.transform);
        }

        // Place at local origin if local position is null.
        this.Place((localPosition.HasValue) ? localPosition.Value : Vector3.zero);
    }

    /// <summary>
    /// Place visual effect at specified location.
    /// </summary>
    /// <param name="position">Local position to place object.</param>
    public void Place(Vector3 position) {
        // Update the local position of this element.
        this.transform.localPosition = position;
    }

    public void Play() { 

    }

    public void Stop() {

    }

}
