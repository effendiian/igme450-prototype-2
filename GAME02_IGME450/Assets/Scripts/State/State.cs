using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Abstract class representing a State. Methods do nothing by default.
/// Based off: https://www.raywenderlich.com/6034380-state-pattern-using-unity
/// </summary>
public abstract class State
{

    /// <summary>
    /// Reference to the StateMachine engine. (For changing states).
    /// </summary>
    protected StateMachine engine;

    /// <summary>
    /// Creation of a State object accepts a StateMachine reference.
    /// </summary>
    /// <param name="sm"></param>
    public State(StateMachine stateMachine) => this.engine = stateMachine;

    /// <summary>
    /// Initialize the State.
    /// </summary>
    public virtual void Enter() { } // => Debug.Log($"Entering state '{this}'");

    /// <summary>
    /// Handle and process input during this particular State.
    /// </summary>
    public virtual void HandleInput() { }

    /// <summary>
    /// Per frame update.
    /// </summary>
    public virtual void Update() { }

    /// <summary>
    /// Physics updates that occur at a fixed pace.
    /// </summary>
    public virtual void FixedUpdate() { }

    /// <summary>
    /// On exit from the State.
    /// </summary>
    public virtual void Exit() { } // => Debug.Log($"Exiting state '{this}'");

}