using UnityEngine;
using UnityEngine.UI;

namespace GameCore.UI {
  [ExecuteInEditMode]
  public class ImageWithText : Image {
    [SerializeField]
    private Text TextComponent;
    public string Text {
      get => TextComponent ? TextComponent.text : null;
      set {
        if (TextComponent) {
          TextComponent.text = value;
        }
      }
    }

    protected override void Awake() {
      base.Awake();

      TextComponent = GetComponentInChildren<Text>(true);
    }
  }
}