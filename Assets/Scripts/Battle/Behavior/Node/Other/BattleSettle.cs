using Cysharp.Threading.Tasks;
using Sirenix.OdinInspector;

namespace GameCore.BehaviorFuncs {
  [CreateNodeMenu("节点/行为/其他/战斗结算")]
  public class BattleSettle : ActionNode {
    [LabelText("是否胜利")]
    public bool IsWin;

    public override async UniTask<NodeResult> Run(Behavior behavior, Context context) {
      await behavior.Battle.Settle(IsWin);
      return NodeResult.True;
    }
  }
}