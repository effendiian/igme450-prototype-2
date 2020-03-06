using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;
using MHO.Extensions;

/// <summary>
/// Initialize the Game.Core scenes on Awake.
/// </summary>
public class GameController : MonoBehaviour
{

    /// <summary>
    /// State tracker.
    /// </summary>
    [SerializeField, ReadOnly]
    protected StateMachine engine;

    /// <summary>
    /// Load the listed scenes additively.
    /// </summary>
    [SerializeField]
    protected SceneLoadProfile profile = null;

    /// <summary>
    /// On Awake, initialize scene's dependencies.
    /// </summary>
    private void Awake()
    {
        // Initialize the GameManager instance.
        GameManager.Instance.DoNothing();

        // Ensure state machine component exists.
        this.engine = gameObject.GetOrAddComponent<StateMachine>();
    }
    
    /// <summary>
    /// Start the engine.
    /// </summary>
    private void Start() => this.StartCoroutine(this.InitializeGame());

    public virtual void Update()
    {
        if (!this.engine.IsDone)
        {
            // Handle input.
            this.engine.CurrentState.HandleInput();

            // Update the current state.
            this.engine.CurrentState.Update();
        } else
        {
            Debug.Log($"Done! {this.engine.CurrentState}");
        }
    }

    /// <summary>
    /// Initialize game coroutine.
    /// </summary>
    /// <returns>Returns coroutine.</returns>
    private IEnumerator InitializeGame()
    {
        // Load scene additives.
        yield return InitializeProfile(this.profile);

        // Initialize pause menu.

        // Initialize the state machine once profile is loaded.
        this.engine.Initialize(new GameState(this.engine));

    }

    /// <summary>
    /// Initialize profile.
    /// </summary>
    /// <returns>Returns coroutine.</returns>
    private IEnumerator InitializeProfile(SceneLoadProfile loadProfile)
    {
        if (loadProfile)
        {
            yield return loadProfile.LoadSceneAdditively();
        }
    }

}
