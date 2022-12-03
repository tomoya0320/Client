using UnityEngine;

namespace GameCore.UI {
  public class UIBuff : MonoBehaviour {
    [SerializeField]
    private ImageWithText ImageWithText;
    private Buff Buff;

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