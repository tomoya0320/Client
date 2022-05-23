using UnityEngine;

namespace GameCore {
  public class DamageManager : BattleBase {
    public DamageManager(Battle battle) : base(battle) {

    }

    public bool Damage(Unit source, Unit target, float damageValue) {
      // Test
      Debug.Log($"{source.RuntimeId}:{source.Name}��{target.RuntimeId}:{target.Name}���{damageValue}���˺�");
      target.AddAttrib(AttribType.HP, -(int)damageValue);
      return true;
    }
  }
}