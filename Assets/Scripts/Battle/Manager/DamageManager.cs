using UnityEngine;

namespace Battle {
  public class DamageManager : BattleBase {
    public DamageManager(BattleManager battleManager) : base(battleManager) {

    }

    public bool Damage(Unit source, Unit target, float damageValue) {
      // Test
      Debug.Log($"{source.RuntimeId}:{source.Name}对{target.RuntimeId}:{target.Name}造成{damageValue}点伤害");
      return true;
    }
  }
}