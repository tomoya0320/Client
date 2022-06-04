using Cysharp.Threading.Tasks;
using UnityEngine;

namespace GameCore {
  public class DamageManager : BattleBase {
    public DamageManager(Battle battle) : base(battle) {

    }

    public async UniTask Damage(Unit source, Unit target, int damageValue) {
      int realDamageValue = target.AddAttrib(AttribType.HP, -damageValue);
      // Test
      Debug.Log($"{source.RuntimeId}:{source.Name} �� {target.RuntimeId}:{target.Name} ���{-realDamageValue}���˺�");
      if (target.GetAttrib(AttribType.HP).Value <= 0) {
        await target.TryDie(source, realDamageValue);
      }
    }
  }
}