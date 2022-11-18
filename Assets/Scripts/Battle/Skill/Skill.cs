using Cysharp.Threading.Tasks;

namespace GameCore {
  public class Skill : BattleBase {
    public SkillTemplate SkillTemplate;
    public Unit Owner { get; private set; }

    public Skill(Battle battle, Unit owner, string skillId) : base(battle) { 
      Owner = owner;
      Battle.SkillManager.TryGetTemplate(skillId, out SkillTemplate);
    }

    public async UniTask Cast(Unit mainTarget) {
      foreach (var skillEvent in SkillTemplate.SKillEvents) {
        await UniTask.Delay((int)(skillEvent.WaitTime * BattleConstant.THOUSAND));
        var magicId = skillEvent.Magic?.AssetGUID;
        var targets = TempList<Unit>.Get();
        skillEvent.TargetSelector.Select(Battle, Owner, mainTarget, targets);
        foreach (var target in targets) {
          await Battle.MagicManager.DoMagic(magicId, Owner, target);
        }
        TempList<Unit>.Release(targets);
      }
    }
  }
}