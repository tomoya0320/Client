using Cysharp.Threading.Tasks;
using UnityEngine;

namespace GameCore {
  public class DamageManager : BattleBase {
    public DamageManager(Battle battle) : base(battle) { }

    public async UniTask Damage(Unit source, Unit target, int damageValue) {
      DamageContext damageContext = Battle.ObjectPool.Get<DamageContext>();
      damageContext.Source = source;
      damageContext.Target = target;
      damageContext.DamageValue = damageValue;

      await Battle.BehaviorManager.RunRoot(TickTime.ON_BEFORE_DAMAGE, source, damageContext);
      await Battle.BehaviorManager.RunRoot(TickTime.ON_BEFORE_DAMAGED, target, damageContext);

      damageContext.DamageValue = Mathf.Max(damageContext.DamageValue, 0);

      int realDamageValue = target.AddAttrib(AttribType.HP, -damageContext.DamageValue);
      Battle.UIBattle.ShowText($"{realDamageValue}", target.UIUnit.NumNode.position, Color.red, true);

      await Battle.BehaviorManager.RunRoot(TickTime.ON_LATE_DAMAGE, source, damageContext);
      await Battle.BehaviorManager.RunRoot(TickTime.ON_LATE_DAMAGED, target, damageContext);

      if (target.Attribs[(int)AttribType.HP].Value <= 0) {
        await target.TryDie(damageContext);
      }

      Battle.ObjectPool.Release(damageContext);
    }
  }
}