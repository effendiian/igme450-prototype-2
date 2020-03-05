using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;

/// <summary>
/// Base controller for triggering VisualEffects.
/// </summary>
public abstract class VisualEffectController<T> : MonoBehaviour where T : IVisualEffect
{

    /////////////////////////////////////
    // Fields / Properties
    /////////////////////////////////////

    /// <summary>
    /// Prefab for instantiation of visual effect.
    /// </summary>
    [SerializeField, Required, Label("Visual Effect Prefab")]
    private GameObject prefab = null;

    /// <summary>
    /// 
    /// </summary>
    StateMachine engine = null;
}
