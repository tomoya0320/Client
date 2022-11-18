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
          foreach (var card in Owner.BattleCardControl[CardHeapType.DISCARD]) {
            card.CardHeapType = CardHeapType.DRAW;
          }
        }
        var drawCardList = Owner.BattleCardControl[CardHeapType.DRAW];
        MathUtil.FisherYatesShuffle(drawCardList);
        for (int i = 0; i < drawCardList.Count && i < drawCardCount; i++) {
          var card = drawCardList[i];
          card.CardHeapType = CardHeapType.HAND;
        }
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
      foreach (var card in Owner.BattleCardControl[CardHeapType.HAND]) {
        card.CardHeapType = CardHeapType.DISCARD;
      }
    }
  }

  public class UnitDeadState : State<Unit> {
    public UnitDeadState(StateMachine<Unit> stateMachine) : base((int)UnitState.DEAD, stateMachine) {
    }
    
    public async override UniTask OnEnter(State<Unit> lastState, Context context = null) {
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