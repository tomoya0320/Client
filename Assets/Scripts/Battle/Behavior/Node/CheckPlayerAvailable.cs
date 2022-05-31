using Cysharp.Threading.Tasks;
using Sirenix.OdinInspector;

namespace GameCore.BehaviorFuncs {
  [CreateNodeMenu("�ڵ�/��Ϊ/�������Ƿ����")]
  public class CheckPlayerAvailable : ActionNode {
    [LabelText("��ҵ�λ")]
    public NodeParamKey UnitKey;

    public override UniTask<bool> Run(Behavior behavior, Context context) {
      var unit = behavior.GetUnit(UnitKey);
      return UniTask.FromResult(unit.Player.Available);
    }
  }
}