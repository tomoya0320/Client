using Cysharp.Threading.Tasks;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Battle {
  public class BuffManager : BattleBase {
    private Dictionary<string, BuffTemplate> BuffTemplates = new Dictionary<string, BuffTemplate>();
    private Dictionary<int, BuffComponent> BuffComponents = new Dictionary<int, BuffComponent>();

    public BuffManager(BattleManager battleManager) : base(battleManager) {

    }

    public async UniTask PreloadBuff(string buffId) {
      if (BuffTemplates.ContainsKey(buffId)) {
        return;
      }
      BuffTemplate buffData = await Addressables.LoadAssetAsync<BuffTemplate>(buffId);
      BuffTemplates.Add(buffId, buffData);
    }

    public bool TryGetBuffTemplate(string buffId, out BuffTemplate buffTemplate) {
      return BuffTemplates.TryGetValue(buffId, out buffTemplate);
    }

    public void AddComponent(Unit unit) {
      if (BuffComponents.ContainsKey(unit.RuntimeId)) {
        Debug.LogWarning($"BuffComponent repeat add. id:{unit.RuntimeId}");
        return;
      }
      BuffComponents.Add(unit.RuntimeId, new BuffComponent(BattleManager, unit));
    }

    public void RemoveComponent(int runtimeId) {
      if (!BuffComponents.TryGetValue(runtimeId, out var buffComponent)) {
        Debug.LogError($"BuffComponent is not exist. id:{runtimeId}");
        return;
      }
      buffComponent.CleanUp();
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