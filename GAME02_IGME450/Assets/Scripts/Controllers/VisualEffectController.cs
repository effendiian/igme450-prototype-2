using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;
using MHO.Extensions;

/// <summary>
/// Idle - pending user input to create a new effect.
/// </summary>
public class VFXControllerIdleState<T> : State where T : IVisualEffect
{

    /// <summary>
    /// Controller reference.
    /// </summary>
    private VisualEffectController<T> controller;

    /// <summary>
    /// Track the position of the last frame.
    /// </summary>
    private bool lastFrameUp = true;
    
    private Camera uiCamera;

    /// <summary>
    /// Construct state.
    /// </summary>
    /// <param name="engine">State machine.</param>
    public VFXControllerIdleState(StateMachine engine, VisualEffectController<T> controller) : base(engine)
    {
        this.uiCamera = this.uiCamera ?? GameObject.FindGameObjectWithTag("UICamera").GetComponent<Camera>();
        this.controller = controller;
    }

    /// <summary>
    /// On user click, get effect position and attempt to spawn an effect.
    /// </summary>
    public override void HandleInput()
    {
        base.HandleInput();
        bool thisFrameUp = Input.GetMouseButtonUp(0);
        if(thisFrameUp && !this.lastFrameUp)
        {
            Vector3 position = this.uiCamera.ScreenToWorldPoint(Input.mousePosition);
            this.controller.SpawnEffect(position);
        }
        this.lastFrameUp = thisFrameUp;
    }

}

/// <summary>
/// Buffer - State with delay.
/// </summary>
/// <typeparam name="T"></typeparam>
public class VFXControllerBufferState<T> : State where T : IVisualEffect
{

    private VisualEffectController<T> controller = null;

    /// <summary>
    /// Delay timer.
    /// </summary>
    private float timer = 0.0f;

    /// <summary>
    /// Delay timer start value.
    /// </summary>
    private float delay = 0.02f;

    /// <summary>
    /// Is buffer counting?
    /// </summary>
    private bool isRunning = false;

    /// <summary>
    /// Determine if done.
    /// </summary>
    public bool IsDone => this.timer <= 0.0f;
    
    /// <summary>
    /// Construct state.
    /// </summary>
    /// <param name="engine">State machine.</param>
    public VFXControllerBufferState(StateMachine engine, VisualEffectController<T> controller, float delay = 1.0f) : base(engine)
    {
        this.controller = controller;
        this.delay = delay;
        this.timer = delay;
        this.isRunning = false;
    }

    /// <summary>
    /// On enter.
    /// </summary>
    public override void Enter()
    {
        base.Enter();
        this.isRunning = true;
        this.timer = this.delay;
    }

    /// <summary>
    /// Count down the timer.
    /// </summary>
    public override void Update()
    {
        base.Update();
        this.timer -= Time.deltaTime;

        if (this.IsDone)
        {
            this.controller.StartIdle();
        }
    }

    /// <summary>
    /// Stop running the timer.
    /// </summary>
    public override void Exit()
    {
        base.Exit();
        this.isRunning = false;
        this.timer = 0.0f;
    }

    /// <summary>
    /// Delay duration.
    /// </summary>
    /// <param name="duration">Value to assign (in seconds)</param>
    public void SetDelay(float duration = 0.02f)
    {
        this.delay = duration;
    }

}

/// <summary>
/// Hold - limit reached; waiting for VFX to end in order to return to idle.
/// </summary>
public class VFXControllerHoldState<T> : State where T : IVisualEffect
{

    /// <summary>
    /// Controller reference.
    /// </summary>
    private VisualEffectController<T> controller;
    
    /// <summary>
    /// Construct state.
    /// </summary>
    /// <param name="engine">State machine.</param>
    public VFXControllerHoldState(StateMachine engine, VisualEffectController<T> controller) : base(engine)
    {
        this.controller = controller;
    }

    /// <summary>
    /// Check controller queue to see if limit is removed.
    /// </summary>
    public override void Update()
    {
        base.Update();
        if (this.controller.AtLimit)
        {
            // Try and remove completed effects.
            this.controller.FlushComplete();
        }

        this.controller.StartBuffer();
    }

}

/// <summary>
/// Base controller for triggering VisualEffects.
/// </summary>
public abstract class VisualEffectController<T> : MonoBehaviour where T : IVisualEffect {

    /////////////////////////////////////
    // Fields / Properties
    /////////////////////////////////////

    /// <summary>
    /// Prefab for instantiation of visual effect.
    /// </summary>
    [SerializeField, Required, Label("Visual Effect Prefab")]
    protected GameObject vfxPrefab = null;

    /// <summary>
    /// Pool for VFX.
    /// </summary>
    [SerializeField, Label("Visual Effect Pool")]
    protected GameObjectPool vfxPool = null;

    /// <summary>
    /// Controller state machine.
    /// </summary>
    protected StateMachine controller = null;

    /// <summary>
    /// Idle state.
    /// </summary>
    protected VFXControllerIdleState<T> idleState;

    /// <summary>
    /// Buffer state.
    /// </summary>
    protected VFXControllerBufferState<T> bufferState;

    /// <summary>
    /// Hold state.
    /// </summary>
    protected VFXControllerHoldState<T> holdState;

    /// <summary>
    /// VFX queue.
    /// </summary>
    protected Queue<T> vfxQueue = null;

    /// <summary>
    /// Limit of simultaneous VFX in the queue.
    /// </summary>
    [SerializeField, Range(1, 25)]
    protected int vfxLimit = 1;

    /// <summary>
    /// Small time buffer between clicks.
    /// </summary>
    [SerializeField, Range(0.0f, 2.0f)]
    protected float vfxBuffer = 1;

    /// <summary>
    /// Current size of VFX effects.
    /// </summary>
    public int Size => (this.vfxQueue != null) ? this.vfxQueue.Count : 0;

    /// <summary>
    /// Max limit.
    /// </summary>
    public int Capacity => (this.vfxQueue != null) ? this.vfxLimit : 0;

    /// <summary>
    /// Determine if we are at the limit.
    /// </summary>
    public bool AtLimit => this.Size >= this.Capacity;

    /////////////////////////////////////
    // Unity Behaviours
    /////////////////////////////////////

    /// <summary>
    /// Prepare the state machine.
    /// </summary>
    protected void Awake()
    {
        this.controller = this.controller ?? this.GetOrAddComponent<StateMachine>();
        this.vfxQueue = this.vfxQueue ?? new Queue<T>();
        this.vfxPool = new GameObjectPool(vfxPrefab, vfxLimit, true);
        this.idleState = this.idleState ?? new VFXControllerIdleState<T>(this.controller, this);
        this.bufferState = this.bufferState ?? new VFXControllerBufferState<T>(this.controller, this, this.vfxBuffer);
        this.holdState = this.holdState ?? new VFXControllerHoldState<T>(this.controller, this);

        this.controller.Initialize(this.idleState);
    }

    /// <summary>
    /// Update the visual effects controller.
    /// </summary>
    protected void Update()
    {
        this.FlushComplete();
        this.controller.CurrentState.HandleInput();
        this.controller.CurrentState.Update();
    }

    /////////////////////////////////////
    // Effect Controls
    /////////////////////////////////////

    /// <summary>
    /// Flush completed effects.
    /// </summary>
    public virtual void FlushComplete()
    {
        for(int i = 0; i < this.Size; i++)
        {
            T effect = vfxQueue.Peek();
            if (effect.IsStopped)
            {
                // Remove element from the queue.
                effect = vfxQueue.Dequeue();

                // Set it as inactive, to give it back to the pool.
                effect.Release();
            }
        }
    }

    /// <summary>
    /// Start Idle state.
    /// </summary>
    public virtual void StartIdle() =>  this.controller.ChangeState(this.idleState);

    /// <summary>
    /// Start Buffer state.
    /// </summary>
    public virtual void StartBuffer()
    {
        this.bufferState.SetDelay(this.vfxBuffer);
        this.controller.ChangeState(this.bufferState);
    }

    /// <summary>
    /// Start Hold state.
    /// </summary>
    public virtual void StartHold() => this.controller.ChangeState(this.holdState);

    /// <summary>
    /// 
    /// </summary>
    /// <param name="location"></param>        
    public void SpawnEffect(Vector3? location = null)
    {
        // Get the position.
        Vector3 position = (location.HasValue) ? location.Value : Vector3.zero;

        // Spawn effect.
        this.StartCoroutine(SpawnEffectCoroutine(position));

        // Check if limit is reached.
        if (!this.AtLimit)
        {
            this.StartHold();
        } else
        {
            this.StartBuffer();
        }
    }

    /// <summary>
    /// Spawn and play effect at specified location.
    /// </summary>
    /// <param name="location">Location.</param>
    /// <returns>Returns coroutine.</returns>
    protected IEnumerator SpawnEffectCoroutine(Vector3 location)
    {
        if (this.AtLimit)
        {
            this.vfxQueue.Peek().Stop();
            this.FlushComplete();
        }

        GameObject effectObject = vfxPool.GetPooledObject();
        if (effectObject)
        {
            T effect = effectObject.GetComponent<T>();
            vfxQueue.Enqueue(effect);
            effect.Enable();
            effect.Place(this.gameObject, location);
            effect.Play();
        }

        yield return null;
    }
    



}
