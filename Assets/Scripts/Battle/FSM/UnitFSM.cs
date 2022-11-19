using Cysharp.Threading.Tasks;

namespace GameCore {
  public enum UnitState {
    BORN,
    IN_TURN,
    OUT_TURN,
    DEAD,
  }

  public class UnitBornState : State<Unit> {
    public UnitBornState(StateMachine<Unit> stateMachine) : base((int)UnitState.BORN, stateMachine) {
    }
  }

  public class UnitInTurnState : State<Unit> {
    public UnitInTurnState(StateMachine<Unit> stateMachine) : base((int)UnitState.IN_TURN, stateMachine) {
    }

    public async override UniTask OnEnter(State<Unit> lastState, Context context = null) {
      // 执行回合开始前的行为树
      await Owner.Battle.BehaviorManager.RunRoot(TickTime.ON_START_TURN, Owner, context);
      // Test
      int drawCardCount = BattleConstant.MAX_HAND_CARD_COUNT - Owner.BattleCardControl.GetCardCount(CardHeapType.HAND);
      if (drawCardCount > 0) {
        if (Owner.BattleCardControl.GetCardCount(CardHeapType.DRAW) < drawCardCount) {
          var discardCardList = TempList<Card>.Get();
          var uniTaskList = TempList<UniTask>.Get();
          Owner.BattleCardControl.GetCardList(CardHeapType.DISCARD, discardCardList);
          foreach (var card in discardCardList) {
            uniTaskList.Add(card.SetCardHeapType(CardHeapType.DRAW));
          }
          await UniTask.WhenAll(uniTaskList);
          TempList<Card>.Release(discardCardList);
          TempList<UniTask>.Release(uniTaskList);
        }
        var drawCardList = TempList<Card>.Get();
        Owner.BattleCardControl.GetCardList(CardHeapType.DRAW, drawCardList);
        MathUtil.FisherYatesShuffle(drawCardList);
        for (int i = 0; i < drawCardList.Count && i < drawCardCount; i++) {
          await drawCardList[i].SetCardHeapType(CardHeapType.HAND);
          await UniTask.Delay(200);
        }
        TempList<Card>.Release(drawCardList);
      }
    }
  }

  public class UnitOutTurnState : State<Unit> {
    public UnitOutTurnState(StateMachine<Unit> stateMachine) : base((int)UnitState.OUT_TURN, stateMachine) {
    }

    public async override UniTask OnEnter(State<Unit> lastState, Context context = null) {
      // 执行回合结束前的行为树
      await Owner.Battle.BehaviorManager.RunRoot(TickTime.ON_END_TURN, Owner, context);
      // Test
      var handCardList = TempList<Card>.Get();
      var uniTaskList = TempList<UniTask>.Get();
      Owner.BattleCardControl.GetCardList(CardHeapType.HAND, handCardList);
      foreach (var card in handCardList) {
        uniTaskList.Add(card.SetCardHeapType(CardHeapType.DISCARD));
      }
      await UniTask.WhenAll(uniTaskList);
      TempList<Card>.Release(handCardList);
      TempList<UniTask>.Release(uniTaskList);
    }
  }

  public class UnitDeadState : State<Unit> {
    public UnitDeadState(StateMachine<Unit> stateMachine) : base((int)UnitState.DEAD, stateMachine) {
    }
    
    public async override UniTask OnEnter(State<Unit> lastState, Context context = null) {
      Owner.UIUnit.PlayAnimation("Die");
      await UniTask.Delay((int)(Owner.DieAnimTime * BattleConstant.THOUSAND));
      Owner.Player.DeadUnitCount++;
      await Owner.Battle.BehaviorManager.RunRoot(TickTime.ON_UNIT_DEAD, Owner, context);
      Owner.Battle.UnitManager.OnUnitDie(Owner);
    }

    public override bool CheckLeave(State<Unit> nextState) => false;
  }

  public class UnitStateMachine : StateMachine<Unit> {
    public UnitStateMachine(Unit owner) : base(owner) {
      RegisterState(new UnitBornState(this));
      RegisterState(new UnitInTurnState(this));
      RegisterState(new UnitOutTurnState(this));
      RegisterState(new UnitDeadState(this));

      CurrentState = States[(int)UnitState.BORN];
      CurrentState.OnEnter(null);
    }

    public bool IsAlive => CurrentState.StateId != (int)UnitState.DEAD;
  }
}