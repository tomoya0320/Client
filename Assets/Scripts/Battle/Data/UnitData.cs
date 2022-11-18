using Sirenix.OdinInspector;
using System;
using UnityEngine.AddressableAssets;

namespace GameCore {
  [Serializable]
  public class UnitData {
    public AssetReferenceT<UnitTemplate> Template;
    [LabelText("等级")]
    public int Lv;
    [LabelText("卡牌数据列表")]
    public CardData[] CardData;
  }
}