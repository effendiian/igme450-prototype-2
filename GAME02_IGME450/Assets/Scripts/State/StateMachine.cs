using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Null State is used to return values without causing Null reference errors.
/// </summary>
public sealed class NullState : State
{
    /// <summary>
    /// Construct a null state that cannot be constructed beyond inheritance implementations.
    /// </summary>
    public NullState(StateMachine engine) : base(engine) { }
}

/// <summary>
/// StateMachine tracks state.
/// Based off: https://www.raywenderlich.com/6034380-state-pattern-using-unity
/// </summary>
public class StateMachine : MonoBehaviour
{
    
    /// <summary>
    /// Private reference.
    /// </summary>
    private Stack<State> m_States;

    /// <summary>
    /// Null state reference.
    /// </summary>
    private State m_NullState;

    /// <summary>
    /// State collection.
    /// </summary>
    protected Stack<State> States { 
        get {
            m_States = m_States ?? new Stack<State>();
            return m_States;
        } 
        set {
            m_States = value;
        }
    }

    /// <summary>
    /// Initial state.
    /// </summary>
    protected State StartingState { get; set; }

    /// <summary>
    /// Current state.
    /// </summary>
    public State CurrentState => (!this.IsDone) ? this.States.Peek() : this.NullState;

    /// <summary>
    /// Current state.
    /// </summary>
    public State NullState {
        get
        {
            m_NullState = m_NullState ?? new NullState(this);
            return m_NullState;
        }
    }

    /// <summary>
    /// StateMachine can be considered done if the States collection count is zero.
    /// </summary>
    public bool IsDone => this.States.Count == 0;

    /// <summary>
    /// Start is called before the first frame update
    /// </summary>
    void Start()
    {
        this.StartingState = this.StartingState ?? null;
        this.States = this.States ?? null;
    }

    /// <summary>
    /// Set the starting state and initialize the state.
    /// </summary>
    /// <param name="startingState">State to initialize the machine with.</param>
    public void Initialize(State startingState)
    {
        // Initialize the collection.
        this.States = new Stack<State>();
        this.StartingState = startingState;

        // Change into the starting state.
        this.PushState(startingState);
    }

    /// <summary>
    /// Reset the state machine using the stored starting state.
    /// </summary>
    public void Reset() => this.Initialize(this.StartingState);

    /// <summary>
    /// Enter into a sub-state, pushing a new one onto the Stack without exiting the previous context.
    /// </summary>
    /// <param name="subState">Sub-state to enter into.</param>
    public void PushState(State subState)
    {
        // Push the sub state onto the stack and initialize it.
        this.States.Push(subState);
        this.CurrentState.Enter();
    }

    /// <summary>
    /// Exit current state and pop it from the stack.
    /// </summary>
    public void PopState()
    {
        // Exit the current state and pop it from the stack.
        this.CurrentState.Exit();
        this.States.Pop();
    }

    /// <summary>
    /// Change states by popping the last one off of the stack, before pushing the new one on.
    /// </summary>
    /// <param name="nextState">Next state to switch into.</param>
    public void ChangeState(State nextState)
    {
        // Pop the current state after exiting it.
        this.PopState();

        // Push the current state and then enter it.
        this.PushState(nextState);
    }

    /// <summary>
    /// Exit all states on the stack and clear it.
    /// </summary>
    public void End()
    {
        while (!this.IsDone)
        {
            this.PopState();
        }
    }

}

/// <summary>
/// StateMachine tracks state.
/// Based off: https://www.raywenderlich.com/6034380-state-pattern-using-unity
/// </summary>
public abstract class StateMachine<T> : StateMachine where T : State
{
    
    /// <summary>
    /// State collection.
    /// </summary>
    // protected Stack<T> States { get; set; }

    /// <summary>
    /// Initial state.
    /// </summary>
    // protected T StartingState { get; set; }

    /// <summary>
    /// Current state.
    /// </summary>
    // public T CurrentState { get => this.States.Peek(); }
    
    /// <summary>
    /// StateMachine can be considered done if the States collection count is zero.
    /// </summary>
    // public bool IsDone { get => this.States.Count == 0; }

    /// <summary>
    /// Start is called before the first frame update
    /// </summary>
    void Start()
    {
        this.StartingState = null;
        this.States = null;
    }
    
    /// <summary>
    /// Set the starting state and initialize the state.
    /// </summary>
    /// <param name="startingState">State to initialize the machine with.</param>
    public void Initialize(T startingState) => base.Initialize(startingState);

    /// <summary>
    /// Enter into a sub-state, pushing a new one onto the Stack without exiting the previous context.
    /// </summary>
    /// <param name="subState">Sub-state to enter into.</param>
    public void PushState(T subState) => base.PushState(subState);

    /// <summary>
    /// Change states by popping the last one off of the stack, before pushing the new one on.
    /// </summary>
    /// <param name="nextState">Next state to switch into.</param>
    public void ChangeState(T nextState) => base.ChangeState(nextState);

}
