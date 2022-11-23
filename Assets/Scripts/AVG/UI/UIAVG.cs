using DG.Tweening;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace GameCore.UI {
  public class UIAVG : UIBase {
    private struct Option {
      public GameObject Root;
      public Text Content;
      public Button Ok;
    }

    [SerializeField]
    private Transform OptionNode;
    [SerializeField]
    private Text NameText;
    [SerializeField]
    private Text DialogueText;
    [SerializeField]
    private Button NextBtn;
    private AVG AVG;
    private List<Option> Options = new List<Option>();

    public override UIBase Init(params object[] args) {
      AVG = new AVG(this, args[0] as AVGGraph);

      NameText.text = string.Empty;
      DialogueText.text = string.Empty;
      NextBtn.onClick.AddListener(() => AVG.Run());
      foreach (Transform op in OptionNode) {
        Options.Add(new Option {
          Root = op.gameObject,
          Content = op.GetComponentInChildren<Text>(true),
          Ok = op.GetComponentInChildren<Button>(true)
        });
      }
      OptionNode.gameObject.SetActiveEx(false);

      return base.Init(args);
    }

    public override void OnDestroy() {
      base.OnDestroy();
      AVG?.Clear();
      AVG = null;
    }

    public Tween SetDialogue(string name, string dialogue, float fadeTime, bool setSpeedBased) {
      NameText.text = name;
      DialogueText.text = string.Empty;
      var tween = DialogueText.DOText(dialogue, fadeTime);
      if (setSpeedBased) {
        tween.SetSpeedBased();
      }
      return tween;
    }

    public void SetOptions(string[] options, Action<int> callback) {
      for (int i = 0; i < Options.Count; i++) {
        var op = Options[i];
        op.Content.text = string.Empty;
        op.Ok.onClick.RemoveAllListeners();
        op.Root.SetActiveEx(false);
        if (i < options.Length) {
          int index = i; // ±Õ°ü
          op.Content.text = options[i];
          op.Ok.onClick.AddListener(() => {
            OptionNode.gameObject.SetActiveEx(false);
            callback?.Invoke(index);
          });
          op.Root.SetActiveEx(true);
        }
      }

      OptionNode.gameObject.SetActiveEx(true);
    }
  }
}