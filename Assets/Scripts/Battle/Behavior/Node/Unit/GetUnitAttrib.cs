using Cysharp.Threading.Tasks;
using Sirenix.OdinInspector;

namespace GameCore.BehaviorFuncs {
  [CreateNodeMenu("节点/行为/单位/获取单位属性")]
  public class GetUnitAttrib : ActionNode {
    [LabelText("单位")]
    public NodeParamKey UnitKey;
    [LabelText("属性类型")]
    public AttribType AttribType;
    [LabelText("属性字段")]
    public AttribField AttribField;
    [LabelText("属性存值")]
    public NodeParamKey AttribValueKey;

    public override UniTask<NodeResult> Run(Behavior behavior, Context context) {
      var unit = behavior.GetUnit(UnitKey);
      if (unit == null) {
        return UniTask.FromResult(NodeResult.False);
      }

      behavior.SetInt(AttribValueKey, unit.GetAttribField(AttribType, AttribField));
      return UniTask.FromResult(NodeResult.True);
    }
  }
}