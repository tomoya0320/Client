using Cysharp.Threading.Tasks;
using Sirenix.OdinInspector;

namespace GameCore.BehaviorFuncs {
  [CreateNodeMenu("�ڵ�/��Ϊ/��λ/�޸ĵ�λ����")]
  public class AddUnitAttrib : ActionNode {
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

      unit.AddAttrib(AttribType, behavior.GetInt(AttribValueKey), AttribField);
      return UniTask.FromResult(NodeResult.True);
    }
  }
}