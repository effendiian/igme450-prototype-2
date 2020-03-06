using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using NaughtyAttributes;

public class HUDController : MonoBehaviour
{
    [Required]
    public Button pauseButton;

    [Required]
    public Button restartButton;

    /// <summary>
    /// Restart the application.
    /// </summary>
    [Button("Restart Application")]
    public void RestartApplication()
    {
        GameManager.Instance.LoadScene("Game.Core");
    }

    [Button("Pause")]
    public void PauseApplication()
    {
        if (PauseMenuController.Instance)
        {
            pauseButton.interactable = false;
            pauseButton.enabled = false;
            this.StartCoroutine(PauseMenuController.Instance.Popup(ResumeApplication));
        }
    }

    [Button("Resume")]
    public void ResumeApplication()
    {
        if (PauseMenuController.Instance)
        {
            pauseButton.interactable = true;
            pauseButton.enabled = true;
            Debug.Log("Resume application.");
        }
    }


}
