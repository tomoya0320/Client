using Sirenix.OdinInspector;
using System;
using UnityEngine;

namespace Battle {
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

  [Serializable]
  public struct Attrib {
    [SerializeField]
    private int _Value;
    public int Value => _Value;
    [SerializeField]
    private int _MaxValue;
    public int MaxValue => _MaxValue;
    [NonSerialized]
    public bool AllowNegative;
    [NonSerialized]
    public bool AllowExceedMax;

    /// <summary>
    /// 添加值
    /// </summary>
    /// <param name="value">可正可负</param>
    /// <returns>返回实际添加的值</returns>
    public int AddValue(int value) {
      int origin = _Value;
      _Value += value;
      if (_Value < 0 && !AllowNegative) {
        _Value = 0;
      }
      if (_Value > _MaxValue && !AllowExceedMax) {
        _Value = _MaxValue;
      }
      return _Value - origin;
    }

    /// <summary>
    /// 添加最大值
    /// </summary>
    /// <param name="maxValue">可正可负</param>
    /// <returns>返回实际添加的最大值</returns>
    public int AddMaxValue(int maxValue) {
      int origin = _MaxValue;
      // 最大值不能为负
      _MaxValue = Mathf.Max(0, _MaxValue + maxValue);
      if (_MaxValue > origin) {
        AddValue(_MaxValue - origin);
      } else if (_MaxValue < _Value) {
        AddValue(_MaxValue - _Value);
      }
      return _MaxValue - origin;
    }
  }
}