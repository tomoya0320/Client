using Cysharp.Threading.Tasks;
using UnityEngine;

namespace GameCore {
  public class Skill : BattleBase {
    public SkillTemplate SkillTemplate;
    public Unit Owner { get; private set; }

    public Skill(Battle battle, Unit owner, string skillId) : base(battle) {
      Owner = owner;
      Battle.SkillManager.TryGetAsset(skillId, out SkillTemplate);
    }

    public async UniTask Cast(Unit mainTarget) {
      float startTime = Time.realtimeSinceStartup;
      Owner.UIUnit.PlayAnimation(SkillTemplate.Anim);
      foreach (var skillEvent in SkillTemplate.SKillEvents) {
        await UniTask.Delay((int)(skillEvent.WaitTime * BattleConstant.THOUSAND), cancellationToken: Battle.CancellationToken);
        var magicId = skillEvent.Magic?.AssetGUID;
        var targets = TempList<Unit>.Get();
        skillEvent.TargetSelector.Select(Battle, Owner, mainTarget, targets);
        foreach (var target in targets) {
          await Battle.MagicManager.DoMagic(magicId, Owner, target);
        }
        TempList<Unit>.Release(targets);
      }
      float leftTime = SkillTemplate.AnimTime - (Time.realtimeSinceStartup - startTime);
      if (leftTime > 0) {
        await UniTask.Delay((int)(leftTime * BattleConstant.THOUSAND), cancellationToken: Battle.CancellationToken);
      }
    }
  }
}