using System.Collections.Generic;
using UnityEngine;

namespace GameCore {
  public class BuffComponent : IPoolObject {
    private int IncId;
    public Unit Unit { get; private set; }
    private Battle BattleManager => Unit.Battle;
    private BuffManager BuffManager => BattleManager.BuffManager;
    private Dictionary<int, Buff> Buffs = new Dictionary<int, Buff>();

    public BuffComponent Init(Unit unit) {
      Unit = unit;
      return this;
    }

    public void Release() {
      IncId = 0;
      Unit = null;
      foreach (var buff in Buffs.Values) {
        BattleManager.ObjectPool.Release(buff);
      }
      Buffs.Clear();
    }

    public void Update(BattleTurnPhase phase) {
      var list = TempList<Buff>.Get();
      list.AddRange(Buffs.Values);
      foreach (var buff in list) {
        if (!buff.UpdateTurn()) {
          Remove(buff.RuntimeId);
        }
      }
      TempList<Buff>.CleanUp();
    }

    public int Add(Unit source, string buffId) {
      if (!BuffManager.Templates.TryGetValue(buffId, out var buffTemplate)) {
        Debug.LogError($"BuffTemplate is null. id:{buffId}");
        return 0;
      }

      int runtimeId = ++IncId;
      var buff = BattleManager.ObjectPool.Get<Buff>();
      if(buff.Init(runtimeId, this, buffTemplate, source, Unit)) {
        Buffs.Add(runtimeId, buff);
      } else {
        BattleManager.ObjectPool.Release(buff);
      }

      return runtimeId;
    }

    public bool Remove(int runtimeId) {
      if(!Buffs.TryGetValue(runtimeId, out var buff)) {
        Debug.LogError($"Buff is not exists. runtimeId:{runtimeId}");
        return false;
      }
      Buffs.Remove(runtimeId);
      BattleManager.ObjectPool.Release(buff);
      return true;
    }
  }
}