using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using NaughtyAttributes;
using TMPro;

/// <summary>
/// Struct containing data for the Instructions screen.
/// </summary>
[System.Serializable]
public struct InstructionData
{

    /// <summary>
    /// Title for Instruction ModalWindow.
    /// </summary>
    [TextArea(1, 1)]
    public string Title;

    /// <summary>
    /// Resizable text area for content of Instruction ModalWindow.
    /// </summary>
    [ResizableTextArea]
    public string Body;
}

/// <summary>
/// ModalWindow that displays instructions to the user on a particular screen.
/// </summary>
public class Instructions : ModalWindow
{

    /**************************
     * Fields / Properties
     *************************/

    /// <summary>
    /// Instructions window instance.
    /// </summary>
    public static Instructions Instance { get; private set; }

    /// <summary>
    /// Content for the ModalWindow.
    /// </summary>
    [Label("Instructions")]
    public InstructionData content;

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
    /// UI element responsible for the body text.
    /// </summary>
    [SerializeField, Required, BoxGroup("Canvas References")]
    protected TextMeshProUGUI body;

    /**************************
     * MonoBehaviour Methods
     *************************/

    /// <summary>
    /// Initialize the CanvasRevealer.
    /// </summary>
    public override void Awake()
    {
        base.Awake();
        this.canvasRevealer = this.canvasRevealer ?? new CanvasRevealer();

        if (!Instructions.Instance)
        {
            Instructions.Instance = this;
            this.UpdateText();
            this.Hide();
        }
        else
        {
            Destroy(this);
        }

    }

    /**************************
     * Service Methods
     *************************/

    /// <summary>
    /// Update the instruction content.
    /// </summary>
    public void UpdateText()
    {
        if (this.IsVisible && this.canvasRevealer.HasCanvas)
        {
            // Update the instructions.
            this.title.text = this.content.Title;
            this.body.text = this.content.Body;
        }
    }

    /// <summary>
    /// Show the ModalWindow UI.
    /// </summary>
    public override void Show()
    {
        this.UpdateText();
        this.canvasRevealer.Show();
        this.IsVisible = true;
    }

    /// <summary>
    /// Hide the ModalWindow UI.
    /// </summary>
    public override void Hide()
    {
        this.canvasRevealer.Hide();
        this.IsVisible = false;
    }
    
}
