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
    public Blackboard Blackboard { get; private set; }
    public Unit Master { get; private set; }
    public Unit[] Units { get; private set; }
    public int DeadUnitCount;
    public int TotalUnitCount => Units.Length;
    public bool Available => DeadUnitCount < TotalUnitCount;
    public PlayerTurnState TurnState { get; private set; } = PlayerTurnState.NONE;
    private Queue<BattleOperation> OperationQueue = new Queue<BattleOperation>();

    public Player(Battle battle, int runtimeId, PlayerData playerData) : base(battle) {
      Blackboard = Battle.ObjectPool.Get<Blackboard>();
      RuntimeId = runtimeId;
      Units = new Unit[playerData.UnitData.Length];
      for (int i = 0; i < Units.Length; i++) {
        Units[i] = Battle.UnitManager.Create(this, playerData.UnitData[i]);
      }
      Master = Units[playerData.FirstIndex];
    }

    public void RefreshEnergy() {
      // 每回合开始前刷新能量(强制设置为最大值)
      foreach (var unit in Units) {
        if (!unit.IsDead) {
          unit.RefreshEnergy();
        }
      }
    }

    private void DrawCard() {
      foreach (var unit in Units) {
        if (!unit.IsDead) {
          unit.DrawCard();
        }
      }
    }

    private void DiscardCard() {
      foreach (var unit in Units) {
        if (!unit.IsDead) {
          unit.DiscardCard();
        }
      }
    }

    public async UniTask OnTurn() {
      DrawCard();

      do {
        TurnState = PlayerTurnState.WAIT_OP;
        await UniTask.WaitUntil(OnTurnWait);
        TurnState = PlayerTurnState.DO_OP;
        var operation = OperationQueue.Dequeue();
        await operation.DoOperation();
        Battle.ObjectPool.Release(operation);
      } while (TurnState != PlayerTurnState.END_TURN);

      DiscardCard();
    }

    private bool OnTurnWait() => OperationQueue.Count > 0;

    public void AddOperation(BattleOperation operation) {
      OperationQueue.Enqueue(operation);
    }

    public void EndTurn() {
      TurnState = PlayerTurnState.END_TURN;
    }
  }
}