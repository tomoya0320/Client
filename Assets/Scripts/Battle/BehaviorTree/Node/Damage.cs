using Battle;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace BehaviorTree.Battle {
	[CreateNodeMenu("节点/行为/伤害")]
	public class Damage : ActionNode {
    public int DamgeValue;

    public override async UniTask<bool> Run(BattleManager battleManager, Context context) {
      // Test
      await UniTask.Delay(1000);
      Debug.Log($"造成伤害:{DamgeValue}");

      return true;
    }
  }
}