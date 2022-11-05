using Cysharp.Threading.Tasks;
using Sirenix.OdinInspector;

namespace GameCore.BehaviorFuncs {
  [CreateNodeMenu("节点/行为/单位/设置单位属性")]
  public class SetUnitAttrib : ActionNode {
    [LabelText("单位")]
    public NodeParamKey UnitKey;
    [LabelText("属性类型")]
    public AttribType AttribType;
    [LabelText("属性字段")]
    public AttribField AttribField;
    [LabelText("属性值")]
    public NodeIntParam AttribValueKey;

    public override UniTask<NodeResult> Run(Behavior behavior, Context context) {
      var unit = behavior.GetUnit(UnitKey);
      if (unit == null) {
        return UniTask.FromResult(NodeResult.False);
      }

      unit.SetAttrib(AttribType, AttribField, behavior.GetInt(AttribValueKey));
      return UniTask.FromResult(NodeResult.True);
    }
  }
}