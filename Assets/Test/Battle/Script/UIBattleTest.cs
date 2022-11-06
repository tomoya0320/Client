using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

namespace GameCore {
  public class UIBattleTest : MonoBehaviour {
    private Battle Battle;
    public Button FinishTurnButton;
    public Button DrawButton;
    public Button DiscardButton;
    public Button ConsumeButton;
    public Text TurnText;
    public Text PlayCardCountText;
    public Text PlayerMasterText;
    public InputField TargetIdText;
    public GameObject EnemyTemplate;
    public GameObject CardTemplate;
    private Dictionary<Unit, Text> UnitInfoDict = new Dictionary<Unit, Text>();
    private Dictionary<Card, GameObject> CardDict = new Dictionary<Card, GameObject>();

    public void InitBattle(Battle battle) {
      Battle = battle;

      foreach (var unit in Battle.UnitManager.AllUnits) {
        if (unit.Player == Battle.SelfPlayer) {
          continue;
        }

        var go = Instantiate(EnemyTemplate, EnemyTemplate.transform.parent);
        go.SetActive(true);
        UnitInfoDict.Add(unit, go.GetComponentInChildren<Text>());
      }

      foreach (var card in Battle.SelfPlayer.Master.BattleCardControl[CardHeapType.DRAW]) {
        var go = Instantiate(CardTemplate, CardTemplate.transform.parent);
        go.SetActive(true);
        go.GetComponentInChildren<Text>().text = card.ToString();
        go.GetComponentInChildren<Button>().onClick.AddListener(() => {
          int.TryParse(TargetIdText.text, out int targetId);
          Unit target = Battle.UnitManager.GetUnit(targetId);
          if (target != null && Battle.SelfPlayer.Master.BattleCardControl.PlayCard(card, target)) {
            go.SetActive(false);
          }
        });
        CardDict.Add(card, go);
      }
    }

    private void Start() {
      FinishTurnButton?.onClick.AddListener(() => {
        if(Battle.CurPlayer == Battle.SelfPlayer) {
          var op = new EndTurnOp();
          op.Unit = Battle.CurPlayer.Master;
          Battle.CurPlayer.AddOperation(op);
        }
      });
      DrawButton?.onClick.AddListener(() => LogCards(CardHeapType.DRAW));
      DiscardButton?.onClick.AddListener(() => LogCards(CardHeapType.DISCARD));
      ConsumeButton?.onClick.AddListener(() => LogCards(CardHeapType.CONSUME));
    }

    private void Update() {
      if (Battle == null) {
        return;
      }

      if (TurnText) {
        TurnText.text = Battle.Turn.ToString();
      }
      if (PlayCardCountText) {
        PlayCardCountText.text = Battle.SelfPlayer.Master.BattleCardControl.PlayCardCount.ToString();
      }
      if (PlayerMasterText) {
        PlayerMasterText.text = GetUnitInfo(Battle.SelfPlayer.Master);
      }
      foreach (var kv in UnitInfoDict) {
        kv.Value.text = GetUnitInfo(kv.Key);
      }
      foreach (var kv in CardDict) {
        kv.Value.SetActive(kv.Key.CardHeapType == CardHeapType.HAND);
      }
    }

    private string GetUnitInfo(Unit unit) {
      StringBuilder stringBuilder = new StringBuilder($"{unit.RuntimeId}:{unit.Name}\nLv:{unit.Lv}\nHp:{unit.Attribs[(int)AttribType.HP]}\nAtk:{unit.Attribs[(int)AttribType.ATK]}\nEnergy:{unit.Attribs[(int)AttribType.ENERGY]}");
      foreach (var buff in Battle.BuffManager[unit.RuntimeId].AllBuffs) {
        stringBuilder.Append($"\n{buff.RuntimeId}:{buff.BuffTemplate.name}-Duration:{buff.Turn}/{buff.BuffTemplate.TotalDuration}({buff.BuffTemplate.Delay})");
      }
      
      return stringBuilder.ToString();
    }

    private void LogCards(CardHeapType cardHeapType) {
      StringBuilder stringBuilder = new StringBuilder();
      foreach (var card in Battle.SelfPlayer.Master.BattleCardControl[cardHeapType]) {
        stringBuilder.Append(card);
      }
      Debug.Log(stringBuilder);
    }
  }
}