using UnityEngine;

namespace GameCore {
  public class DamageManager : BattleBase {
    public DamageManager(Battle battle) : base(battle) {

    }

    public bool Damage(Unit source, Unit target, float damageValue) {
      // Test
      Debug.Log($"{source.RuntimeId}:{source.Name}对{target.RuntimeId}:{target.Name}造成{damageValue}点伤害");
      target.AddAttrib(AttribType.HP, -(int)damageValue);
      return true;
    }
  }
}