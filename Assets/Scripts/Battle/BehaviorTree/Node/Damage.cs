﻿using Battle;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace BehaviorTree.Battle {
  [CreateNodeMenu("节点/行为/伤害")]
  public class Damage : ActionNode {
    public NodeParam DamageValue;
    public NodeParamKey TargetUnit;

    public override async UniTask<bool> Run(Behavior behavior, Context context) {
      // Test
      await UniTask.Delay(1000);
      Unit targetUnit = behavior.GetUnit(TargetUnit);
      if (targetUnit == null) {
        return false;
      }
      Debug.Log($"对{targetUnit}造成伤害:{behavior.GetFloat(DamageValue)}");

      return true;
    }
  }
}