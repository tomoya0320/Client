using UnityEngine;

namespace GameCore {
  public class UIUnit : MonoBehaviour {
    private Unit Unit;
    [SerializeField]
    private Animator Animator;
    [SerializeField]
    private GameObject SelectedGo;
    private int SelectedCount;
    [SerializeField]
    private ImageWithText UIHp;
    public Transform NumNode;
    public Transform BattleTextNode;

    public UIUnit Init(Unit unit) {
      Unit = unit;
      Unit.AddAttribChangedCallback(AttribType.HP, OnHpChanged);
      SetUIHp();
      UpdateSelectedGo();
      return this;
    }

    public void SetSelected(bool selected) {
      SelectedCount += selected ? 1 : -1;
      UpdateSelectedGo();
    }

    private void UpdateSelectedGo() {
      if (SelectedGo) {
        SelectedGo.SetActive(SelectedCount > 0);
      }
    }

    public void PlayAnimation(string animation) {
      if (Animator) {
        Animator.Play(animation);
      }
    }

    private void OnHpChanged(int beforeValue, int beforeMaxValue) => SetUIHp();

    private void SetUIHp() {
      if (UIHp) {
        Attrib hpAttrib = Unit.Attribs[(int)AttribType.HP];
        UIHp.fillAmount = (float)hpAttrib.Value / hpAttrib.MaxValue;
        UIHp.Text = $"{hpAttrib.Value}/{hpAttrib.MaxValue}";
      }
    }
  }
}