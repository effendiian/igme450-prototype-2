using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Scene Load Profile.
/// </summary>
[CreateAssetMenu(fileName = "New Load Profile", menuName = "Scenes/Load Profile")]
public class SceneLoadProfile : ScriptableObject
{

    /// <summary>
    /// Scenes to load additively.
    /// </summary>
    [SerializeField]
    private List<string> scenes;

    /// <summary>
    /// Property reference to load profile scenes.
    /// </summary>
    public List<string> Scenes
    {
        get => this.scenes = this.scenes ?? new List<string>();
    }

    /// <summary>
    /// Load scenes additively.
    /// </summary>
    /// <returns></returns>
    public IEnumerator LoadSceneAdditively()
    {
        // If scenes exist, assign value.
        int sceneIndex = (this.Scenes.Count > 0) ? 0 : -1;

        // Enumerate while loading scenes additively.
        while(sceneIndex >= 0 && sceneIndex < this.Scenes.Count)
        {
            yield return GameManager.Instance.LoadSceneAdditive(this.Scenes[sceneIndex]);
            sceneIndex++;
        }
    }

}
