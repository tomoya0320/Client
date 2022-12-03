using UnityEngine;
using UnityEngine.UI;

namespace GameCore.UI {
  public class UIBuff : MonoBehaviour {
    [SerializeField]
    private ImageWithText ImageWithText;
    private Buff Buff;

    private void Awake() {
      GetComponent<Button>().onClick.AddListener(() => {
        if (Buff != null) {
          Buff.Target.Battle.UIBattle.ShowPermanentText(Buff.BuffTemplate.Desc);
        }
      });
    }

    public UIBuff Init(Buff buff) {
      Buff = buff;
      ImageWithText.sprite = Buff.BuffTemplate.Icon?.Asset as Sprite;
      OnUpdate();
      transform.SetAsLastSibling();
      gameObject.SetActiveEx(true);
      return this;
    }

    public void OnUpdate() {
      ImageWithText.Text = Buff.GetLeftTurnText();
    }

    public UIBuff OnRemove() {
      Buff = null;
      ImageWithText.Clear();
      gameObject.SetActiveEx(false);
      return this;
    }
  }
}