using Sirenix.OdinInspector;
using System;
using UnityEngine;

namespace GameCore {
  public enum AttribType {
    [InspectorName("生命")]
    HP,
    [InspectorName("攻击")]
    ATK,
    [InspectorName("能量")]
    ENERGY,
  }

  [Serializable]
  public struct AttribAdditive {
    [LabelText("属性类型")]
    public AttribType Type;
    [LabelText("基于属性值的比率")]
    public int RateOnValue;
    [LabelText("基于属性最大值的比率")]
    public int RateOnMaxValue;

    public int GetValue(Unit unit) {
      Attrib attrib = unit.GetAttrib(Type);
      return (attrib.Value * RateOnValue + attrib.MaxValue * RateOnValue) / BattleConstant.THOUSAND;
    }
  }

  public struct Attrib {
    public int Value;
    public int MaxValue;
    public bool AllowNegative;
    public bool AllowExceedMax;

    /// <summary>
    /// 添加值
    /// </summary>
    /// <param name="value">可正可负</param>
    /// <returns>返回实际添加的值</returns>
    public int AddValue(int value) {
      int origin = Value;
      Value += value;
      if (Value < 0 && !AllowNegative) {
        Value = 0;
      }
      if (Value > MaxValue && !AllowExceedMax) {
        Value = MaxValue;
      }
      return Value - origin;
    }

    /// <summary>
    /// 添加最大值
    /// </summary>
    /// <param name="maxValue">可正可负</param>
    /// <returns>返回实际添加的最大值</returns>
    public int AddMaxValue(int maxValue) {
      int origin = MaxValue;
      // 最大值不能为负
      MaxValue = Mathf.Max(0, MaxValue + maxValue);
      if (MaxValue > origin) {
        AddValue(MaxValue - origin);
      } else if (MaxValue < Value) {
        AddValue(MaxValue - Value);
      }
      return MaxValue - origin;
    }
  }
}