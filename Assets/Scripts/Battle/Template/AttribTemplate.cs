using System;
using UnityEngine;
using Sirenix.OdinInspector;
using System.Collections.Generic;

namespace Battle {
  [CreateAssetMenu(menuName = "模板/属性")]
  public class AttribTemplate : SerializedScriptableObject {
    [Serializable]
    public struct AttribTemplateItem {
      [LabelText("曲线")]
      public AnimationCurve Curve;
      [LabelText("最小值")]
      public int MinValue;
      [LabelText("最大值")]
      public int MaxValue;
    }

    [DictionaryDrawerSettings(KeyLabel = "属性类型", ValueLabel = "属性成长")]
    public Dictionary<AttribType, AttribTemplateItem> Attribs;

    public Attrib[] GetAttribs(int level, int maxLevel) {
      float p = Mathf.Clamp01((float)level / maxLevel);
      Attrib[] attribs = new Attrib[BattleConstant.ATTRIB_LENGTH];
      foreach (var kv in Attribs) {
        var type = kv.Key;
        var item = kv.Value;
        int value = (int)(item.Curve.Evaluate(p) * (item.MaxValue - item.MinValue) + item.MinValue);
        attribs[(int)type].Value = value;
        attribs[(int)type].MaxValue = value;
      }
      return attribs;
    }
  }
}