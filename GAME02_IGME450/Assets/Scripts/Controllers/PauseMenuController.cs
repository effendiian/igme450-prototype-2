using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using NaughtyAttributes;

public class PauseMenuController : ModalController
{
    /// <summary>
    /// Pause flag.
    /// </summary>
    public bool IsPaused => this.isOpen;

    /// <summary>
    /// Close the modal menu.
    /// </summary>
    public void Resume() => this.CloseModal();

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
