using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// A visual effect can be triggered.
/// </summary>
public interface IVisualEffect
{

    /// <summary>
    /// Set position of visual effect.
    /// </summary>
    /// <param name="parent">Object to make parent of this component.</param>
    /// <param name="localPosition">Location where visual effect should be placed.</param>
    void Place(GameObject parent, Vector3? localPosition = null);

    /// <summary>
    /// Place visual effect at specified location.
    /// </summary>
    /// <param name="position">Local position to place object.</param>
    public void Place(Vector3 position);

    /// <summary>
    /// Play the visual effect.
    /// </summary>
    void Play();

    /// <summary>
    /// Stop the visual effect.
    /// </summary>
    void Stop();

}
