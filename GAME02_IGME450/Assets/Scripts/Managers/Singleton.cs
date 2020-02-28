using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MHO.Extensions;

/// <summary>
/// Inherit from this base class to create a singleton.
/// Based off: http://wiki.unity3d.com/index.php/Singleton
/// </summary>
/// <typeparam name="T">MonoBehaviour that inherits this class.</typeparam>
public class Singleton<T> : MonoBehaviour where T : MonoBehaviour
{

    /// <summary>
    /// If set to delete, prevent access.
    /// </summary>
    private static bool m_Garbage = false;

    /// <summary>
    /// Thread lock object.
    /// </summary>
    private static object m_ThreadLocker = new object();

    /// <summary>
    /// GameObject with manager components.
    /// </summary>
    private static GameObject m_Manager;

    /// <summary>
    /// Singleton instance.
    /// </summary>
    private static T m_Instance;

    /// <summary>
    /// Manager GameObject.
    /// </summary>
    protected static GameObject Manager
    {
        get
        {
            if(m_Garbage)
            {
                Debug.LogWarning($"[Singleton] Instance '{typeof(T)}' already destroyed. Returning null.");
                return null;
            }

            if (!m_Manager)
            {
                // If null, search for tagged object.
                GameObject[] managers = GameObject.FindGameObjectsWithTag("Manager");
                for(int i = 0; i < managers.Length; i++)
                {
                    if (managers[i].GetComponent<T>())
                    {
                        // If component exists on object, we can keep it.
                        m_Manager = managers[i];
                        break;
                    }
                }

                // If still null, create and tag the object.
                if (!m_Manager)
                {
                    // Create and tag the Manager object.
                    m_Manager = new GameObject($"{typeof(T)}")
                    {
                        tag = "Manager"
                    };
                }
            }

            // Return the reference.
            return m_Manager;
        }
    }

    /// <summary>
    /// Accessor to the Singleton.
    /// </summary>
    public static T Instance
    {
        get
        {


            lock (m_ThreadLocker)
            {
                if (!m_Instance)
                {
                    // If instance is null, search for instance.
                    m_Instance = (T)FindObjectOfType(typeof(T));

                    // If instance doesn't exist, create a new one.
                    if (!m_Instance)
                    {
                        // Find tagged manager GameObject to attach Singleton to.
                        m_Instance = Manager.GetOrAddComponent<T>();
                    }
                }

                // Return reference.
                return m_Instance;
            }
        }
    }

    /// <summary>
    /// Return the Singleton{T} name.
    /// </summary>
    protected virtual string Name
    {
        get
        {
            return $"'{typeof(T)}' [Singleton]";
        }
    }

    /// <summary>
    /// Flag as inaccessible once quitting.
    /// </summary>
    private void OnApplicationQuit() => m_Garbage = true;

    /// <summary>
    /// Flag as inaccessible once destroying.
    /// </summary>
    private void OnDestroy() => m_Garbage = true;
    
}
