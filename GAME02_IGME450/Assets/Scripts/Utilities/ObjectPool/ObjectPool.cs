using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Creates an object pool of a particular type.
/// Based off of: https://www.raywenderlich.com/847-object-pooling-in-unity
/// </summary>
/// <typeparam name="T">Type to pool</typeparam>
[System.Serializable]
public abstract class ObjectPool<T> {

    /////////////////////////////////////
    // Fields / Properties
    /////////////////////////////////////
    
    /// <summary>
    /// Collection of pooled objects.
    /// </summary>
    protected List<T> pool;

    /// <summary>
    /// Blueprint object to create pool items from.
    /// </summary>
    protected T source;

    /// <summary>
    /// Should the pool expand beyond the capacity?
    /// </summary>
    protected bool shouldExpand;

    /// <summary>
    /// Minimum number of items to store before removing from the pool.
    /// </summary>
    protected int capacity;

    /// <summary>
    /// Collection of pooled objects.
    /// </summary>
    protected List<T> Pool
    {
        get => this.pool = this.pool ?? new List<T>();
        set => this.pool = value;
    }
    
    /// <summary>
    /// Property reference to the blueprint object.
    /// </summary>
    protected abstract T Source
    {
        get;
        set;
    }

    /// <summary>
    /// Total size of the pool.
    /// </summary>
    public int Size => this.Pool.Count;

    /// <summary>
    /// Determine if the pool has no items.
    /// </summary>
    public bool IsEmpty => this.Size != 0;

    /// <summary>
    /// Determine if the pool has vacant space left. (Below capacity).
    /// </summary>
    protected bool HasVacancy => this.capacity > this.Size;

    /// <summary>
    /// Determine if the pool can expand.
    /// </summary>
    public bool CanExpand => this.shouldExpand || this.HasVacancy;

    /// <summary>
    /// Determine if the pool is at capacity.
    /// </summary>
    protected bool AtCapacity => this.capacity == this.Size;

    /// <summary>
    /// Determine if the pool is above capacity.
    /// </summary>
    protected bool AboveCapacity => this.capacity < this.Size;


    /////////////////////////////////////
    // Constructors
    /////////////////////////////////////

    /// <summary>
    /// Construct an ObjectPool using an input source object.
    /// </summary>
    /// <param name="sourceObject">Source object to pool.</param>
    /// <param name="permitExpansion">Permit expansion of the pool?</param>
    public ObjectPool(T sourceObject, bool permitExpansion = true) : this(sourceObject, 1, permitExpansion) { }

    /// <summary>
    /// Construct an ObjectPool using an input source object and a specific initial size.
    /// </summary>
    /// <param name="sourceObject">Source object to pool.</param>
    /// <param name="numberOfItems">Number of items to instantiate into the pool.</param>
    /// <param name="permitExpansion">Permit expansion of the pool?</param>
    public ObjectPool(T sourceObject, int numberOfItems = 1, bool permitExpansion = true) => this.Initialize(sourceObject, numberOfItems, permitExpansion);

    /////////////////////////////////////
    // Service Methods
    /////////////////////////////////////

    /// <summary>
    /// Instantiate a pool filled with items.
    /// </summary>
    /// <param name="sourceObject">Source object to pool.</param>
    /// <param name="numberOfItems">Number of items to instantiate into the pool.</param>
    /// <param name="permitExpansion">Permit expansion of the pool?</param>
    protected virtual void Initialize(T sourceObject, int numberOfItems, bool permitExpansion = true)
    {
        // Set member data.
        this.capacity = numberOfItems;
        this.shouldExpand = permitExpansion;

        // Instantiate initial set.
        this.Source = sourceObject;
        this.Pool = new List<T>(this.capacity);
        this.AllocateItems(numberOfItems, true); // When initializing, force allocation for set size.

    }

    /// <summary>
    /// Instantiate new, single item, disable it, and add it to the pool.
    /// </summary>
    /// <param name="ignoreCapacity">Ignore capacity?</param>
    protected virtual T AllocateItem(bool ignoreCapacity = false)
    {
        // If vacancies available and can expand.
        if (this.CanExpand || ignoreCapacity)
        {
            T clone = this.Instantiate();
            this.Disable(clone);
            this.Pool.Add(clone);
            return clone;
        }
        return default(T);
    }

    /// <summary>
    /// Instantiate new set of items.
    /// </summary>
    /// <param name="numberOfItems">Number of items to add to the pool.</param>
    /// <param name="ignoreCapacity">Ignore capacity?</param>
    protected virtual void AllocateItems(int numberOfItems, bool ignoreCapacity = false)
    {
        for(int i = 0; i < numberOfItems; i++)
        {
            this.AllocateItem(ignoreCapacity);   
        }
    }

    /// <summary>
    /// Deallocate a single item from the collection. If above capacity, remove from the pool.
    /// </summary>
    protected abstract void ReleaseItem(T item);

    /// <summary>
    /// Instantiate item to pool.
    /// </summary>
    /// <returns></returns>
    protected abstract T Instantiate();

    /// <summary>
    /// Disable item, after adding to pool.
    /// </summary>
    /// <param name="item">Item to disable.</param>
    protected abstract void Disable(T item);
    
    /// <summary>
    /// Determine if item is active in the scene hierarchy or alternative list.
    /// </summary>
    /// <param name="item">Item to check.</param>
    /// <returns>Returns true if item is active.</returns>
    protected abstract bool IsActive(T item);

    /// <summary>
    /// Determine if object is free in pool and return it.
    /// </summary>
    /// <returns>Returned pooled objects from the collection.</returns>
    public T GetPooledObject()
    {
        // Search for free item.
        foreach(T item in this.Pool)
        {
            if (!this.IsActive(item))
            {
                return item;
            }
        }

        // If item is not found, return a newly allocated one. If we can't expand, null is returned.
        return this.AllocateItem();
    }

}
