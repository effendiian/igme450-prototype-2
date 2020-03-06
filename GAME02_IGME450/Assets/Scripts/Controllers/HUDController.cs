using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using NaughtyAttributes;
using MHO.Extensions;

/// <summary>
/// Start HUD state.
/// </summary>
public class HUDShowButtonState : State
{

    /**************************
     * Fields / Properties
     *************************/
    
    /// <summary>
    /// Seed button.
    /// </summary>
    private Button[] showableButtons;

    /**************************
     * Constructors
     *************************/

    /// <summary>
    /// Provide state with engine and hideable button.
    /// </summary>
    /// <param name="engine">State machine.</param>
    /// <param name="buttons">Showable buttons.</param>
    public HUDShowButtonState(StateMachine engine, params Button[] buttons) : base(engine)
    {
        this.showableButtons = buttons;
    }

    /**************************
     * Service Methods
     *************************/

    /// <summary>
    /// On enter, show the showable button.
    /// </summary>
    public override void Enter()
    {
        base.Enter();
        foreach(Button button in this.showableButtons)
        {
            button.gameObject.SetActive(true);
        }
    }

    /// <summary>
    /// On exit, hide the showable button.
    /// </summary>
    public override void Exit()
    {
        foreach (Button button in this.showableButtons)
        {
            button.gameObject.SetActive(false);
        }
        base.Exit();
    }

}

/// <summary>
/// Paused HUD state.
/// </summary>
public class HUDPausedState : State
{

    /// <summary>
    /// Canvas revealer.
    /// </summary>
    private CanvasRevealer canvasRevealer;

    /// <summary>
    /// Special case HUDShowButtonState.
    /// </summary>
    /// <param name="engine">State machine.</param>
    /// <param name="buttons">Pause button.</param>
    public HUDPausedState(StateMachine engine, GameObject canvas) : base(engine)
    {
        this.canvasRevealer = new CanvasRevealer(canvas);
    }

    /// <summary>
    /// Enter the state and pause the hud.
    /// </summary>
    public override void Enter()
    {
        base.Enter();
        this.canvasRevealer.Hide();
        HUDController.Instance.IsPaused = true;
    }

    /// <summary>
    /// Exit the state and resume the hud.
    /// </summary>
    public override void Exit()
    {
        this.canvasRevealer.Show();
        HUDController.Instance.IsPaused = false;
        base.Exit();
    }

}

/// <summary>
/// End HUD state.
/// </summary>
public class HUDEndState : HUDShowButtonState
{

    /// <summary>
    /// Special case HUDShowButtonState.
    /// </summary>
    /// <param name="engine">State machine.</param>
    /// <param name="restartButton">Restart button.</param>
    public HUDEndState(StateMachine engine, Button restartButton) : base(engine, restartButton) { }

    /// <summary>
    /// Enter the state and pause the hud.
    /// </summary>
    public override void Enter()
    {
        base.Enter();
        HUDController.Instance.IsRestartPending = true;
    }

    /// <summary>
    /// Exit the state and resume the hud.
    /// </summary>
    public override void Exit()
    {
        HUDController.Instance.IsRestartPending = false;
        base.Exit();
    }

}


/// <summary>
/// Controller for the HUD.
/// </summary>
public class HUDController : MonoBehaviour
{

    /**************************
     * Fields / Properties
     *************************/

    /// <summary>
    /// Instance reference to this controller.
    /// </summary>
    public static HUDController Instance { get; private set; }
    
    /// <summary>
    /// StateMachine for tracking HUDController state.
    /// </summary>
    private StateMachine engine;
    private HUDShowButtonState idleState; // Seed + Pause buttons.
    private HUDShowButtonState gameState; // Pause button.
    private HUDPausedState pauseState; // No buttons; just canvas.
    private HUDEndState endState; // Restart button.
    
    /// <summary>
    /// Canvas references.
    /// </summary>
    [SerializeField, Required, Label("Canvas"), BoxGroup("Canvas References")]
    private GameObject canvasObject;

    /// <summary>
    /// Button from the UI.
    /// </summary>
    [SerializeField, Required, BoxGroup("Canvas References")]
    private Button seedButton;

    /// <summary>
    /// Button from the UI.
    /// </summary>
    [SerializeField, Required, BoxGroup("Canvas References")]
    private Button pauseButton;

    /// <summary>
    /// Button from the UI.
    /// </summary>
    [SerializeField, Required, BoxGroup("Canvas References")]
    private Button restartButton;

    /// <summary>
    /// Paused flag.
    /// </summary>
    public bool IsPaused { get; set; }

    /// <summary>
    /// Is restart pending?
    /// </summary>
    public bool IsRestartPending { get; set; }
      
    /**************************
     * MonoBehaviour Methods
     *************************/

    /// <summary>
    /// Initialize the components.
    /// </summary>
    private void Awake()
    {
        if (!HUDController.Instance)
        {
            HUDController.Instance = this;
            this.engine = gameObject.GetOrAddComponent<StateMachine>();
            this.IsPaused = false;
            this.IsRestartPending = false;

            // Prepare states.
            this.idleState = new HUDShowButtonState(this.engine, this.seedButton, this.pauseButton);
            this.gameState = new HUDShowButtonState(this.engine, this.pauseButton);
            this.pauseState = new HUDPausedState(this.engine, this.canvasObject);
            this.endState = new HUDEndState(this.engine, this.restartButton);
        }
        else
        {
            Destroy(this);
        }
    }

    /// <summary>
    /// Start the statemachine on start.
    /// </summary>
    private void Start()
    {
        this.engine.Initialize(this.idleState);
    }

    /**************************
     * Service Methods
     *************************/

    /// <summary>
    /// Restart the application.
    /// </summary>
    [Button("Restart Application")]
    public void RestartApplication() => this.StartCoroutine(this.RestartApplicationProcess());

    /// <summary>
    /// Reload this scene.
    /// </summary>
    /// <returns>Process for loading scene.</returns>
    private IEnumerator RestartApplicationProcess()
    {
        Debug.Log("Restarting the game.");
        yield return GameManager.Instance.LoadScene("Game.Core");
    }

    /// <summary>
    /// Pause the game.
    /// </summary>
    [Button("Pause")]
    public void PauseApplication()
    {
        if (PauseMenuController.Instance && !this.IsPaused)
        {
            this.engine.PushState(this.pauseState);
            this.StartCoroutine(PauseMenuController.Instance.Popup(ResumeApplication));
        }
        else
        {
            Debug.Log("Game already paused.");
        }
    }

    /// <summary>
    /// Resume the game.
    /// </summary>
    [Button("Resume")]
    public void ResumeApplication()
    {
        if (PauseMenuController.Instance && this.IsPaused)
        {
            this.engine.PopState();
            PauseMenuController.Instance.Resume();
            Debug.Log("Resume application.");
        }
        else
        {
            Debug.Log("Game already running.");
        }
    }


}
