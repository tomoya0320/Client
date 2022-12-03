using Cysharp.Threading.Tasks;
using System.Collections.Generic;
using UnityEngine;

namespace GameCore {
  public class BuffComponent {
    public Unit Unit { get; private set; }
    private Battle Battle => Unit.Battle;
    private BuffManager BuffManager => Battle.BuffManager;
    private Dictionary<int, Buff> Buffs = new Dictionary<int, Buff>();
    public Dictionary<int, Buff>.ValueCollection AllBuffs => Buffs.Values;

    public BuffComponent(Unit unit) => Unit = unit;

    public async UniTask Update(TickTime updateTime) {
      var buffList = TempList<Buff>.Get();
      buffList.AddRange(Buffs.Values);
      foreach (var buff in buffList) {
        if (!await buff.Update(updateTime)) {
          await Remove(buff.RuntimeId);
        }
      }
      TempList<Buff>.Release(buffList);
    }

    public async UniTask<Buff> Add(Unit source, BuffTemplate buffTemplate, int runtimeId) {
      if (!buffTemplate) {
        Debug.LogError($"BuffTemplate is null!");
        return null;
      }

      // 检查免疫类型
      string buffKind = buffTemplate.BuffKind;
      if (!string.IsNullOrEmpty(buffKind)) {
        foreach (var checkBuff in Buffs.Values) {
          var checkBuffImmuneKinds = checkBuff.BuffTemplate.ImmuneKinds;
          if (checkBuffImmuneKinds != null && checkBuffImmuneKinds.Contains(buffKind)) {
            return null;
          }
        }
      }
      var immuneKinds = buffTemplate.ImmuneKinds;
      if (immuneKinds != null && immuneKinds.Count > 0) {
        var tempBuffList = TempList<Buff>.Get();
        tempBuffList.AddRange(Buffs.Values);
        foreach (var checkBuff in tempBuffList) {
          string checkBuffKind = checkBuff.BuffTemplate.BuffKind;
          if (!string.IsNullOrEmpty(checkBuffKind) && immuneKinds.Contains(checkBuffKind)) {
            await Remove(checkBuff.RuntimeId);
          }
        }
        TempList<Buff>.Release(tempBuffList);
      }

      var buff = Battle.ObjectPool.Get<Buff>();
      buff.Init(runtimeId, this, buffTemplate, source, Unit);
      Buffs.Add(runtimeId, buff);

      if (buffTemplate.Delay <= 0) {
        await Battle.MagicManager.DoMagic(buff.Magic, buff.Source, buff.Target, buff.BuffContext);
        if (buffTemplate.Duration == 0) {
          await Remove(runtimeId);
        }
      }

      return Buffs.ContainsKey(runtimeId) ? buff : null;
    }

    public async UniTask<bool> Remove(int runtimeId) {
      if(!Buffs.TryGetValue(runtimeId, out var buff)) {
        Debug.LogError($"Buff is not exists. runtimeId:{runtimeId}");
        return false;
      }

      await Battle.MagicManager.DoMagic(buff.Magic, buff.Source, buff.Target, buff.BuffContext, true);
      Buffs.Remove(runtimeId);
      Battle.ObjectPool.Release(buff);

      return true;
    }
  }
}