using Cysharp.Threading.Tasks;
using System.Collections.Generic;

namespace GameCore {
  public abstract class State<T> where T : class {
    public int StateId { get; private set; }
    public T Owner => StateMachine?.Owner;
    public StateMachine<T> StateMachine { get; private set; }

    protected State(int stateId, StateMachine<T> stateMachine) {
      StateId = stateId;
      StateMachine = stateMachine;
    }

    public virtual UniTask OnEnter(int lastStateId, Context context = null) => UniTask.CompletedTask;
    public virtual UniTask OnExit(int nextStateId, Context context = null) => UniTask.CompletedTask;
    public virtual bool CheckLeave(int nextStateId) => true;
  }

  public abstract class StateMachine<T> where T : class {
    protected Dictionary<int, State<T>> States = new Dictionary<int, State<T>>();
    public T Owner { get; private set; }
    public State<T> CurrentState { get; protected set; }

    public StateMachine(T owner) => Owner = owner;

    public bool RegisterState(State<T> state) {
      if (state == null || States.ContainsKey(state.StateId)) {
        return false;
      }
      States.Add(state.StateId, state);
      return true;
    }

    public async UniTask<bool> SwitchState(int nextStateId, Context context = null) {
      if (States.TryGetValue(nextStateId, out var nextState) && CurrentState.CheckLeave(nextStateId)) {
        int lastStateId = CurrentState.StateId;
        await CurrentState.OnExit(nextStateId, context);
        CurrentState = nextState;
        await CurrentState.OnEnter(lastStateId, context);
        
        return true;
      }
      return false;
    }
  }
}