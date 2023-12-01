using Cysharp.Threading.Tasks;
using Sirenix.OdinInspector;
using UnityEngine;

namespace GameCore.MagicFuncs {
  [CreateAssetMenu(menuName = "模板/效果/加属性")]
  public class AddAttrib : MagicFuncBase {
    public override bool IgnoreOnEnd => false;
    [LabelText("目标属性类型")]
    public AttribType Type;
    [LabelText("当前/最大值")]
    public AttribField AttribField;
    [LabelText("基础值")]
    public int Value;
    [LabelText("附加值")]
    public AttribAdditive AttribAdditive;

    public override UniTask Run(Battle battle, Context context, MagicArgs args) {
      if (args.IsEnd) {
        if (!(context is BuffContext buffContext) || buffContext.AttribValue == 0) return UniTask.CompletedTask;
        args.Target.AddAttrib(Type, buffContext.AttribValue, AttribField);
        buffContext.AttribValue = 0;
      } else {
        int attribValue = Value + AttribAdditive.GetValue(args.Target);
        int realAttribValue = args.Target.AddAttrib(Type, attribValue, AttribField);
        if (realAttribValue != 0 && context is BuffContext buffContext) {
          buffContext.AttribValue = realAttribValue;
        }
      }
      return UniTask.CompletedTask;
    }
  }
}