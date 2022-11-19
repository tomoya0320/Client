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
  }

  public abstract class UICardOutHand : State<UICard> {
    protected abstract Transform CardHeapNode { get; }

    protected UICardOutHand(int stateId, StateMachine<UICard> stateMachine) : base(stateId, stateMachine) { }

    public async override UniTask OnEnter(State<UICard> lastState, Context context = null) {
      if (lastState == null) {
        Owner.transform.position = CardHeapNode.position;
        Owner.transform.localScale = Vector3.one * UICardStateMachine.OUT_HAND_SCALE;
        Owner.gameObject.SetActive(false);
        return;
      }

      await UniTask.WhenAll(Owner.transform.DOMove(CardHeapNode.position, UICardStateMachine.OUT_HAND_MOVE_TIME).AwaitForComplete(),
                            Owner.transform.DOScale(UICardStateMachine.OUT_HAND_SCALE, UICardStateMachine.OUT_HAND_SCALE_TIME).AwaitForComplete());
      Owner.gameObject.SetActive(false);
    }

    public override UniTask OnExit(State<UICard> nextState, Context context = null) {
      Owner.gameObject.SetActive(true);
      return base.OnExit(nextState, context);
    }
  }

  public class UICardInDraw : UICardOutHand {
    protected override Transform CardHeapNode => Owner.DrawNode;

    public UICardInDraw(StateMachine<UICard> stateMachine) : base((int)UICardState.IN_DRAW, stateMachine) { }
  }

  public class UICardInHand : State<UICard> {
    private int InHandIndex;
    private Vector2 Pos;

    public UICardInHand(StateMachine<UICard> stateMachine) : base((int)UICardState.IN_HAND, stateMachine) { }

    public override UniTask OnEnter(State<UICard> lastState, Context context = null) {
      InHandIndex = -1;
      return base.OnEnter(lastState, context);
    }

    public override void OnTick() {
      if (InHandIndex != Owner.InHandIndex) {
        InHandIndex = Owner.InHandIndex;
        Owner.transform.SetSiblingIndex(InHandIndex);
        Pos = Owner.InHandNode.anchoredPosition + InHandIndex * 0.5f * new Vector2(Owner.RectTransform.rect.width, 0);
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

  public class UICardDragging : State<UICard> {
    public UICardDragging(StateMachine<UICard> stateMachine) : base((int)UICardState.DRAGGING, stateMachine) { }

    public override bool CheckEnter(State<UICard> lastState) => lastState != null && lastState.StateId == (int)UICardState.IN_HAND;

    public override UniTask OnEnter(State<UICard> lastState, Context context = null) {
      Owner.transform.SetAsLastSibling();
      return base.OnEnter(lastState, context);
    }

    public override void OnTick() {
      // ��С����
      Owner.transform.localScale = Vector3.Lerp(Owner.transform.localScale, Vector3.one * UICardStateMachine.DRAGGING_SCALE, UICardStateMachine.DRAGGING_SCALE_SPEED * Time.deltaTime);
      // λ���ƶ�
      Owner.RectTransform.anchoredPosition = Owner.EventData.position - 0.5f * new Vector2(Screen.width, Screen.height);
      // Ŀ��ѡ�е��߼���
      Unit beforeMainTarget = Owner.MainTarget;
      if (Owner.transform.position.y > 0) {
        Owner.MainTarget = Owner.Card.Battle.UnitManager.GetNearestUnit(Owner.transform.position, Owner.Card.TargetCamp, Owner.Card.Owner);
      } else {
        Owner.MainTarget = null;
      }
      // Ŀ��ѡ�еı��ֲ�
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

  public class UICardStateMachine : StateMachine<UICard> {
    public const float OUT_HAND_SCALE = 0.1f;
    public const float OUT_HAND_SCALE_TIME = 0.1f;
    public const float OUT_HAND_MOVE_TIME = 0.6f;
    public const float DRAGGING_SCALE = 1.2f;
    public const float DRAGGING_SCALE_SPEED = 12.0f;
    public const float IN_HAND_SPEED = 0.02f;

    public UICardStateMachine(UICard owner) : base(owner) {
      RegisterState(new UICardInDraw(this));
      RegisterState(new UICardInHand(this));
      RegisterState(new UICardInDiscard(this));
      RegisterState(new UICardInConsume(this));
      RegisterState(new UICardDragging(this));
      RegisterState(new UICardPlaying(this));

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
    public Card Card { get; private set; }
    public Transform DrawNode => Card.Battle.UIBattle.DrawNode;
    public Transform DiscardNode => Card.Battle.UIBattle.DiscardNode;
    public Transform ConsumeNode => Card.Battle.UIBattle.ConsumeNode;
    public RectTransform InHandNode => Card.Battle.UIBattle.InHandNode;
    public UICardStateMachine UICardStateMachine { get; private set; }
    public PointerEventData EventData { get; private set; }
    public RectTransform RectTransform { get; private set; }
    public Unit MainTarget;

    private void Awake() {
      RectTransform = GetComponent<RectTransform>();
    }

    public void Init(Card card) {
      Card = card;
      UICardStateMachine = new UICardStateMachine(this);
      // ��ʼ��UI
      LvText.text = $"{Card.Lv + 1}";
      CostText.text = Card.Cost >= 0 ? Card.Cost.ToString() : "X";
      DescText.text = Card.Desc;
      NameText.text = Card.Name;
      TypeText.text = Card.CardType.GetDescription();
      if (Card.Battle.SpriteManager.TryGetAsset(Card.IconId, out var sprite)) {
        IconImage.sprite = sprite;
      }
    }

    private void Update() {
      if (Card.Battle.BattleState != BattleState.Run) {
        return;
      }

      UICardStateMachine.OnTick();
    }

    public void OnPointerDown(PointerEventData eventData) {
      if (UICardStateMachine.CurrentState.StateId == (int)UICardState.IN_HAND) {
        EventData = eventData;
        UICardStateMachine.SwitchState((int)UICardState.DRAGGING).Forget();
      }
    }

    public void OnPointerUp(PointerEventData eventData) {
      if (UICardStateMachine.CurrentState.StateId == (int)UICardState.DRAGGING) {
        if (Card.Owner.BattleCardControl.PlayCard(Card, MainTarget)) {
          UICardStateMachine.SwitchState((int)UICardState.PLAYING).Forget();
        } else {
          UICardStateMachine.SwitchState((int)UICardState.IN_HAND).Forget();
        }
      }

    }
  }
}