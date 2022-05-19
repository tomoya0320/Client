using UnityEngine;

namespace Battle {
  public class DamageManager : BattleBase {
    public DamageManager(BattleManager battleManager) : base(battleManager) {

    }

    public bool Damage(Unit source, Unit target, float damageValue) {
      // Test
      Debug.Log($"{source.RuntimeId}:{source.Name}��{target.RuntimeId}:{target.Name}���{damageValue}���˺�");
      return true;
    }
  }
}