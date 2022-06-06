using Cysharp.Threading.Tasks;
using System.Collections.Generic;
using UnityEngine;

namespace GameCore {
  public class BuffComponent : IPoolObject {
    public Unit Unit { get; private set; }
    private Battle Battle => Unit.Battle;
    private BuffManager BuffManager => Battle.BuffManager;
    private Dictionary<int, Buff> Buffs = new Dictionary<int, Buff>();

    public BuffComponent Init(Unit unit) {
      Unit = unit;
      return this;
    }

    public void Release() {
      foreach (var buff in Buffs.Values) {
        Battle.ObjectPool.Release(buff);
      }
      Buffs.Clear();

      Unit = null;
    }

    public async UniTask Update(BehaviorTime updateTime) {
      var buffList = TempList<Buff>.Get();
      buffList.AddRange(Buffs.Values);
      foreach (var buff in buffList) {
        if (!buff.Update(updateTime)) {
          await Remove(buff.RuntimeId);
        }
      }
      TempList<Buff>.CleanUp();
    }

    public async UniTask<Buff> Add(Unit source, string buffId, int runtimeId) {
      if (!BuffManager.Templates.TryGetValue(buffId, out var buffTemplate)) {
        Debug.LogError($"BuffTemplate is null. id:{buffId}");
        return null;
      }

      var buff = Battle.ObjectPool.Get<Buff>();
      buff.Init(runtimeId, this, buffTemplate, source, Unit);
      Buffs.Add(buff.RuntimeId, buff);

      await Battle.MagicManager.DoMagic(buff.MagicId, buff.Source, buff.Target, buff.BuffContext);

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