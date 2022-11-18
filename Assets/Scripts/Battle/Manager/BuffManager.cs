using Cysharp.Threading.Tasks;
using System.Collections.Generic;
using UnityEngine;

namespace GameCore {
  public class BuffManager : AssetManager<BuffTemplate> {
    private int IncId;
    private Dictionary<int, BuffComponent> BuffComponents = new Dictionary<int, BuffComponent>();

    public BuffManager(Battle battle) : base(battle) { }

    public BuffComponent this[int id] {
      get {
        BuffComponents.TryGetValue(id, out var buffComponent);
        return buffComponent;
      }
    }

    public async UniTask Update(TickTime updateTime, Unit unit) {
      var buffComponentList = TempList<BuffComponent>.Get();
      if (unit == null) {
        buffComponentList.AddRange(BuffComponents.Values);
      } else if (BuffComponents.TryGetValue(unit.RuntimeId, out var buffComponent)) {
        buffComponentList.Add(buffComponent);
      }
      foreach (var buffComponent in buffComponentList) {
        await buffComponent.Update(updateTime);
      }
      TempList<BuffComponent>.Release(buffComponentList);
    }

    public void AddComponent(Unit unit) {
      if (BuffComponents.ContainsKey(unit.RuntimeId)) {
        Debug.LogError($"BuffComponent repeat add. id:{unit.RuntimeId}");
        return;
      }
      var buffComponent = Battle.ObjectPool.Get<BuffComponent>();
      BuffComponents.Add(unit.RuntimeId, buffComponent.Init(unit));
    }

    public void RemoveComponent(int runtimeId) {
      if (!BuffComponents.TryGetValue(runtimeId, out var buffComponent)) {
        Debug.LogError($"BuffComponent is not exist. id:{runtimeId}");
        return;
      }
      Battle.ObjectPool.Release(buffComponent);
      BuffComponents.Remove(runtimeId);
    }

    public async UniTask<Buff> AddBuff(string buffId, Unit source, Unit target) {
      if(!BuffComponents.TryGetValue(target.RuntimeId, out var buffComponent)) {
        Debug.LogError($"BuffComponent is not exist. id:{target.RuntimeId}");
        return null;
      }
      return await buffComponent.Add(source, buffId, ++IncId);
    }

    public async UniTask<bool> RemoveBuff(Unit unit, int runtimeId) {
      if (!BuffComponents.TryGetValue(unit.RuntimeId, out var buffComponent)) {
        Debug.LogError($"BuffComponent is not exist. id:{unit.RuntimeId}");
        return false;
      }

      bool result = await buffComponent.Remove(runtimeId);
      return result;
    }
  }
}