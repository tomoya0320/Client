using UnityEngine;
using Sirenix.OdinInspector;
using UnityEngine.AddressableAssets;
using System.ComponentModel;

namespace GameCore {
  public enum CardType {
    [Description("攻击")]
    [InspectorName("攻击")]
    ATTACK,
    [Description("技能")]
    [InspectorName("技能")]
    SKILL,
  }

  public struct LvCardItem {
    public string Name;
    public int Cost;
    public bool Consumable;
    [TextArea]
    public string Desc;
    [DrawWithUnity]
    public AssetReferenceT<SkillTemplate> Skill;
    public CardPrePlayer CardPlayer;
  }

  [CreateAssetMenu(menuName = "模板/卡牌")]
  public class CardTemplate : SerializedScriptableObject {
    [LabelText("类型")]
    public CardType CardType;
    [LabelText("图片")]
    public AssetReferenceT<Sprite> Icon;
    [LabelText("目标相对阵营")]
    public PlayerCamp TargetCamp;
    [LabelText("不同等级的配置列表")]
    public LvCardItem[] LvCardItems;
  }
}