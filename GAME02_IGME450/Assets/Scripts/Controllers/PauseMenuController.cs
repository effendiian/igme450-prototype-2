using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using NaughtyAttributes;

public class PauseMenuController : ModalController
{

    public static PauseMenuController Instance { get; private set; }

    /// <summary>
    /// Pause flag.
    /// </summary>
    public bool IsPaused => this.isOpen;

    /// <summary>
    /// Close the modal menu.
    /// </summary>
    public void Resume() => this.CloseModal();

    public void Awake()
    {
        if (!PauseMenuController.Instance)
        {
            PauseMenuController.Instance = this;
        } else
        {
            Destroy(this);
        }
    }

    /// <summary>
    /// Go to the main menu.
    /// </summary>
    public void MainMenu()
    {
        // TODO.
        this.Close();
    }

    /// <summary>
    /// On pressing the quit button.
    /// </summary>
    public void Quit()
    {
        if (this.IsPaused)
        {
            GameManager.Instance.Quit();
        }
    }

}
