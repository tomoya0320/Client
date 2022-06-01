using System.Collections.Generic;
using UnityEngine;

namespace GameCore {
  public class BuffManager : TemplateManager<BuffTemplate> {
    private Dictionary<int, BuffComponent> BuffComponents = new Dictionary<int, BuffComponent>();

    public BuffManager(Battle battle) : base(battle) {

    }

    public void Update(BattleTurnPhase phase, params Unit[] units) {
      var list = TempList<BuffComponent>.Get();
      foreach (var unit in units) {
        list.Add(BuffComponents[unit.RuntimeId]);
      }

      foreach (var buffComponent in list) {
        buffComponent.Update(phase);
      }
      TempList<BuffComponent>.CleanUp();
    }

    public void AddComponent(Unit unit) {
      if (BuffComponents.ContainsKey(unit.RuntimeId)) {
        Debug.LogWarning($"BuffComponent repeat add. id:{unit.RuntimeId}");
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

    public int AddBuff(Unit source, Unit target, string buffId) {
      if(!BuffComponents.TryGetValue(target.RuntimeId, out var buffComponent)) {
        Debug.LogError($"BuffComponent is not exist. id:{target.RuntimeId}");
        return 0;
      }
      return buffComponent.Add(source, buffId);
    }
  }
}