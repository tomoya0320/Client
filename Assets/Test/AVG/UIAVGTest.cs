using DG.Tweening;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace GameCore.UI {
  public class UIAVGTest : MonoBehaviour, IUIAVG {
    private struct Option {
      public GameObject Root;
      public Text Content;
      public Button Ok;
    }

    public AVGGraph AVGGraph;
    public Transform OptionTransform;
    public Text Name;
    public Text Dialogue;
    public Button Next;
    private AVG AVG;
    private List<Option> Options = new List<Option>();

    private void Awake() {
      AVG = new AVG();
      AVG.Init(this, AVGGraph);

      Name.text = string.Empty;
      Dialogue.text = string.Empty;
      Next.onClick.AddListener(() => AVG.Run());
      foreach (Transform op in OptionTransform) {
        Options.Add(new Option {
          Root = op.gameObject,
          Content = op.GetComponentInChildren<Text>(true),
          Ok = op.GetComponentInChildren<Button>(true)
        });
      }
      OptionTransform.gameObject.SetActive(false);
    }

    public Tween SetDialogue(string name, string dialogue, float fadeTime, bool setSpeedBased) {
      Name.text = name;
      Dialogue.text = string.Empty;
      var tween = Dialogue.DOText(dialogue, fadeTime);
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
        op.Root.SetActive(false);
        if (i < options.Length) {
          int index = i; // ±Õ°ü
          op.Content.text = options[i];
          op.Ok.onClick.AddListener(() => {
            OptionTransform.gameObject.SetActive(false);
            callback?.Invoke(index);
          });
          op.Root.SetActive(true);
        }
      }

      OptionTransform.gameObject.SetActive(true);
    }
  }
}