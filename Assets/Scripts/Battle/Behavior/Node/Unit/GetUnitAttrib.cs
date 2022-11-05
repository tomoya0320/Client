using Cysharp.Threading.Tasks;
using Sirenix.OdinInspector;

namespace GameCore.BehaviorFuncs {
  [CreateNodeMenu("�ڵ�/��Ϊ/��λ/��ȡ��λ����")]
  public class GetUnitAttrib : ActionNode {
    [LabelText("��λ")]
    public NodeParamKey UnitKey;
    [LabelText("��������")]
    public AttribType AttribType;
    [LabelText("�����ֶ�")]
    public AttribField AttribField;
    [LabelText("���Դ�ֵ")]
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