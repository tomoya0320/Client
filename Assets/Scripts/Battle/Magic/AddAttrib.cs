using Sirenix.OdinInspector;
using UnityEngine;

namespace Battle.MagicFuncs {
  [CreateAssetMenu(menuName = "模板/效果/加属性")]
  public class AddAttrib : MagicTemplate {
    public override bool IgnoreOnEnd => false;
    [LabelText("目标属性类型")]
    public AttribType Type;
    [LabelText("是否作用于最大值")]
    public bool OnMaxValue;
    [LabelText("基础值")]
    public int Value;
    [LabelText("附加值")]
    public AttribAdditive AttribAdditive;

    public override void Run(BattleManager battleManager, Context context, MagicArgs args) {
      if (args.IsEnd) {
        if (context is BuffContext buffContext && buffContext.AttribValue != 0) {
          args.Target.AddAttrib(Type, buffContext.AttribValue, OnMaxValue);
          buffContext.AttribValue = 0;
        }
      } else {
        int attribValue = Value + AttribAdditive.GetValue(args.Target);
        int realAttribValue = args.Target.AddAttrib(Type, attribValue, OnMaxValue);
        if (realAttribValue != 0 && context is BuffContext buffContext) {
          buffContext.AttribValue = realAttribValue;
        }
      }
    }
  }
}