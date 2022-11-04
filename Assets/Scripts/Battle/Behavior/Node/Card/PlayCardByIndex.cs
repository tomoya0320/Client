using Cysharp.Threading.Tasks;
using Sirenix.OdinInspector;

namespace GameCore.BehaviorFuncs {
  [CreateNodeMenu("节点/行为/卡牌/通过索引出牌")]
  public class PlayCardByIndex : ActionNode {
    [LabelText("目标单位")]
    public NodeParamKey TargetUnit;
    [LabelText("出牌单位")]
    public NodeParamKey CardUnit;
    [LabelText("卡牌索引")]
    public NodeIntParam CardIndex;

    public override UniTask<NodeResult> Run(Behavior behavior, Context context) {
      var targetUnit = behavior.GetUnit(TargetUnit);
      var cardUnit = behavior.GetUnit(CardUnit);
      var cardIndex = behavior.GetInt(CardIndex);
      if (targetUnit == null || cardUnit == null) {
        return UniTask.FromResult(NodeResult.False);
      }
      var cardList = cardUnit.BattleCardControl[CardHeapType.HAND];
      if(cardIndex < 0 || cardIndex >= cardList.Count) {
        return UniTask.FromResult(NodeResult.False);
      }
      return UniTask.FromResult(BoolToNodeResult(cardUnit.PlayCard(cardList[cardIndex], targetUnit)));
    }
  }
}