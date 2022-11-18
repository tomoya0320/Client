using Cysharp.Threading.Tasks;
using System;
using System.Threading;
using UnityEngine;
using Object = UnityEngine.Object;

namespace GameCore {
  public enum BattleState {
    None,
    Load,
    Run,
    Exit,
  }

  public class Battle {
    public CancellationToken CancellationToken { get; private set; }
    public BattleState BattleState { get; private set; }
    public BattleData BattleData { get; private set; }
    public LevelTemplate LevelTemplate { get; private set; }
    public Blackboard Blackboard { get; private set; }
    public ObjectPool ObjectPool { get; private set; }
    public Player CurPlayer { get; private set; }
    public Player SelfPlayer { get; private set; }
    public int Turn { get; private set; }

    #region Manager
    public UnitManager UnitManager { get; private set; }
    public BuffManager BuffManager { get; private set; }
    public MagicManager MagicManager { get; private set; }
    public BehaviorManager BehaviorManager { get; private set; }
    public AttribManager AttribManager { get; private set; }
    public CardManager CardManager { get; private set; }
    public LevelManager LevelManager { get; private set; }
    public SkillManager SkillManager { get; private set; }

    public PlayerManager PlayerManager { get; private set; }
    public DamageManager DamageManager { get; private set; }
    #endregion

    #region UI
    public UIBattle UIBattle { get; private set; } // Test
    #endregion

    #region Static
    public static Battle Instance { get; private set; }
    #endregion

    public static bool Enter(BattleData battleData, CancellationToken token) {
      if (Instance != null) {
        Debug.LogError("上一场战斗未结束!");
        return false;
      }
      // 战斗实例初始化
      Instance = new Battle(battleData, token);
      // 首先是加载资源
      Instance.BattleState = BattleState.Load;
      Instance.Update();
      return true;
    }

    private Battle(BattleData battleData, CancellationToken token) {
      BattleData = battleData;
      CancellationToken = token;
      UnitManager = new UnitManager(this);
      BuffManager = new BuffManager(this);
      MagicManager = new MagicManager(this);
      BehaviorManager = new BehaviorManager(this);
      AttribManager = new AttribManager(this);
      CardManager = new CardManager(this);
      LevelManager = new LevelManager(this);
      SkillManager = new SkillManager(this);

      PlayerManager = new PlayerManager(this);
      DamageManager = new DamageManager(this);

      Blackboard = new Blackboard();
      ObjectPool = new ObjectPool();
    }

    public async void Update() {
      while (BattleState != BattleState.None) {
        switch (BattleState) {
          case BattleState.Load:
            await Load();
            break;
          case BattleState.Run:
            try {
              await Run();
            } catch (OperationCanceledException) {
              BattleState = BattleState.Exit;
            }
            break;
          case BattleState.Exit:
            await Clear();
            break;
        }
      }
    }

    private async UniTask Load() {
      // step1:加载关卡和单位的相关资源
      LevelTemplate = LevelManager.Preload(BattleData.LevelId);
      foreach (var behaviorId in LevelTemplate.BehaviorIds) {
        await PreloadBehavior(behaviorId);
      }

      foreach (var unitData in BattleData.PlayerData.UnitData) {
        await PreloadUnit(unitData);
      }

      foreach (var playerData in LevelTemplate.PlayerData) {
        foreach (var unitData in playerData.UnitData) {
          await PreloadUnit(unitData);
        }
      }

      // step2:创建单位
      SelfPlayer = PlayerManager.Create(BattleData.PlayerData);
      foreach (var playerData in LevelTemplate.PlayerData) {
        PlayerManager.Create(playerData);
      }

      // step3:初始化关卡和单位的行为树
      foreach (var behaviorId in LevelTemplate.BehaviorIds) {
        await BehaviorManager.Add(behaviorId);
      }
      foreach (var unit in UnitManager.AllUnits) {
        await unit.InitBehavior();
      }

      // step4:加载战斗UI
      // ---------------------------------Test-------------------------------------------
      UIBattle = Object.Instantiate(GameResManager.LoadAsset<GameObject>("UIBattle"),
        GameObject.Find("UIRoot").transform).GetComponent<UIBattle>();
      UIBattle.InitBattle(this);
      // --------------------------------------------------------------------------------

      BattleState = BattleState.Run;
      Debug.Log("战斗加载完毕");
    }

    private async UniTask PreloadUnit(UnitData unitData) {
      var unitTemplate = UnitManager.Preload(unitData.TemplateId);
      AttribManager.Preload(unitTemplate.AttribId);
      foreach (var behaviorId in unitTemplate.BehaviorIds) {
        await PreloadBehavior(behaviorId);
      }
      // 预加载卡牌
      foreach (var cardData in unitData.CardData) {
        var cardTemplate = CardManager.Preload(cardData.TemplateId);
        foreach (var item in cardTemplate.LvCardItems) {
          var skillTemplate = SkillManager.Preload(item.SkillId);
          foreach (var skillEvent in skillTemplate.SKillEvents) {
            await PreloadMagic(skillEvent.MagicId);
          }
        }
      }
    }

    private async UniTask PreloadBehavior(string behaviorId) {
      var behavior = BehaviorManager.Preload(behaviorId);
      if (behavior) {
        foreach (var behaviorNode in behavior.nodes) {
          switch (behaviorNode) {
            case BehaviorFuncs.DoMagic doMagic:
              await PreloadMagic(doMagic.MagicId);
              break;
            case BehaviorFuncs.AddBuff addBuff:
              await PreloadBuff(addBuff.BuffId);
              break;
          }
        }
      }
    }

    private async UniTask PreloadMagic(string magicId) {
      var magicFunc = MagicManager.Preload(magicId);
      if (magicFunc) {
        switch (magicFunc) {
          case MagicFuncs.AddBehavior addBehavior:
            await PreloadBehavior(addBehavior.BehaviorId);
            break;
          case MagicFuncs.AddBuff addBuff:
            await PreloadBuff(addBuff.BuffId);
            break;
        }
      }
    }

    private async UniTask PreloadBuff(string buffId) {
      var buffTemplate = BuffManager.Preload(buffId);
      if (buffTemplate) {
        await PreloadMagic(buffTemplate.MagicId);
        await PreloadMagic(buffTemplate.IntervalMagicId);
      }
    }

    public UniTask Settle(bool isWin) {
      Debug.Log(isWin ? "Battle win" : "Battle lose");
      return UniTask.FromCanceled(CancellationToken);
    }

    private async UniTask Clear() {
      // ---------------------------------Test-------------------------------------------
      if (UIBattle) {
        Object.Destroy(UIBattle.gameObject);
      }
      // --------------------------------------------------------------------------------

      Instance = null;
      BattleData = null;
      CancellationToken = default;
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

      SkillManager.Release();
      SkillManager = null;

      PlayerManager = null;
      DamageManager = null;

      Blackboard = null;
      ObjectPool = null;
      CurPlayer = null;
      SelfPlayer = null;
      LevelTemplate = null;

      await UniTask.Yield();

      GC.Collect();
      Debug.Log("清理战斗");
    }

    private async UniTask Run() {
      // 更新当前回合的玩家
      CurPlayer = PlayerManager.MoveNext();
      if (CurPlayer == SelfPlayer) {
        Turn++;
      }
      Debug.Log($"当前玩家回合 id:{CurPlayer.RuntimeId} name:{CurPlayer.PlayerId}");
      // 回合中的逻辑
      await CurPlayer.OnTurn();
    }
  }
}