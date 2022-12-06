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
    [LabelText("名称")]
    public string Name;
    [LabelText("耗能")]
    public int Cost;
    [LabelText("是否消耗")]
    public bool Consumable;
    [LabelText("是否保留")]
    public bool Retainable;
    [TextArea]
    [LabelText("描述")]
    public string Desc;
    [DrawWithUnity]
    public AssetReferenceT<SkillTemplate> Skill;
    public CardPrePlayer CardPlayer;
  }

  [CreateAssetMenu(menuName = "模板/卡牌")]
  public class CardTemplate : SerializedScriptableObject {
    [LabelText("类型")]
    public CardType CardType;
    [LabelText("目标相对阵营")]
    public PlayerCamp TargetCamp;
    [LabelText("图片")]
    public AssetReferenceT<Sprite> Icon;
    [LabelText("不同等级的配置列表")]
    public LvCardItem[] LvCardItems;
  }
}