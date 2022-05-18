using Cysharp.Threading.Tasks;
using System;
using UnityEngine;

namespace Battle {
  public enum BattleState {
    None, // 退出循环的标记
    Loading,
    Running,
    Settle,
    Exit,
  }

  public class BattleManager {
    private BattleData BattleData;
    private BattleState BattleState;
    public UnitManager UnitManager { get; private set; }
    public BuffManager BuffManager { get; private set; }
    public BehaviorManager BehaviorManager { get; private set; }
    public Blackboard Blackboard { get; private set; }
    public ObjectPool ObjectPool { get; private set; }
    public static BattleManager Instance { get; private set; }

    public static bool Enter(BattleData battleData) {
      if(Instance != null) {
        Debug.LogError("上一场战斗未结束!");
        return false;
      }

      // 战斗数据初始化
      Instance = new BattleManager(battleData);

      Instance.Run();
      return true;
    }

    private BattleManager(BattleData battleData) {
      BattleState = BattleState.None;
      BattleData = battleData;
      UnitManager = new UnitManager(this);
      BuffManager = new BuffManager(this);
      BehaviorManager = new BehaviorManager(this);
      Blackboard = new Blackboard();
      ObjectPool = new ObjectPool();
    }

    public async void Run() {
      do {
        switch (BattleState) {
          case BattleState.None:
            BattleState = BattleState.Loading;
            break;
          case BattleState.Loading:
            await Load();
            BattleState = BattleState.Running;
            break;
          case BattleState.Running:
            await BeforeTurn();
            await OnTurn();
            await LateTurn();
            break;
          case BattleState.Settle:
            await Settle();
            BattleState = BattleState.Exit;
            break;
          case BattleState.Exit:
            await Exit();
            BattleState = BattleState.None;
            break;
        }
      }
      while (BattleState != BattleState.None);
    }

    private async UniTask Load() {
      // Test
      await UniTask.Delay(1000);
      Debug.Log("战斗资源加载完毕");
    }

    private async UniTask Settle() {
      // Test
      await UniTask.Delay(1000);
      Debug.Log("战斗结算");
    }

    private async UniTask Exit(bool force = false) {
      // Test
      await UniTask.Delay(1000);
      Instance = null;
      BattleData = null;
      UnitManager = null;
      BuffManager = null;
      BehaviorManager = null;
      Blackboard = null;
      ObjectPool = null;

      GC.Collect();
      Debug.Log("退出战斗");
    }

    private async UniTask BeforeTurn() {
      // Test
      await UniTask.Delay(1000);
      Debug.Log("BeforeTurn");
    }

    private async UniTask OnTurn() {
      // Test
      await UniTask.Delay(1000);
      Debug.Log("OnTurn");
    }

    private async UniTask LateTurn() {
      // Test
      await UniTask.Delay(1000);
      Debug.Log("LateTurn");
    }
  }
}