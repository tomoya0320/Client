using Cysharp.Threading.Tasks;
using System;
using UnityEngine;

namespace Battle {
  public enum BattleState {
    None, // 退出循环的标记
    Load,
    Run,
    Settle,
    Exit,
  }

  public class BattleManager {
    private BattleData BattleData;
    private BattleState BattleState;
    public UnitManager UnitManager { get; private set; }
    public BuffManager BuffManager { get; private set; }
    public MagicManager MagicManager { get; private set; }
    public BehaviorManager BehaviorManager { get; private set; }
    public DamageManager DamageManager { get; private set; }
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

      Instance.Update();
      return true;
    }

    private BattleManager(BattleData battleData) {
      BattleState = BattleState.None;
      BattleData = battleData;
      UnitManager = new UnitManager(this);
      BuffManager = new BuffManager(this);
      MagicManager = new MagicManager(this);
      BehaviorManager = new BehaviorManager(this);
      DamageManager = new DamageManager(this);
      Blackboard = new Blackboard();
      ObjectPool = new ObjectPool();
    }

    public async void Update() {
      do {
        switch (BattleState) {
          case BattleState.None:
            BattleState = BattleState.Load;
            break;
          case BattleState.Load:
            await Load();
            BattleState = BattleState.Run;
            break;
          case BattleState.Run:
            await Run();
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
      MagicManager.CleanUp();
      MagicManager = null;
      BehaviorManager.CleanUp();
      BehaviorManager = null;
      DamageManager = null;
      Blackboard = null;
      ObjectPool = null;

      GC.Collect();
      Debug.Log("退出战斗");
    }

    private async UniTask Run() {
      // 执行回合开始前的行为树
      await BehaviorManager.Run(BehaviorTime.ON_BEFORE_TURN);

      // 回合中的逻辑
      Debug.Log("回合中...");
      await UniTask.Delay(1000);

      // 执行回合结束后的行为树
      await BehaviorManager.Run(BehaviorTime.ON_LATE_TURN);
    }
  }
}