using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using Cysharp.Threading.Tasks;
using DG.Tweening;

namespace GameCore {

  #region StateMachine
  public enum UICardState {
    IN_DRAW,
    IN_HAND,
    IN_DISCARD,
    IN_CONSUME,
    DRAGGING,
    PLAYING,
    SETTLE,
  }

  public abstract class UICardOutHand : State<UICard> {
    protected abstract Transform CardHeapNode { get; }

    protected UICardOutHand(int stateId, StateMachine<UICard> stateMachine) : base(stateId, stateMachine) { }

    public override async UniTask OnEnter(State<UICard> lastState, Context context = null) {
      if (lastState == null) {
        var transform = Owner.transform;
        transform.position = CardHeapNode.position;
        transform.localScale = UICardStateMachine.OUT_HAND_SCALE * Vector3.one;
        Owner.gameObject.SetActiveEx(false);
        return;
      }

      Owner.transform.DOMove(CardHeapNode.position, UICardStateMachine.OUT_HAND_MOVE_TIME);
      Owner.transform.DOScale(UICardStateMachine.OUT_HAND_SCALE, UICardStateMachine.OUT_HAND_SCALE_TIME);
      int delay = (int)(GameConstant.THOUSAND * Mathf.Max(UICardStateMachine.OUT_HAND_MOVE_TIME, UICardStateMachine.OUT_HAND_SCALE_TIME));
      await UniTask.Delay(delay, cancellationToken: Owner.Battle.CancellationToken);

      Owner.gameObject.SetActiveEx(false);
    }

    public override UniTask OnExit(State<UICard> nextState, Context context = null) {
      if (nextState.StateId != (int)UICardState.SETTLE) {
        Owner.gameObject.SetActiveEx(true);
      }
      return base.OnExit(nextState, context);
    }
  }

  public class UICardInDraw : UICardOutHand {
    protected override Transform CardHeapNode => Owner.DrawNode;

    public UICardInDraw(StateMachine<UICard> stateMachine) : base((int)UICardState.IN_DRAW, stateMachine) { }
  }

  public class UICardInHand : StateWithUpdate<UICard> {
    private int InHandIndex;
    private int HandCardCount;
    private Vector2 Pos;

    public UICardInHand(StateMachine<UICard> stateMachine) : base((int)UICardState.IN_HAND, stateMachine) { }

    public override UniTask OnEnter(State<UICard> lastState, Context context = null) {
      InHandIndex = -1;
      HandCardCount = 0;
      return base.OnEnter(lastState, context);
    }

    protected override void Update() {
      if (InHandIndex != Owner.InHandIndex || HandCardCount != Owner.HandCardCount) {
        InHandIndex = Owner.InHandIndex;
        HandCardCount = Owner.HandCardCount;
        var originPos = Owner.HandCardPosRef - new Vector2(0.25f * (HandCardCount - 1) * Owner.Width, 0);
        Pos = originPos + InHandIndex * 0.5f * new Vector2(Owner.Width, 0);
      }

      if (Owner.transform.GetSiblingIndex() != InHandIndex) {
        Owner.transform.SetSiblingIndex(InHandIndex);
      }

      Owner.transform.localScale = Vector3.Lerp(Owner.transform.localScale, Vector3.one, UICardStateMachine.IN_HAND_SPEED);
      Owner.RectTransform.anchoredPosition = Vector2.Lerp(Owner.RectTransform.anchoredPosition, Pos, UICardStateMachine.IN_HAND_SPEED);
      Owner.transform.eulerAngles = Vector3.Slerp(Owner.transform.eulerAngles, Vector3.zero, UICardStateMachine.IN_HAND_SPEED);
    }
  }

  public class UICardInDiscard : UICardOutHand {
    protected override Transform CardHeapNode => Owner.DiscardNode;

    public UICardInDiscard(StateMachine<UICard> stateMachine) : base((int)UICardState.IN_DISCARD, stateMachine) { }
  }

  public class UICardInConsume : UICardOutHand {
    protected override Transform CardHeapNode => Owner.ConsumeNode;

    public UICardInConsume(StateMachine<UICard> stateMachine) : base((int)UICardState.IN_CONSUME, stateMachine) { }
  }

  public class UICardDragging : StateWithUpdate<UICard> {

    public UICardDragging(StateMachine<UICard> stateMachine) : base((int)UICardState.DRAGGING, stateMachine) { }

    public override bool CheckEnter(State<UICard> lastState) => lastState != null && lastState.StateId == (int)UICardState.IN_HAND;

    protected override void Update() {
      // 大小缩放
      Owner.transform.localScale = Vector3.Lerp(Owner.transform.localScale, Vector3.one * UICardStateMachine.DRAGGING_SCALE, UICardStateMachine.DRAGGING_SCALE_SPEED * Time.deltaTime);
      // 位置移动
      Owner.RectTransform.anchoredPosition = Owner.EventData.position - 0.5f * new Vector2(Screen.width, Screen.height);
      // 目标选中的逻辑层
      Unit beforeMainTarget = Owner.MainTarget;
      if (Owner.transform.position.y > 0 && Owner.Battle.BattleState == BattleState.RUN) {
        Owner.MainTarget = Owner.Card.Battle.UnitManager.GetNearestUnit(Owner.EventData.position, Owner.Card.TargetCamp, Owner.Card.Owner);
      } else {
        Owner.MainTarget = null;
      }
      // 目标选中的表现层
      if (beforeMainTarget != Owner.MainTarget) {
        beforeMainTarget?.UIUnit.SetSelected(false);
        Owner.MainTarget?.UIUnit.SetSelected(true);
      }
    }

    public override UniTask OnExit(State<UICard> nextState, Context context = null) {
      Owner.MainTarget?.UIUnit.SetSelected(false);
      Owner.MainTarget = null;
      return base.OnExit(nextState, context);
    }
  }

  public class UICardPlaying : State<UICard> {
    public UICardPlaying(StateMachine<UICard> stateMachine) : base((int)UICardState.PLAYING, stateMachine) { }

    public override bool CheckEnter(State<UICard> lastState) => lastState != null && lastState.StateId == (int)UICardState.DRAGGING;
  }

  public class UICardSettle : State<UICard> {
    public UICardSettle(StateMachine<UICard> stateMachine) : base((int)UICardState.SETTLE, stateMachine) { }

    public override bool CheckLeave(State<UICard> nextState) => false;
  }

  public class UICardStateMachine : StateMachine<UICard> {
    public const float OUT_HAND_SCALE = 0.1f;
    public const float OUT_HAND_SCALE_TIME = 0.1f;
    public const float OUT_HAND_MOVE_TIME = 0.6f;
    public const float DRAGGING_SCALE = 1.2f;
    public const float DRAGGING_SCALE_SPEED = 12.0f;
    public const float IN_HAND_SPEED = 0.1f;

    public UICardStateMachine(UICard owner) : base(owner) {
      RegisterState(new UICardInDraw(this));
      RegisterState(new UICardInHand(this));
      RegisterState(new UICardInDiscard(this));
      RegisterState(new UICardInConsume(this));
      RegisterState(new UICardDragging(this));
      RegisterState(new UICardPlaying(this));
      RegisterState(new UICardSettle(this));

      CurrentState = States[(int)UICardState.IN_DRAW];
      CurrentState.OnEnter(null);
    }
  }
  #endregion

  public class UICard : MonoBehaviour, IPointerDownHandler, IPointerUpHandler {
    [SerializeField]
    private Text LvText;
    [SerializeField]
    private Text CostText;
    [SerializeField]
    private Text DescText;
    [SerializeField]
    private Text NameText;
    [SerializeField]
    private Text TypeText;
    [SerializeField]
    private Image IconImage;
    public int InHandIndex;
    public int HandCardCount;
    public Card Card { get; private set; }
    public Battle Battle => Card.Battle;
    public Transform DrawNode => Battle.UIBattle.DrawNode;
    public Transform DiscardNode => Battle.UIBattle.DiscardNode;
    public Transform ConsumeNode => Battle.UIBattle.ConsumeNode;
    public Vector2 HandCardPosRef => Battle.UIBattle.HandCardPosRefNode.anchoredPosition - 0.5f * new Vector2(0, Screen.height);
    public UICardStateMachine UICardStateMachine { get; private set; }
    public PointerEventData EventData { get; private set; }
    private RectTransform _RectTransform;
    public RectTransform RectTransform => _RectTransform ? _RectTransform : _RectTransform = GetComponent<RectTransform>();
    public float Width => RectTransform.rect.width;
    public Unit MainTarget;

    public void Init(Card card) {
      Card = card;
      UICardStateMachine = new UICardStateMachine(this);
      // 初始化UI
      LvText.text = $"{Card.Lv + 1}";
      CostText.text = Card.Cost >= 0 ? Card.Cost.ToString() : "X";
      DescText.text = Card.Desc;
      NameText.text = Card.Name;
      TypeText.text = Card.CardType.GetDescription();
      IconImage.sprite = Card.CardTemplate.Icon?.Asset as Sprite;
    }

    public async void OnPointerDown(PointerEventData eventData) {
      EventData = eventData;
      if (Battle.BattleState == BattleState.RUN && UICardStateMachine.CurrentState.StateId == (int)UICardState.IN_HAND) {
        await UICardStateMachine.SwitchState((int)UICardState.DRAGGING);
      }
    }

    public async void OnPointerUp(PointerEventData eventData) {
      if (Battle.BattleState == BattleState.RUN && UICardStateMachine.CurrentState.StateId == (int)UICardState.DRAGGING) {
        if (Card.Owner.BattleCardControl.PlayCard(Card, MainTarget)) {
          await UICardStateMachine.SwitchState((int)UICardState.PLAYING);
        } else {
          await UICardStateMachine.SwitchState((int)UICardState.IN_HAND);
        }
      }
      EventData = null;
    }
  }
}