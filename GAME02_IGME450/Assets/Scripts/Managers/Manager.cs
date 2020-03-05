using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Manager reference ensures separate Manager scene exists.
/// </summary>
public abstract class Manager<T> : Singleton<T> where T : MonoBehaviour
{

    /// <summary>
    /// ID for the manager scene.
    /// </summary>
    private const string ManagerSceneID = "Manager Scene";

    /// <summary>
    /// Reference for manager scene.
    /// </summary>
    private static Scene m_ManagerScene;

    /// <summary>
    /// Accessor to the ManagerScene.
    /// </summary>
    public static Scene ManagerScene
    {
        get {
            if(m_ManagerScene.name != ManagerSceneID || !m_ManagerScene.isLoaded)
            {
                // Search for scene by name, to see if it is loaded.
                m_ManagerScene = SceneManager.GetSceneByName(ManagerSceneID);
                if(m_ManagerScene.name != ManagerSceneID || !m_ManagerScene.isLoaded)
                {
                    // If scene still not found, create new one programmatically.
                    // Adds it programmatically.
                    m_ManagerScene = SceneManager.CreateScene(ManagerSceneID);               
                }
            }

            // Return scene reference.
            return m_ManagerScene;
        }

    }

    /// <summary>
    /// Return the Manager{T} name.
    /// </summary>
    protected override string Name
    {
        get
        {
            return $"{base.Name} [Manager]";
        }
    }

    /// <summary>
    /// When Awake is called.
    /// </summary>
    public virtual void Awake()
    {
        PrepareScene();
        Setup();
        Debug.Log($"Initialized {this.Name}.");
    }

    /// <summary>
    /// Ensures that this component exists on the Manager 
    /// tagged GameObject and that the GameObject exists 
    /// on the Manager active scene.
    /// </summary>
    private void PrepareScene()
    {
        Scene currentScene = Manager.scene;
        if(currentScene.name != ManagerScene.name)
        {
            // Move manager to the Manager scene.
            SceneManager.MoveGameObjectToScene(Manager, ManagerScene);

            // Remove currentScene (or unload) if scene root count is zero after moving.
            if(currentScene.rootCount == 0)
            {
                // Make the manager scene the active scene.
                SceneManager.SetActiveScene(ManagerScene);

                // Asynchronously unload the scene.
                SceneManager.UnloadSceneAsync(currentScene);
            }
        }
    }

    /// <summary>
    /// Initialization for the manager is executed here.
    /// </summary>
    protected abstract void Setup();

    /// <summary>
    /// Does nothing. Called by users to ensure Awake() has been run.
    /// </summary>
    public void DoNothing() { }

}
