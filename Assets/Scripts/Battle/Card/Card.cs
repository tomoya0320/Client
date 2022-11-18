using Cysharp.Threading.Tasks;

namespace GameCore {
  public class Card : BattleBase {
    public CardTemplate CardTemplate;
    public int RuntimeId { get; private set; }
    public PlayerCamp TargetCamp => CardTemplate.TargetCamp;
    public CardData CardData { get; private set; }
    public string TemplateId => CardData.TemplateId;
    public int Lv => CardData.Lv;
    public Unit Owner { get; private set; }
    public Skill[] Skills { get; private set; }
    private Skill Skill => Skills[Lv];
    public int Cost => CardTemplate.LvCardItems[Lv].Cost;
    public bool Consumable => CardTemplate.LvCardItems[Lv].Consumable;
    private CardPrePlayer CardPlayer => CardTemplate.LvCardItems[Lv].CardPlayer;
    public CardHeapType CardHeapType = CardHeapType.DRAW;

    public Card(Battle battle, int runtimeId, Unit owner, CardData cardData) : base(battle) {
      RuntimeId = runtimeId;
      Owner = owner;
      CardData = cardData;

      Battle.CardManager.TryGetTemplate(TemplateId, out CardTemplate);

      Skills = new Skill[CardTemplate.LvCardItems.Length];
      for (int i = 0; i < Skills.Length; i++) {
        Skills[i] = new Skill(Battle, Owner, CardTemplate.LvCardItems[i].SkillId);
      }
    }

    public bool CheckTargetCamp(Unit mainTarget) {
      switch (TargetCamp) {
        case PlayerCamp.NONE:
          return false;
        case PlayerCamp.ALL:
          return true;
        case PlayerCamp.ALLY:
          return Owner.PlayerCamp == mainTarget.PlayerCamp;
        case PlayerCamp.ENEMY:
          return Owner.PlayerCamp != mainTarget.PlayerCamp;
        default:
          return false;
      }
    }

    public async UniTask Cast(Unit mainTarget) => await Skill.Cast(mainTarget);

    public bool PrePlay(Unit mainTarget) => CardPlayer.PrePlay(this, mainTarget);

    public override string ToString() {
      return $"{RuntimeId}:{CardTemplate.LvCardItems[Lv].Name}\nLv:{Lv}\nCost:{CardTemplate.LvCardItems[Lv].Cost}"; // Test
    }
  }
}