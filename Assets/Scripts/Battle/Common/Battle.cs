using Cysharp.Threading.Tasks;
using System;
using UnityEngine;

namespace GameCore {
  public enum BattleState {
    None,
    Load,
    Run,
    Exit,
  }

  public enum BattleTurnPhase {
    ON_BEFORE_TURN,
    ON_TURN,
    ON_LATE_TURN,
  }

  public class Battle {
    private BattleState BattleState;
    public BattleData BattleData { get; private set; }
    public UnitManager UnitManager { get; private set; }
    public BuffManager BuffManager { get; private set; }
    public MagicManager MagicManager { get; private set; }
    public BehaviorManager BehaviorManager { get; private set; }
    public AttribManager AttribManager { get; private set; }
    public CardManager CardManager { get; private set; }
    public LevelManager LevelManager { get; private set; }

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

      // 战斗实例初始化
      Instance = new Battle(battleData);
      // 首先是加载资源
      Instance.BattleState = BattleState.Load;
      Instance.Update();
      return true;
    }

    private Battle(BattleData battleData) {
      //-----------------first--------------------
      BattleData = battleData;
      UnitManager = new UnitManager(this);
      BuffManager = new BuffManager(this);
      MagicManager = new MagicManager(this);
      BehaviorManager = new BehaviorManager(this);
      AttribManager = new AttribManager(this);
      CardManager = new CardManager(this);
      LevelManager = new LevelManager(this);

      PlayerManager = new PlayerManager(this);
      DamageManager = new DamageManager(this);

      Blackboard = new Blackboard();
      ObjectPool = new ObjectPool();
      //------------------------------------------
    }

    public async void Update() {
      while (BattleState != BattleState.None) {
        switch (BattleState) {
          case BattleState.Load:
            await Load();
            break;
          case BattleState.Run:
            await Run();
            break;
          case BattleState.Exit:
            await Clear();
            break;
        }
      }
    }

    private async UniTask Load() {
      // 加载关卡模板
      var levelTemplate = await LevelManager.Preload(BattleData.LevelId);
      // 加载关卡行为树
      foreach (var behaviorId in levelTemplate.BehaviorIds) {
        var behavior = await BehaviorManager.Preload(behaviorId);
        if (behavior) {
          BehaviorManager.AddBehavior(behaviorId);
        }
      }      
      // 先加载我方角色
      foreach (var unitData in BattleData.PlayerData.UnitData) {
        await PreloadUnit(unitData);
      }
      PlayerManager.Add(BattleData.PlayerData);
      // 再加载敌方角色
      foreach (var enemyData in levelTemplate.EnemyData) {
        foreach (var unitData in enemyData.UnitData) {
          await PreloadUnit(unitData);
        }
        PlayerManager.Add(enemyData);
      }

      BattleState = BattleState.Run;
      Debug.Log("战斗资源加载完毕");
    }

    private async UniTask PreloadUnit(UnitData unitData) {
      await UnitManager.Preload(unitData.TemplateId); // 角色模板
      foreach (var cardData in unitData.CardData) {
        await CardManager.Preload(cardData.TemplateId); // 卡牌模板
      }
    }

    public void Exit(bool force = false) {
      BattleState = BattleState.Exit;
      Debug.Log("退出战斗");
    }

    private async UniTask Clear() {
      Instance = null;
      BattleData = null;
      BattleState = BattleState.None;

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

      LevelManager.Release();
      LevelManager = null;

      PlayerManager = null;
      DamageManager = null;

      Blackboard = null;
      ObjectPool = null;

      await UniTask.Yield();

      GC.Collect();
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