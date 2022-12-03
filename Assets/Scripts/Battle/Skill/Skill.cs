using Cysharp.Threading.Tasks;
using GameCore.MagicFuncs;
using UnityEngine;

namespace GameCore {
  public class Skill : BattleBase {
    public SkillTemplate SkillTemplate { get; private set; }
    public Unit Owner { get; private set; }

    public Skill(Battle battle, Unit owner, SkillTemplate skillTemplate) : base(battle) {
      Owner = owner;
      SkillTemplate = skillTemplate;
    }

    public async UniTask Cast(Unit mainTarget) {
      float startTime = Time.realtimeSinceStartup;
      Owner.UIUnit.PlayAnimation(SkillTemplate.Anim);
      foreach (var skillEvent in SkillTemplate.SKillEvents) {
        await UniTask.Delay((int)(skillEvent.WaitTime * GameConstant.THOUSAND), cancellationToken: Battle.CancellationToken);
        var magic = skillEvent.Magic?.Asset as MagicFuncBase;
        var targets = TempList<Unit>.Get();
        skillEvent.TargetSelector.Select(Battle, Owner, mainTarget, targets);
        foreach (var target in targets) {
          await Battle.MagicManager.DoMagic(magic, Owner, target);
        }
        TempList<Unit>.Release(targets);
      }
      float leftTime = SkillTemplate.AnimTime - (Time.realtimeSinceStartup - startTime);
      if (leftTime > 0) {
        await UniTask.Delay((int)(leftTime * GameConstant.THOUSAND), cancellationToken: Battle.CancellationToken);
      }
    }
  }
}