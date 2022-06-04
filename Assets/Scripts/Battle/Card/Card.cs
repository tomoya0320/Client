using Cysharp.Threading.Tasks;

namespace GameCore {
  public class Card : BattleBase {
    public CardTemplate CardTemplate;
    public int RuntimeId { get; private set; }
    public int Lv { get; private set; }
    public Unit Owner { get; private set; }
    public Skill[] Skills { get; private set; }
    private Skill Skill => Skills[Lv];
    public int Cost => CardTemplate.LvCardItems[Lv].Cost;
    private CardPlayer CardPlayer => CardTemplate.LvCardItems[Lv].CardPlayer;
    public CardHeapType CardHeapType = CardHeapType.DRAW;

    public Card(Battle battle, int runtimeId, Unit owner, CardData cardData) : base(battle) {
      RuntimeId = runtimeId;
      Lv = cardData.Lv;
      Owner = owner;
      Battle.CardManager.Templates.TryGetValue(cardData.TemplateId, out CardTemplate);
      Skills = new Skill[CardTemplate.LvCardItems.Length];
      for (int i = 0; i < Skills.Length; i++) {
        Skills[i] = new Skill(Battle, Owner, CardTemplate.LvCardItems[i].SkillId);
      }
    }

    public async UniTask Cast(Unit mainTarget) => await Skill.Cast(mainTarget);

    public bool TryPlay(Unit mainTarget) => CardPlayer.TryPlay(this, mainTarget);
  }
}