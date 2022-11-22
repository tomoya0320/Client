using Cysharp.Threading.Tasks;
using UnityEngine;

namespace GameCore {
  public class DamageManager : BattleBase {
    public DamageManager(Battle battle) : base(battle) { }

    public async UniTask Damage(Unit source, Unit target, int damageValue) {
      int realDamageValue = target.AddAttrib(AttribType.HP, -damageValue);
      Battle.UIBattle.ShowText($"{realDamageValue}", target.UIUnit.NumNode.position, Color.red, true);
      if (target.Attribs[(int)AttribType.HP].Value <= 0) {
        await target.TryDie(source, realDamageValue);
      }
    }
  }
}