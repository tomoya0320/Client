using Cysharp.Threading.Tasks;
using Sirenix.OdinInspector;

namespace GameCore.BehaviorFuncs {
  [CreateNodeMenu("�ڵ�/��Ϊ/ս������")]
  public class BattleSettle : ActionNode {
    [LabelText("�Ƿ�ʤ��")]
    public bool IsWin;

    public override async UniTask<bool> Run(Behavior behavior, Context context) {
      behavior.Battle.Settle(IsWin);
      await UniTask.FromCanceled();
      return true;
    }
  }
}