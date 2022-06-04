using Cysharp.Threading.Tasks;
using System.Collections.Generic;
using UnityEngine;

namespace GameCore {
  public class BuffManager : TemplateManager<BuffTemplate> {
    private Dictionary<int, BuffComponent> BuffComponents = new Dictionary<int, BuffComponent>();

    public BuffManager(Battle battle) : base(battle) { }

    public async UniTask Update(BattleTurnPhase phase, params Unit[] units) {
      var list = TempList<BuffComponent>.Get();
      foreach (var unit in units) {
        list.Add(BuffComponents[unit.RuntimeId]);
      }

      foreach (var buffComponent in list) {
        await buffComponent.Update(phase);
      }
      TempList<BuffComponent>.CleanUp();
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
      var buff = await buffComponent.Add(source, buffId);
      return buff;
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