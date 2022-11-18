using Cysharp.Threading.Tasks;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace GameCore {
  [Flags]
  public enum PlayerCamp {
    [InspectorName("无")]
    NONE = 0,
    [InspectorName("我方")]
    ALLY = 1 << 0,
    [InspectorName("敌方")]
    ENEMY = 1 << 1,
    [InspectorName("全部")]
    ALL = ALLY | ENEMY,
  }

  public class Player : BattleBase {
    public int RuntimeId { get; private set; }
    public string PlayerId => PlayerData.PlayerId;
    public PlayerCamp PlayerCamp => PlayerData.PlayerCamp;
    public PlayerData PlayerData { get; private set; }
    public Blackboard Blackboard { get; private set; }
    public Unit Master { get; private set; }
    public Unit[] Units { get; private set; }
    public int DeadUnitCount;
    public int TotalUnitCount { get; }
    public bool Available => DeadUnitCount < TotalUnitCount;
    public bool IsSelf => Battle.SelfPlayer == this;
    private bool _EndTurnFlag;
    public bool EndTurnFlag {
      get => _EndTurnFlag;
      set {
        if (_EndTurnFlag == value) {
          return;
        }

        _EndTurnFlag = value;
        if (_EndTurnFlag) {
          AddOperation(null);
          Debug.Log("结束回合");
        }
      }
    }
    private Queue<BattleOperation> Operations = new Queue<BattleOperation>();
    public bool HasOperation => Operations.Count > 0;
    public event Action OnStartTurn;

    public Player(Battle battle, int runtimeId, PlayerData playerData) : base(battle) {
      Blackboard = Battle.ObjectPool.Get<Blackboard>();
      RuntimeId = runtimeId;
      PlayerData = playerData;
      Units = new Unit[playerData.UnitData.Length];
      for (int i = 0; i < Units.Length; i++) {
        Units[i] = Battle.UnitManager.Create(this, PlayerData.UnitData[i]);
      }
      Master = Units[PlayerData.FirstIndex];
      TotalUnitCount = Units.Length;
    }

    private async UniTask StartTurn() {
      foreach (var unit in Units) {
        await unit.UnitStateMachine.SwitchState((int)UnitState.IN_TURN);
      }
    }

    private async UniTask EndTurn() {
      foreach (var unit in Units) {
        await unit.UnitStateMachine.SwitchState((int)UnitState.OUT_TURN);
      }
    }

    private async UniTask InTurn() {
      while (!EndTurnFlag) {
        while (!HasOperation) {
          await Battle.BehaviorManager.RunRoot(TickTime.ON_TURN_WAIT_OP, Master);
          await UniTask.Yield();
        }
        await DoOperation();
      }
    }

    public async UniTask OnTurn() {
      OnStartTurn?.Invoke();
      await StartTurn();
      await InTurn();
      await EndTurn();
    }

    public void AddOperation(BattleOperation operation) {
      Operations.Enqueue(operation);
    }

    private async UniTask DoOperation() {
      if (Operations.TryDequeue(out var operation) && operation != null) {
        await operation.DoOperation();
        Battle.ObjectPool.Release(operation);
      }
    }
  }
}