using Cysharp.Threading.Tasks;
using UnityEngine;

namespace GameCore {
  public class DamageManager : BattleBase {
    public DamageManager(Battle battle) : base(battle) { }

    public async UniTask Damage(Unit source, Unit target, int damageValue) {
      int realDamageValue = target.AddAttrib(AttribType.HP, -damageValue);
      // Test
      Debug.Log($"{source.RuntimeId}:{source.Name} 对 {target.RuntimeId}:{target.Name} 造成{-realDamageValue}点伤害");
      if (target.Attribs[(int)AttribType.HP].Value <= 0) {
        await target.TryDie(source, realDamageValue);
      }
    }
  }
}