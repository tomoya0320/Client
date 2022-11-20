using Cysharp.Threading.Tasks;
using System;
using System.Collections.Generic;
using System.Threading;

namespace GameCore {
  public abstract class State<T> where T : class {
    public int StateId { get; private set; }
    public T Owner => StateMachine?.Owner;
    public StateMachine<T> StateMachine { get; private set; }

    protected State(int stateId, StateMachine<T> stateMachine) {
      StateId = stateId;
      StateMachine = stateMachine;
    }

    public virtual UniTask OnEnter(State<T> lastState, Context context = null) => UniTask.CompletedTask;
    public virtual UniTask OnExit(State<T> nextState, Context context = null) => UniTask.CompletedTask;
    public virtual bool CheckLeave(State<T> nextState) => true;
    public virtual bool CheckEnter(State<T> lastState) => true;
  }

  public abstract class StateWithUpdate<T> : State<T> where T : class {
    private bool Updating;
    protected virtual CancellationToken CancellationToken => CancellationToken.None;

    protected StateWithUpdate(int stateId, StateMachine<T> stateMachine) : base(stateId, stateMachine) { }

    public override UniTask OnEnter(State<T> lastState, Context context = null) {
      Updating = true;
      Update();
      return base.OnEnter(lastState, context);
    }

    private async void Update() {
      while (Updating) {
        UpdateInternal();
        try {
          await UniTask.Yield(CancellationToken);
        } catch (OperationCanceledException) {
          break;
        }
      }
    }

    protected abstract void UpdateInternal();

    public override UniTask OnExit(State<T> nextState, Context context = null) {
      Updating = false;
      return base.OnExit(nextState, context);
    }
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
      if (States.TryGetValue(nextStateId, out var nextState) && CurrentState.CheckLeave(nextState) && nextState.CheckEnter(CurrentState)) {
        // 注意顺序
        var lastState = CurrentState;
        CurrentState = nextState;
        await lastState.OnExit(nextState, context);
        await nextState.OnEnter(lastState, context);

        return true;
      }
      return false;
    }
  }
}