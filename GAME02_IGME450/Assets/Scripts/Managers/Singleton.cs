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
    /// Constructor can only be used internally.
    /// </summary>
    protected Singleton() { }

    /// <summary>
    /// GameObject with manager components.
    /// </summary>
    private static GameObject m_Managers;

    /// <summary>
    /// Singleton instance.
    /// </summary>
    private static T m_Instance;

    /// <summary>
    /// Manager GameObject.
    /// </summary>
    protected static GameObject Managers
    {
        get
        {
            if(m_Garbage)
            {
                Debug.LogWarning($"[Singleton] '{typeof(T)}' Instance already destroyed. Returning null.");
                return null;
            }

            if (!m_Managers)
            {
                // If null, search for tagged object.
                m_Managers = GameObject.FindGameObjectWithTag("Manager");

                // If still null, create and tag the object.
                if (!m_Managers)
                {
                    // Create and tag the Manager object.
                    m_Managers = new GameObject("Managers")
                    {
                        tag = "Manager"
                    };
                }
            }

            // Return the reference.
            return m_Managers;
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
                        m_Instance = Managers.GetOrAddComponent<T>();
                    }
                }

                // Return reference.
                return m_Instance;
            }
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
