using UnityEngine;

namespace GameCore {
  public class UIUnit : MonoBehaviour {
    [SerializeField]
    private Animator Animator;
    [SerializeField]
    private GameObject SelectedGo;
    private int SelectedCount;
    [SerializeField]
    private ImageWithText UIHp;
    private Unit Unit;

    public UIUnit Init(Unit unit) {
      Unit = unit;
      Unit.AddAttribChangedCallback(AttribType.HP, OnHpChanged);
      SetUIHp();
      return this;
    }

    public void SetSelected(bool selected) {
      SelectedCount += selected ? 1 : -1;
      SelectedGo.SetActive(SelectedCount > 0);
    }

    public void PlayAnimation(string animation) => Animator?.Play(animation);

    private void OnHpChanged(int beforeValue, int beforeMaxValue) => SetUIHp();

    private void SetUIHp() {
      if (UIHp) {
        Attrib hpAttrib = Unit.Attribs[(int)AttribType.HP];
        UIHp.fillAmount = (float)hpAttrib.Value / hpAttrib.MaxValue;
        UIHp.Text = $"{hpAttrib.Value}";
      }
    }
  }
}