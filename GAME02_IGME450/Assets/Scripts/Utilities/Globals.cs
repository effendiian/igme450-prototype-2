using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MHO.Extensions;

/// <summary>
/// Structure for tracking global values.
/// </summary>
public class Globals : MonoBehaviour
{

    /// <summary>
    /// State machine.
    /// </summary>
    public StateMachine Engine { get; set; }

    /// <summary>
    /// Setup.
    /// </summary>
    public void Setup()
    {
        // Get reference to a StateMachine component.
        this.Engine = this.gameObject.GetOrAddComponent<StateMachine>();
        this.Engine.Initialize(new GameState(this.Engine));
        this.StartCoroutine(QuitAfterDelay(1));
    }


    /// <summary>
    /// Quit the application after a delay.
    /// </summary>
    /// <param name="seconds">Time until quit.</param>
    /// <returns>Coroutine.</returns>
    public IEnumerator QuitAfterDelay(float seconds)
    {
        float time = (seconds > 0.0f) ? seconds : 0.0f;
        Debug.Log($"Quitting after {seconds} second(s).");
        yield return new WaitForSeconds(time);
        this.Quit();
    }

    /// <summary>
    /// Update the current state.
    /// </summary>
    public void Update()
    {
        this.Engine.CurrentState.Update();
    }

    /// <summary>
    /// Update the current state.
    /// </summary>
    public void FixedUpdate()
    {
        this.Engine.CurrentState.FixedUpdate();
    }

    /// <summary>
    /// When quit is triggered, end the game state machine and exit.
    /// </summary>
    public void Quit()
    {
        // Unwind the stack.
        this.Engine.End();
    }

}
