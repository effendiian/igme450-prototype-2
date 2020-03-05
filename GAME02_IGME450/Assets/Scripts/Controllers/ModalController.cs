using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using NaughtyAttributes;

public class ModalController : MonoBehaviour
{
    /// <summary>
    /// Determine if the menu is open.
    /// </summary>
    [SerializeField, ReadOnly]
    protected bool isOpen = false;

    /// <summary>
    /// On closing the modal window.
    /// </summary>
    [SerializeField, ReadOnly]
    protected UnityAction OnClose = null;

    /// <summary>
    /// Modal window that is being shown/hid.
    /// </summary>
    [SerializeField, Required]
    protected GameObject window = null;

    /// <summary>
    /// Set the open flag.
    /// </summary>
    protected void Open() => this.isOpen = true;

    /// <summary>
    /// Set the open flag.
    /// </summary>
    protected void Close() => this.isOpen = false;

    /// <summary>
    /// On pause, set open flag to true and show the canvas.
    /// </summary>
    public void ShowModal(UnityAction onResume)
    {
        this.Open();
        this.OnClose += onResume;
        if (this.window)
        {
            this.window.SetActive(true);
        }
    }

    /// <summary>
    /// On resume, set paused flag to false and hide the canvas.
    /// </summary>
    public void CloseModal()
    {
        this.Close();
        this.OnClose.Invoke();
        if (this.window)
        {
            this.window.SetActive(false);
        }
    }

    /// <summary>
    /// Popup coroutine.
    /// </summary>
    /// <param name="action">Callback</param>
    /// <returns>Returns coroutine.</returns>
    public IEnumerator Popup(UnityAction action)
    {
        this.ShowModal(action);
        while (this.isOpen)
        {
            yield return null;
        }
    }

    /// <summary>
    /// Test the popup function.
    /// </summary>
    [Button("Test Popup")]
    public void TestPopup()
    {
        this.gameObject.SetActive(true);
        this.StartCoroutine(Popup(LogClose));
    }

    /// <summary>
    /// On modal close, log it.
    /// </summary>
    public void LogClose()
    {
        Debug.Log("Closed modal.");
    }

}
