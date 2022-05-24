using System;
using UnityEngine;
using Sirenix.OdinInspector;

namespace GameCore {
  public enum CardType {
    [InspectorName("攻击")]
    ATTACK,
    [InspectorName("技能")]
    SKILL,
  }

  [Serializable]
  public struct LvCardItem {
    public string Name;
    public int Cost;
    public string SkillTemplateId;
  }

  [CreateAssetMenu(menuName = "模板/卡牌")]
  public class CardTemplate : ScriptableObject {
    [LabelText("类型")]
    public CardType CardType;
    [LabelText("不同等级的配置列表")]
    public LvCardItem[] LvCardItems;
  }
}