using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Extension namespace for extension methods.
namespace MHO.Extensions
{

    /// <summary>
    /// Contains extension methods for <see cref="GameObject"/>s.
    /// </summary>
    public static class GameObjectExtensions
    {

        /// <summary>
        /// Get or add a component to a <see cref="GameObject"/>.
        /// </summary>
        /// <typeparam name="T">Type of <see cref="Component"/> to find.</typeparam>
        /// <param name="gameObject"><see cref="GameObject"/> reference to extend.</param>
        /// <returns>Returns reference to retrieved (or added) component.</returns>
        public static T GetOrAddComponent<T>(this GameObject gameObject) 
            where T : Component 
            => gameObject.GetComponent<T>() ?? gameObject.AddComponent<T>();

        public static T GetOrAddComponent<T>(this MonoBehaviour behaviour)
            where T : Component
            => behaviour.GetComponent<T>() ?? behaviour.gameObject.GetOrAddComponent<T>();

    }
}