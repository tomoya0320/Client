using Cysharp.Threading.Tasks;
using Sirenix.OdinInspector;

namespace GameCore.BehaviorFuncs {
  [CreateNodeMenu("节点/行为/战斗结算")]
  public class BattleSettle : ActionNode {
    [LabelText("是否胜利")]
    public bool IsWin;

    public override async UniTask<bool> Run(Behavior behavior, Context context) {
      behavior.Battle.Settle(IsWin);
      await UniTask.FromCanceled();
      return true;
    }
  }
}