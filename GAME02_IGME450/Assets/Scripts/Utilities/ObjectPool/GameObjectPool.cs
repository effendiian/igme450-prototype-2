using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Object pool for specifically <see cref="GameObject"/>s.
/// </summary>
public class GameObjectPool : ObjectPool<GameObject>
{
    
    /////////////////////////////////////
    // Fields / Properties
    /////////////////////////////////////
    
    /// <summary>
    /// Source object property.
    /// </summary>
    protected override GameObject Source {
        get => this.source;
        set => this.source = value;
    }

    /////////////////////////////////////
    // Constructors
    /////////////////////////////////////

    /// <summary>
    /// Create a GameObject pool.
    /// </summary>
    /// <param name="sourceObject">Source object to clone.</param>
    /// <param name="numbersOfItems">Number of items to allocate to collection.</param>
    /// <param name="permitExpansion">Permit expansion of the pool?</param>
    public GameObjectPool(GameObject sourceObject, int numbersOfItems = 1, bool permitExpansion = false) : base(sourceObject, numbersOfItems, permitExpansion) {}

    /////////////////////////////////////
    // Service Methods
    /////////////////////////////////////

    /// <summary>
    /// Instantiate object.
    /// </summary>
    /// <param name="source">Source object to instantiate new object with.</param>
    /// <returns>Returns instantiated object.</returns>
    protected override GameObject Instantiate() => (this.Source) ? GameObject.Instantiate(this.Source) : null;

    /// <summary>
    /// Disable item.
    /// </summary>
    /// <param name="item">Item to disable.</param>
    protected override void Disable(GameObject item) => item.SetActive(false);
    
    /// <summary>
    /// Clear the pool.
    /// </summary>
    public void Clear()
    {
        if(this.Size > 0)
        {
            // Destroy all game objects in the pool.
            foreach(GameObject go in this.Pool)
            {
                GameObject.Destroy(go);
            }
        }
    }

    /// <summary>
    /// Determine if item is active in the scene hierarchy and non-null.
    /// </summary>
    /// <param name="item">Item to check.</param>
    /// <returns>Returns true if item is active in the scene hierarchy.</returns>
    protected override bool IsActive(GameObject item) => item.activeInHierarchy;

    /// <summary>
    /// Destroy game object and remove from the list.
    /// </summary>
    /// <param name="item">Item to release.</param>
    protected override void ReleaseItem(GameObject item)
    {
        if (item && this.IsActive(item))
        {
            // Disable the item if it is active in the scene.
            this.Disable(item);

            if (this.AtCapacity || this.AboveCapacity)
            {
                // Remove and destroy item if it's above capacity and no longer active.
                this.Pool.Remove(item);
                GameObject.Destroy(item);
            }
        }



    }
}
