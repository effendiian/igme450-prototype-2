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
    private StateMachine engine;

    /// <summary>
    /// Load listed scenes additively.
    /// </summary>
    [SerializeField]
    private SceneLoadProfile profile;

    /// <summary>
    /// On Awake, initialize the scene's dependencies.
    /// </summary>
    private void Awake()
    {
        // Initialize the GameManager instance.
        GameManager.Instance.DoNothing();

        // Initialize the CameraManager instance.
        CameraManager.Instance.DoNothing();

        // Ensure state machine component exists.
        this.engine = gameObject.GetOrAddComponent<StateMachine>();
    }

    /// <summary>
    /// Start the engine.
    /// </summary>
    private void Start()
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
    public void Update()
    {
        if (!this.engine.IsDone)
        {
            // Handle input.
            this.engine.CurrentState.HandleInput();

            // Update the current state.
            this.engine.CurrentState.Update();

            // On press of the space key, exit the game.
            if (Input.GetKey(KeyCode.Space))
            {
                this.engine.PopState();
                Debug.Log("Popping state!");
            }
        } else
        {
            Debug.Log($"Done! {this.engine.CurrentState}");
        }
    }

}
