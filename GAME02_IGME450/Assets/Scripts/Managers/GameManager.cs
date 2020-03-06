using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
#if UNITY_EDITOR
using UnityEditor.SceneManagement;
#endif
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
    /// Setup the Manager class.
    /// </summary>
    protected override void Setup()
    {
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
#if UNITY_EDITOR
        // Yield return the loaded scene operation.
        yield return EditorSceneManager.LoadSceneAsync(scene, LoadSceneMode.Additive);
#else
        // Yield return the loaded scene operation.
        yield return SceneManager.LoadSceneAsync(scene, LoadSceneMode.Additive);
#endif
    }

    /// <summary>
    /// Load a single scene.
    /// </summary>
    /// <param name="scene">Scene to load.</param>
    /// <returns>Returns enumerator.</returns>
    public IEnumerator LoadScene(string scene)
    {
#if UNITY_EDITOR
        EditorSceneManager.LoadScene(scene, LoadSceneMode.Single);
#else
        SceneManager.LoadScene(scene, LoadSceneMode.Single);
#endif
        yield return null;
    }



}
