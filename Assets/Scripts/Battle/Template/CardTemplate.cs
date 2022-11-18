using UnityEngine;
using Sirenix.OdinInspector;

namespace GameCore {
  public enum CardType {
    [InspectorName("攻击")]
    ATTACK,
    [InspectorName("技能")]
    SKILL,
  }

  public struct LvCardItem {
    public string Name;
    public int Cost;
    public bool Consumable;
    public string SkillId;
    public CardPrePlayer CardPlayer;
  }

  [CreateAssetMenu(menuName = "模板/卡牌")]
  public class CardTemplate : SerializedScriptableObject {
    [LabelText("类型")]
    public CardType CardType;
    [LabelText("目标相对阵营")]
    public PlayerCamp TargetCamp;
    [LabelText("不同等级的配置列表")]
    public LvCardItem[] LvCardItems;
  }
}