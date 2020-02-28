using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MHO.Extensions;

/// <summary>
/// GameManager Singleton class.
/// </summary>
public class GameManager : Manager<GameManager>
{

    /// <summary>
    /// Allow inheritance but reject constructor calls for this class.
    /// </summary>
    protected GameManager() { }

    /// <summary>
    /// Reference to the Globals component.
    /// </summary>
    public Globals Globals { get; protected set; }

    /// <summary>
    /// Setup the Manager class.
    /// </summary>
    protected override void Setup()
    {
        // Get or add a new component of type Globals.
        this.Globals = this.gameObject.GetOrAddComponent<Globals>();
        this.Globals.Setup();
    }



}
