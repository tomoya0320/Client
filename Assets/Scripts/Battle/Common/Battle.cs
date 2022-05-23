using Cysharp.Threading.Tasks;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace GameCore {
  public enum BattleState {
    None, // 退出循环的标记
    Load,
    Run,
    Settle,
    Exit,
  }

  public enum BattleTurnPhase {
    ON_BEFORE_TURN,
    ON_TURN,
    ON_LATE_TURN,
  }

  public class Battle {
    private BattleState BattleState;

    public UnitManager UnitManager { get; private set; }
    public BuffManager BuffManager { get; private set; }
    public MagicManager MagicManager { get; private set; }
    public BehaviorManager BehaviorManager { get; private set; }
    public AttribManager AttribManager { get; private set; }
    public CardManager CardManager { get; private set; }

    public PlayerManager PlayerManager { get; private set; }
    public DamageManager DamageManager { get; private set; }

    public Blackboard Blackboard { get; private set; }
    public ObjectPool ObjectPool { get; private set; }
    public static Battle Instance { get; private set; }

    public static bool Enter(BattleData battleData) {
      if(Instance != null) {
        Debug.LogError("上一场战斗未结束!");
        return false;
      }

      // 战斗数据初始化
      Instance = new Battle(battleData);

      Instance.Update();
      return true;
    }

    private Battle(BattleData battleData) {
      //-----------------first--------------------
      BattleState = BattleState.None;

      UnitManager = new UnitManager(this);
      BuffManager = new BuffManager(this);
      MagicManager = new MagicManager(this);
      BehaviorManager = new BehaviorManager(this);
      AttribManager = new AttribManager(this);
      CardManager = new CardManager(this);

      PlayerManager = new PlayerManager(this);
      DamageManager = new DamageManager(this);

      Blackboard = new Blackboard();
      ObjectPool = new ObjectPool();
      //------------------------------------------
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
            Settle();
            BattleState = BattleState.Exit;
            break;
          case BattleState.Exit:
            Exit();
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

    private void Settle() {
      Debug.Log("战斗结算");
    }

    private void Exit(bool force = false) {
      Instance = null;

      UnitManager.Release();
      UnitManager = null;

      BuffManager.Release();
      BuffManager = null;

      MagicManager.Release();
      MagicManager = null;

      BehaviorManager.Release();
      BehaviorManager = null;

      AttribManager.Release();
      AttribManager = null;

      CardManager.Release();
      CardManager = null;

      PlayerManager = null;
      DamageManager = null;

      Blackboard = null;
      ObjectPool = null;

      GC.Collect();
      Debug.Log("退出战斗");
    }

    private async UniTask Run() {
      // 先结算buff
      BuffManager.Update(BattleTurnPhase.ON_BEFORE_TURN);
      // 执行回合开始前的行为树
      await BehaviorManager.Run(BehaviorTime.ON_BEFORE_TURN);

      // 先结算buff
      BuffManager.Update(BattleTurnPhase.ON_TURN);
      // TODO:回合中的逻辑


      // 先结算buff
      BuffManager.Update(BattleTurnPhase.ON_LATE_TURN);
      // 执行回合结束后的行为树
      await BehaviorManager.Run(BehaviorTime.ON_LATE_TURN);
    }
  }
}