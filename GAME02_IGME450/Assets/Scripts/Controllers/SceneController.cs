using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;
using MHO.Extensions;

/// <summary>
/// Initializes a Scene on Awake.
/// </summary>
public class SceneController : MonoBehaviour
{

    /// <summary>
    /// StateMachine reference.
    /// </summary>
    [SerializeField, ReadOnly]
    protected StateMachine engine;
    
    /// <summary>
    /// Load listed scenes additively.
    /// </summary>
    [SerializeField]
    private SceneLoadProfile profile = null;

    /// <summary>
    /// On Awake, initialize the scene's dependencies.
    /// </summary>
    private void Awake()
    {
        // Initialize the GameManager instance.
        if (GameManager.Instance) { GameManager.Instance.DoNothing(); }

        // Ensure state machine component exists.
        this.engine = gameObject.GetOrAddComponent<StateMachine>();
    }

    /// <summary>
    /// Start the engine.
    /// </summary>
    protected virtual void Start()
    {
        // Load the scenes in the profile additively.
        if (profile) {
            Debug.Log("Profile found. Loading profiles.");
            this.StartCoroutine(profile.LoadSceneAdditively());
        }

        // Initialize the state machine.
        this.engine.Initialize(new GameState(this.engine));
    }

    /// <summary>
    /// Update the scene controller.
    /// </summary>
    public virtual void Update()
    {
        if (!this.engine.IsDone)
        {
            // Handle input.
            this.engine.CurrentState.HandleInput();

            // Update the current state.
            this.engine.CurrentState.Update();
        }
        else
        {
            Debug.Log($"Done! {this.engine.CurrentState}");
        }
    }

}
