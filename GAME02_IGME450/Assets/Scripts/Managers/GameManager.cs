using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using MHO.Extensions;

/// <summary>
/// GameManager Singleton class.
/// </summary>
public class GameManager : Manager<GameManager>
{

    /// <summary>
    /// Allow inheritance but reject constructor calls for this class.
    /// </summary>
    protected GameManager() { }

    /// <summary>
    /// Reference to the Globals component.
    /// </summary>
    public Globals Globals { get; protected set; }

    /// <summary>
    /// Setup the Manager class.
    /// </summary>
    protected override void Setup()
    {
        // Get or add a new component of type Globals.
        this.Globals = this.gameObject.GetOrAddComponent<Globals>();
        this.Globals.Setup();
    }

    /// <summary>
    /// Quit the application.
    /// </summary>
    public void Quit()
    {
#if UNITY_EDITOR
        Debug.Log($"{this}: Quitting application.");
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif 
    }

    /// <summary>
    /// Attempt to load scene additively.
    /// </summary>
    /// <param name="scene">Scene name.</param>
    /// <returns>Returns enumerator.</returns>
    public IEnumerator LoadSceneAdditive(string scene)
    {
        // Yield return the loaded scene operation.
        yield return SceneManager.LoadSceneAsync(scene, LoadSceneMode.Additive);
    }

    /// <summary>
    /// Load a single scene.
    /// </summary>
    /// <param name="scene">Scene to load.</param>
    /// <returns>Returns enumerator.</returns>
    public IEnumerator LoadScene(string scene)
    {
        yield return SceneManager.LoadSceneAsync(scene, LoadSceneMode.Single);
    }



}
