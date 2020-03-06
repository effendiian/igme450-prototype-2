using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// Interface for if something is revealable.
/// </summary>
public interface UIShowable
{
    bool IsVisible { get; }
    void Show();
    void Hide();
}

/// <summary>
/// ModalWindows can be initialized, shown, hidden, and closed.
/// </summary>
public abstract class ModalWindow : MonoBehaviour, UIShowable
{

    /**************************
     * Fields / Properties
     *************************/
    
    /// <summary>
    /// On open.
    /// </summary>
    public UnityAction<ModalWindow> onOpen { protected get; set; }

    /// <summary>
    /// On close.
    /// </summary>
    public UnityAction<ModalWindow> onClose { protected get; set; }
        
    /// <summary>
    /// Is the modal window open?
    /// </summary>
    public bool IsOpen { get; protected set; }
    
    /// <summary>
    /// Is the window being displayed?
    /// </summary>
    public bool IsVisible { get; set; }

    /**************************
     * MonoBehaviour Methods
     *************************/

    /// <summary>
    /// On Awake, initialize the ModalWindow.
    /// </summary>
    public virtual void Awake()
    {
        this.IsOpen = false;
        this.IsVisible = false;
        this.onOpen = null;
        this.onClose = null;
    }

    /**************************
     * Service Methods
     *************************/

    /// <summary>
    /// Show the window.
    /// </summary>
    public abstract void Show();

    /// <summary>
    /// Hide the window.
    /// </summary>
    public abstract void Hide();

    /// <summary>
    /// Open the ModalWindow.
    /// </summary>
    public virtual void Open()
    {
        this.Show();
        this.IsOpen = true;
        if (this.onOpen != null) { this.onOpen.Invoke(this); }
    }

    /// <summary>
    /// Close the ModalWindow.
    /// </summary>
    public virtual void Close()
    {
        this.Hide();
        this.IsOpen = false;
        if (this.onClose != null) { this.onClose.Invoke(this); }
    }

    /// <summary>
    /// Process coroutine will continue to run until the window has been closed.
    /// </summary>
    /// <param name="window">Modal window that process will act upon.</param>
    /// <param name="openCallback">Callback invoked when window is opened.</param>
    /// <param name="closeCallback">Callback invoked when window is closed.</param>
    /// <returns>Returns coroutine.</returns>
    public static IEnumerator Open(ModalWindow window, UnityAction<ModalWindow> openCallback, UnityAction<ModalWindow> closeCallback)
    {
        // Add the delegate actions.
        window.onOpen += openCallback;
        window.onClose += closeCallback;

        // Open the window.
        window.Open();

        // While the ModalWindow is open, continue to run the coroutine.
        while (window.IsOpen)
        {
            yield return null;
        }

        // After this.IsOpen is false, we can remove callbacks.
        window.onOpen -= openCallback;
        window.onClose -= closeCallback;
    }
       
}
