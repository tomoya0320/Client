using GameCore.UI;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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
    [SerializeField]
    private Text UIEnergy;
    public Transform NumNode;
    public Transform BattleTextNode;
    [SerializeField]
    private GameObject UIBuffTemplate;
    private Stack<UIBuff> UIBuffCache = new Stack<UIBuff>();

    public UIUnit Init(Unit unit) {
      Unit = unit;
      Unit.AddAttribChangedCallback(AttribType.HP, OnHpChanged);
      Unit.AddAttribChangedCallback(AttribType.ENERGY, OnEnergyChanged);
      SetUIHp();
      SetUIEnergy();
      UpdateSelectedGo();
      return this;
    }

    public UIBuff OnAddBuff(Buff buff) {
      if (!UIBuffCache.TryPop(out var uiBuff)) {
        uiBuff = Instantiate(UIBuffTemplate, UIBuffTemplate.transform.parent).GetComponent<UIBuff>();
      }
      return uiBuff.Init(buff);
    }

    public void OnRemoveBuff(UIBuff uiBuff) => UIBuffCache.Push(uiBuff.OnRemove());

    public void SetSelected(bool selected) {
      SelectedCount += selected ? 1 : -1;
      UpdateSelectedGo();
    }

    private void UpdateSelectedGo() {
      if (SelectedGo) {
        SelectedGo.SetActiveEx(SelectedCount > 0);
      }
    }

    public void PlayAnimation(string animation) {
      if (Animator) {
        Animator.Play(animation);
      }
    }

    private void OnHpChanged(int beforeValue, int beforeMaxValue) => SetUIHp();

    private void OnEnergyChanged(int beforeValue, int beforeMaxValue) => SetUIEnergy();

    private void SetUIHp() {
      if (!UIHp) return;
      Attrib hpAttrib = Unit.Attribs[(int)AttribType.HP];
      UIHp.fillAmount = (float)hpAttrib.Value / hpAttrib.MaxValue;
      UIHp.Text = $"{hpAttrib.Value}/{hpAttrib.MaxValue}";
    }

    private void SetUIEnergy() {
      if (!UIEnergy) return;
      Attrib energyAttrib = Unit.Attribs[(int)AttribType.ENERGY];
      UIEnergy.text = $"{energyAttrib.Value}/{energyAttrib.MaxValue}";
    }
  }
}