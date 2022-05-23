using Sirenix.OdinInspector;
using System;

namespace GameCore {
  [Serializable]
  public struct UnitData {
    public string TemplateId;
    public int Lv;
    [LabelText("卡牌数据列表")]
    public CardData[] CardData;
  }
}