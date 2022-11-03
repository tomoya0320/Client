using System.Collections.Generic;

namespace GameCore {
  public abstract class State {
    public int StateId { get; private set; }
    public StateMachine StateMachine { get; set; }

    protected State(int stateId) => StateId = stateId;

    public virtual void OnEnter(int lastStateId) { }
    public virtual void OnTrick() { }
    public virtual void OnExit(int nextStateId) { }
  }

  public abstract class StateBase<T> : State {
    public T Owner { get; private set; }

    protected StateBase(int id, T owner) : base(id) => Owner = owner;
  }

  public abstract class StateMachine {
    protected Dictionary<int, State> states = new Dictionary<int, State>();
    public State CurrentState { get; protected set; }

    public bool RegisterState(State state) {
      if (state == null || states.ContainsKey(state.StateId)) {
        return false;
      }
      states.Add(state.StateId, state);
      state.StateMachine = this;
      
      return true;
    }

    public bool RemoveState(int stateId) {
      if (states.ContainsKey(stateId)) {
        states[stateId].StateMachine = null;
        states.Remove(stateId);
        
        return true;
      }
      return false;
    }

    public void OnTrick() {
      CurrentState?.OnTrick();
    }

    /// <summary>
    /// 切换状态
    /// </summary>
    /// <param name="nextStateId">下一个状态的ID</param>
    /// <param name="force">只有当前状态和下一个状态不一致时才会切换，当force为true时忽略这条规则</param>
    /// <returns></returns>
    public bool SwitchState(int nextStateId, bool force = false) {
      if (states.ContainsKey(nextStateId) && (CurrentState.StateId != nextStateId || force)) {
        int lastStateId = CurrentState.StateId;
        CurrentState.OnExit(nextStateId);
        CurrentState = states[nextStateId];
        CurrentState.OnEnter(lastStateId);
        
        return true;
      }
      return false;
    }
  }
}