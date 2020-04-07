using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using NaughtyAttributes;
using TMPro;

/// <summary>
/// MainMenu.
/// </summary>
public class MainMenuController : MonoBehaviour, UIShowable
{

    /**************************
     * Fields / Properties
     *************************/

    [SerializeField]
    private UnityAction onOpenInstructions;

    /// <summary>
    /// Object containing the Canvas.
    /// </summary>
    [SerializeField, Label("Canvas"), BoxGroup("Canvas References")]
    protected CanvasRevealer canvasRevealer;
    // protected GameObject canvasObject;

    /// <summary>
    /// UI element responsible for the title text.
    /// </summary>
    [SerializeField, Required, BoxGroup("Canvas References")]
    protected TextMeshProUGUI title;

    /// <summary>
    /// Flag determining if UI is visible.
    /// </summary>
    public bool IsVisible { get; protected set; }

    /**************************
     * Service Methods
     *************************/
    
    /// <summary>
    /// Initialize controller.
    /// </summary>
    public void Awake()
    {
        // Prepare reference to the GameManager instance.
        GameManager.Instance.DoNothing();

        // Prepare the canvas revealer.
        this.canvasRevealer = this.canvasRevealer ?? new CanvasRevealer();
    }

    /// <summary>
    /// Show buttons on canvas.
    /// </summary>
    [Button("Show Menu UI")]
    public void Show()
    {
        this.canvasRevealer.Show();
        this.IsVisible = true;
    }

    /// <summary>
    /// Hide buttons on canvas.
    /// </summary>
    [Button("Hide Menu UI")]
    public void Hide()
    {
        this.canvasRevealer.Hide();
        this.IsVisible = false;
    }

    /// <summary>
    /// Start game on button press.
    /// </summary>
    [Button("Start Game")]
    public void StartGame() => this.StartCoroutine(this.StartGameProcess());

    /// <summary>
    /// Coroutine for starting the game.
    /// </summary>
    /// <returns>Coroutine.</returns>
    public IEnumerator StartGameProcess()
    {
        this.Hide();
        yield return GameManager.Instance.LoadScene("Game.Core");
    }

    /// <summary>
    /// Quit the game when this is executed.
    /// </summary>
    [Button("Quit Game")]
    public void QuitGame()
    {
        GameManager.Instance.Quit();
    }

    /// <summary>
    /// Show the instructions modal window on click.
    /// </summary>
    [Button("Show Instructions")]
    public void ShowInstructions() => this.StartCoroutine(this.ShowInstructionsWindow());

    /// <summary>
    /// Coroutine for showing the window.
    /// </summary>
    /// <returns>Coroutine.</returns>
    public IEnumerator ShowInstructionsWindow()
    {
        if (!Instructions.Instance)
        {
            yield return GameManager.Instance.LoadSceneAdditive("MainMenu.UI.Instructions");
        }

        if (Instructions.Instance)
        {
            yield return ModalWindow.Open(Instructions.Instance, this.OpenInstructionsWindow, this.CloseInstructionsWindow);
        }
    }

    /// <summary>
    /// Instructions.OnOpen callback.
    /// </summary>
    /// <param name="instructions">Instructions ModalWindow.</param>
    private void OpenInstructionsWindow(ModalWindow instructions)
    {
        // Animations.
        this.Hide();
    }

    /// <summary>
    /// Instructions.OnClose callback.
    /// </summary>
    /// <param name="instructions">Instructions ModalWindow.</param>
    private void CloseInstructionsWindow(ModalWindow instructions)
    {
        // Animations.
        this.Show();
    }

}
