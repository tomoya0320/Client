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

    public async override UniTask OnEnter(int lastStateId, Context context = null) {
      Owner.RefreshEnergy();

      // 执行回合开始前的行为树
      await Owner.Battle.BehaviorManager.RunRoot(TickTime.ON_START_TURN, Owner);

      // Test
      var handCardList = Owner.BattleCardControl[CardHeapType.HAND];
      int drawCardCount = BattleConstant.MAX_HAND_CARD_COUNT - handCardList.Count;
      if (drawCardCount > 0) {
        var drawCardList = Owner.BattleCardControl[CardHeapType.DRAW];
        if (drawCardList.Count < drawCardCount) {
          var discardCardList = Owner.BattleCardControl[CardHeapType.DISCARD];
          for (int i = 0; i < discardCardList.Count; i++) {
            var card = discardCardList[i];
            drawCardList.Add(card);
            card.CardHeapType = CardHeapType.DRAW;
          }
          Owner.BattleCardControl.RefreshCardList(CardHeapType.DISCARD);
        }
        MathUtil.FisherYatesShuffle(drawCardList);
        for (int i = 0; i < drawCardList.Count && i < drawCardCount; i++) {
          var card = drawCardList[i];
          handCardList.Add(card);
          card.CardHeapType = CardHeapType.HAND;
        }
        Owner.BattleCardControl.RefreshCardList(CardHeapType.DRAW);
      }
    }
  }

  public class UnitOutTurnState : State<Unit> {
    public UnitOutTurnState(StateMachine<Unit> stateMachine) : base((int)UnitState.OUT_TURN, stateMachine) {
    }

    public async override UniTask OnEnter(int lastStateId, Context context = null) {
      // 执行回合结束前的行为树
      await Owner.Battle.BehaviorManager.RunRoot(TickTime.ON_END_TURN, Owner);

      // Test
      var handCardList = Owner.BattleCardControl[CardHeapType.HAND];
      var discardCardList = Owner.BattleCardControl[CardHeapType.DISCARD];
      for (int i = 0; i < handCardList.Count; i++) {
        var card = handCardList[i];
        discardCardList.Add(card);
        card.CardHeapType = CardHeapType.DISCARD;
      }
      Owner.BattleCardControl.RefreshCardList(CardHeapType.HAND);
    }
  }

  public class UnitDeadState : State<Unit> {
    public UnitDeadState(StateMachine<Unit> stateMachine) : base((int)UnitState.DEAD, stateMachine) {
    }
    
    public async override UniTask OnEnter(int lastStateId, Context context = null) {
      Owner.Player.DeadUnitCount++;
      await Owner.Battle.BehaviorManager.RunRoot(TickTime.ON_UNIT_DEAD, Owner, context);
      Owner.Battle.UnitManager.OnUnitDie(Owner);
    }

    public override bool CheckLeave(int nextStateId) => false;
  }

  public class UnitStateMachine : StateMachine<Unit> {
    public UnitStateMachine(Unit owner) : base(owner) {
      RegisterState(new UnitBornState(this));
      RegisterState(new UnitInTurnState(this));
      RegisterState(new UnitOutTurnState(this));
      RegisterState(new UnitDeadState(this));

      CurrentState = States[(int)UnitState.BORN];
      CurrentState.OnEnter(-1);
    }

    public bool IsAlive => CurrentState.StateId != (int)UnitState.DEAD;
  }
}