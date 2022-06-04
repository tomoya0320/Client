using Cysharp.Threading.Tasks;
using System.Collections.Generic;
using UnityEngine;

namespace GameCore {
  public class BuffComponent : IPoolObject {
    private int IncId;
    public Unit Unit { get; private set; }
    private Battle Battle => Unit.Battle;
    private BuffManager BuffManager => Battle.BuffManager;
    private Dictionary<int, Buff> Buffs = new Dictionary<int, Buff>();

    public BuffComponent Init(Unit unit) {
      Unit = unit;
      return this;
    }

    public void Release() {
      IncId = 0;
      Unit = null;
      foreach (var buff in Buffs.Values) {
        Battle.ObjectPool.Release(buff);
      }
      Buffs.Clear();
    }

    public async UniTask Update(BattleTurnPhase phase) {
      var list = TempList<Buff>.Get();
      list.AddRange(Buffs.Values);
      foreach (var buff in list) {
        if (!buff.UpdateTurn()) {
          await Remove(buff.RuntimeId);
        }
      }
      TempList<Buff>.CleanUp();
    }

    public async UniTask<Buff> Add(Unit source, string buffId) {
      if (!BuffManager.Templates.TryGetValue(buffId, out var buffTemplate)) {
        Debug.LogError($"BuffTemplate is null. id:{buffId}");
        return null;
      }

      var buff = Battle.ObjectPool.Get<Buff>();
      buff.Init(++IncId, this, buffTemplate, source, Unit);
      await Battle.MagicManager.DoMagic(buff.MagicId, buff.Source, buff.Target, buff.BuffContext);
      Buffs.Add(buff.RuntimeId, buff);

      if(buffTemplate.Duration <= 0) {
        await Remove(buff.RuntimeId);
        return null;
      }

      return buff;
    }

    public async UniTask<bool> Remove(int runtimeId) {
      if(!Buffs.TryGetValue(runtimeId, out var buff)) {
        Debug.LogError($"Buff is not exists. runtimeId:{runtimeId}");
        return false;
      }

      await Battle.MagicManager.DoMagic(buff.MagicId, buff.Source, buff.Target, buff.BuffContext, true);
      Buffs.Remove(runtimeId);
      Battle.ObjectPool.Release(buff);

      return true;
    }
  }
}