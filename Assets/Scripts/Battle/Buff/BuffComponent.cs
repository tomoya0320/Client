using System.Collections.Generic;
using UnityEngine;

namespace Battle {
  public class BuffComponent : BattleBase {
    private int IncId;
    public Unit Unit { get; private set; }
    private BuffManager BuffManager => BattleManager.BuffManager;
    private Dictionary<int, Buff> Buffs = new Dictionary<int, Buff>();

    public BuffComponent(BattleManager battleManager, Unit unit) : base(battleManager) {
      Unit = unit;
    }

    public void CleanUp() {
      Unit = null;
    }

    public int Add(Unit source, string buffId) {
      var buffData = BuffManager.GetBuffData(buffId);
      if (buffData == null) {
        Debug.LogError($"BuffData is null. id:{buffId}");
        return 0;
      }

      int runtimeId = ++IncId;
      var buff = BattleManager.ObjectPool.Get<Buff>();
      if(buff.Init(runtimeId, this, buffData, source, Unit)) {
        Buffs.Add(runtimeId, buff);
      } else {
        BattleManager.ObjectPool.Release(buff);
      }

      return runtimeId;
    }
  }
}