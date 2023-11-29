using Cysharp.Threading.Tasks;
using Sirenix.OdinInspector;

namespace GameCore.BehaviorFuncs {
  [CreateNodeMenu("�ڵ�/��Ϊ/����/����ı俨�ƶ�����")]
  public class RandomlyChangeCardHeapType : ActionNode {
    [LabelText("Ŀ�굥λ")]
    public NodeParamKey TargetUnit;
    [LabelText("ԭ����")]
    public CardHeapType SourceType;
    [LabelText("Ŀ������")]
    public CardHeapType TargetType;
    [LabelText("�޸�����")]
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