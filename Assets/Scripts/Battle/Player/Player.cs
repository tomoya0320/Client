using Cysharp.Threading.Tasks;
using System.Collections.Generic;

namespace GameCore {
  public enum PlayerTurnState {
    NONE,
    WAIT_OP,
    DO_OP,
    END_TURN,
  }

  public class Player : BattleBase {
    public int RuntimeId { get; private set; }
    public string PlayerId => PlayerData.PlayerId;
    public PlayerData PlayerData { get; private set; }
    public Blackboard Blackboard { get; private set; }
    public Unit Master { get; private set; }
    public Unit[] Units { get; private set; }
    public UnitData[] UnitData => PlayerData.UnitData;
    public int DeadUnitCount;
    public int TotalUnitCount => Units.Length;
    public bool Available => DeadUnitCount < TotalUnitCount;
    public bool IsSelf => Battle.SelfPlayer == this;
    public PlayerTurnState TurnState { get; private set; } = PlayerTurnState.NONE;
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
      // ÿ�غϿ�ʼǰˢ������(ǿ������Ϊ���ֵ)
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

    public async UniTask OnTurn() {
      DrawCard();

      do {
        TurnState = PlayerTurnState.WAIT_OP;
        while (Operation == null) {
          await Battle.BehaviorManager.Run(BehaviorTime.ON_TURN_WAIT_OP, Master);
          await UniTask.Yield();
        }
        TurnState = PlayerTurnState.DO_OP;
        await Operation.DoOperation();
        Battle.ObjectPool.Release(Operation);
        Operation = null;
      } while (TurnState != PlayerTurnState.END_TURN);

      DiscardCard();
    }

    public bool DoOperation(BattleOperation operation) {
      if (TurnState != PlayerTurnState.WAIT_OP) {
        return false;
      }
      Operation = operation;
      return true;
    }

    public void EndTurn() {
      TurnState = PlayerTurnState.END_TURN;
    }
  }
}