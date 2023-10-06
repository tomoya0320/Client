using Cysharp.Threading.Tasks;
using Sirenix.OdinInspector;

namespace GameCore.BehaviorFuncs {
  [CreateNodeMenu("节点/行为/卡牌/随机改变卡牌堆类型")]
  public class RandomlyChangeCardHeapType : ActionNode {
    [LabelText("目标单位")]
    public NodeParamKey TargetUnit;
    [LabelText("原类型")]
    public CardHeapType SourceType;
    [LabelText("目标类型")]
    public CardHeapType TargetType;
    [LabelText("修改数量")]
    public NodeIntParam CountKey;

    public override async UniTask<NodeResult> Run(Behavior behavior, Context context) {
      var targetUnit = behavior.GetUnit(TargetUnit);
      if (targetUnit == null) {
        return NodeResult.False;
      }
      var cardList = TempList<Card>.Get();
      targetUnit.BattleCardControl.GetCardList(SourceType, cardList);
      MathUtil.FisherYatesShuffle(cardList);
      int count = behavior.GetInt(CountKey);
      for (int i = 0; i < count && i < cardList.Count; i++) {
        await cardList[i].SetCardHeapType(TargetType);
        await UniTask.Delay(100, cancellationToken: behavior.Battle.CancellationToken);
      }
      TempList<Card>.Release(cardList);
      return NodeResult.True;
    }
  }
}