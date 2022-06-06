using Cysharp.Threading.Tasks;
using System;
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
    public UnitData[] UnitData => PlayerData.UnitData;
    public int DeadUnitCount;
    public int TotalUnitCount => Units.Length;
    public bool Available => DeadUnitCount < TotalUnitCount;
    public bool IsSelf => Battle.SelfPlayer == this;
    public bool EndTurnFlag;
    private BattleOperation Operation;

    public Player(Battle battle, int runtimeId, PlayerData playerData) : base(battle) {
      Blackboard = Battle.ObjectPool.Get<Blackboard>();
      RuntimeId = runtimeId;
      PlayerData = playerData;
      Units = new Unit[playerData.UnitData.Length];
      for (int i = 0; i < Units.Length; i++) {
        Units[i] = Battle.UnitManager.Create(this, UnitData[i]);
      }
      Master = Units[PlayerData.FirstIndex];
    }

    public void RefreshEnergy() {
      // 每回合开始前刷新能量(强制设置为最大值)
      foreach (var unit in Units) {
        if (unit.UnitState == UnitState.ALIVE) {
          unit.RefreshEnergy();
        }
      }
    }

    private void DrawCard() {
      foreach (var unit in Units) {
        if (unit.UnitState == UnitState.ALIVE) {
          unit.DrawCard();
        }
      }
    }

    private void DiscardCard() {
      foreach (var unit in Units) {
        if (unit.UnitState == UnitState.ALIVE) {
          unit.DiscardCard();
        }
      }
    }

    private async UniTask PlayCard() {
      while (!EndTurnFlag) {
        while (Operation == null) {
          await Battle.BehaviorManager.RunRoot(TrickTime.ON_TURN_WAIT_OP, Master);
          await UniTask.Yield();
        }
        await Operation.DoOperation();
        Battle.ObjectPool.Release(Operation);
        Operation = null;
      }
    }

    public async UniTask OnTurn() {
      DrawCard();
      await PlayCard();
      DiscardCard();
    }

    public bool DoOperation(BattleOperation operation) {
      if (Operation != null) {
        return false;
      }
      Operation = operation;
      return true;
    }
  }
}