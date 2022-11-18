using Cysharp.Threading.Tasks;
using Sirenix.OdinInspector;

namespace GameCore.BehaviorFuncs {
  [CreateNodeMenu("�ڵ�/��Ϊ/��λ/���õ�λ����")]
  public class SetUnitAttrib : ActionNode {
    [LabelText("��λ")]
    public NodeParamKey UnitKey;
    [LabelText("��������")]
    public AttribType AttribType;
    [LabelText("�����ֶ�")]
    public AttribField AttribField;
    [LabelText("����ֵ")]
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