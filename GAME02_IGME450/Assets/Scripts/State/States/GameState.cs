using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// GameState class.
/// </summary>
public class GameState : State
{

    /// <summary>
    /// Construct GameState with reference to the machine.
    /// </summary>
    /// <param name="machine">StateMachine.</param>
    public GameState(StateMachine machine) : base(machine) { }

    /// <summary>
    /// Run when the GameState is started.
    /// </summary>
    public override void Enter()
    {
        base.Enter();
        Debug.Log($"[{typeof(GameState)}] - Launched the application. Pop this state to exit the application.");
    }

    /// <summary>
    /// Run when the GameState is exit.
    /// </summary>
    public override void Exit()
    {
        base.Exit();
        Debug.Log($"[{typeof(GameState)}] - Ending the application.");
        GameManager.Instance.Quit();
    }

}
