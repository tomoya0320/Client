using Cysharp.Threading.Tasks;
using GameCore.MagicFuncs;
using GameCore.UI;
using System;
using System.Threading;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace GameCore {
  public enum BattleState {
    NONE,
    LOAD,
    RUN,
    EXIT,
  }

  public class Battle {
    private CancellationTokenSource CancellationTokenSource;
    public CancellationToken CancellationToken => CancellationTokenSource?.Token ?? CancellationToken.None;
    public event Action OnLoaded;
    public event Action<bool> OnSettled;
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

    public PlayerManager PlayerManager { get; private set; }
    public DamageManager DamageManager { get; private set; }
    #endregion

    #region UI
    public UIBattle UIBattle { get; private set; }
    #endregion

    #region Static
    public static Battle Instance { get; private set; }
    #endregion

    public static bool Enter(BattleData battleData, Action onLoaded = null, Action<bool> onSettled = null) {
      if (Instance != null) {
        Debug.LogError("上一场战斗未结束!");
        return false;
      }
      // 战斗实例初始化
      Instance = new Battle(battleData);
      // 首先是加载资源
      Instance.BattleState = BattleState.LOAD;
      Instance.OnLoaded += onLoaded;
      Instance.OnSettled += onSettled;
      Instance.Update();
      return true;
    }

    private Battle(BattleData battleData) {
      CancellationTokenSource = new CancellationTokenSource();

      BattleData = battleData;

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
    }

    public async void Update() {
      while (BattleState != BattleState.NONE) {
        try {
          switch (BattleState) {
            case BattleState.LOAD:
              await Load();
              break;
            case BattleState.RUN:
              await Run();
              break;
            case BattleState.EXIT:
              await Exit();
              break;
          }
        } catch (OperationCanceledException) { }
      }
    }

    private async UniTask Load() {
      // step1:加载关卡和单位的相关资源
      LevelTemplate = await BattleResPreload.Preload(BattleData.Level);
      foreach (var behaviorId in LevelTemplate.Behaviors) {
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
      SelfPlayer.OnStartTurn += OnSelfStartTurn;
      foreach (var playerData in LevelTemplate.PlayerData) {
        PlayerManager.Create(playerData);
      }

      // step3:初始化关卡和单位的行为树
      foreach (var behavior in LevelTemplate.Behaviors) {
        await BehaviorManager.Add(behavior?.Asset as BehaviorGraph);
      }
      foreach (var unit in UnitManager.AllUnits) {
        await unit.InitBehavior();
      }

      // step4:加载战斗UI
      UIBattle = await UIManager.Instance.Open<UIBattle>(UIType.NORMAL, "UIBattle", args: this); // TODO:优化
      // 依赖UIBattle初始化
      var playerList = PlayerManager.PlayerList;
      for (int i = 0; i < playerList.Count; i++) {
        foreach (var unit in playerList[i].Units) {
          await unit.InitUI(i);
        }
      }

      BattleState = BattleState.RUN;

      OnLoaded?.Invoke();
    }

    private async UniTask PreloadUnit(UnitData unitData) {
      var unitTemplate = await BattleResPreload.Preload(unitData.Template);
      await BattleResPreload.Preload(unitTemplate.Prefab);
      await BattleResPreload.Preload(unitTemplate.Attrib);
      foreach (var behavior in unitTemplate.Behaviors) {
        await PreloadBehavior(behavior);
      }
      // 预加载卡牌
      foreach (var cardData in unitData.CardData) {
        var cardTemplate = await BattleResPreload.Preload(cardData.Template);
        await BattleResPreload.Preload(cardTemplate.Icon);
        foreach (var item in cardTemplate.LvCardItems) {
          var skillTemplate = await BattleResPreload.Preload(item.Skill);
          foreach (var skillEvent in skillTemplate.SKillEvents) {
            await PreloadMagic(skillEvent.Magic);
          }
        }
      }
    }

    private async UniTask PreloadBehavior(AssetReferenceT<BehaviorGraph> behaviorRef) {
      var behavior = await BattleResPreload.Preload(behaviorRef);
      if (behavior) {
        foreach (var behaviorNode in behavior.nodes) {
          switch (behaviorNode) {
            case BehaviorFuncs.DoMagic doMagic:
              await PreloadMagic(doMagic.Magic);
              break;
            case BehaviorFuncs.AddBuff addBuff:
              await PreloadBuff(addBuff.Buff);
              break;
          }
        }
      }
    }

    private async UniTask PreloadMagic(AssetReferenceT<MagicFuncBase> magicRef) {
      var magic = await BattleResPreload.Preload(magicRef);
      if (magic) {
        switch (magic) {
          case AddBehavior addBehavior:
            await PreloadBehavior(addBehavior.Behavior);
            break;
          case AddBuff addBuff:
            await PreloadBuff(addBuff.Buff);
            break;
        }
      }
    }

    private async UniTask PreloadBuff(AssetReferenceT<BuffTemplate> buffRef) {
      var buff = await BattleResPreload.Preload(buffRef);
      if (buff) {
        await PreloadMagic(buff.Magic);
        await PreloadMagic(buff.IntervalMagic);
        await BattleResPreload.Preload(buff.Icon);
      }
    }

    public async UniTask Settle(bool isWin) {
      foreach (var unit in UnitManager.AllUnits) {
        await unit.OnSettle();
      }
      OnSettled?.Invoke(isWin);
      Cancel();
    }

    public void Cancel() {
      BattleState = BattleState.EXIT;
      CancellationTokenSource.Cancel();
    }

    private async UniTask Exit() {
      Instance = null;
      BattleData = null;
      BattleState = BattleState.NONE;

      UnitManager = null;
      BuffManager = null;
      MagicManager = null;
      BehaviorManager = null;
      AttribManager = null;
      CardManager = null;
      PlayerManager = null;
      DamageManager = null;

      Blackboard.Release();
      Blackboard = null;

      ObjectPool.Release();
      ObjectPool = null;

      CurPlayer = null;
      SelfPlayer = null;
      LevelTemplate = null;

      if (UIBattle) {
        await UIManager.Instance.Close(UIBattle);
        UIBattle = null;
      }

      CancellationTokenSource.Dispose();
      CancellationTokenSource = null;

      GC.Collect();
    }

    private async UniTask Run() {
      // 更新当前回合的玩家
      CurPlayer = PlayerManager.MoveNext();
      // 回合中的逻辑
      await CurPlayer.OnTurn();
    }

    private void OnSelfStartTurn() => Turn++;
  }
}