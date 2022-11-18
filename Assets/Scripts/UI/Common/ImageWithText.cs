using UnityEngine;
using UnityEngine.UI;

namespace GameCore {
  [ExecuteInEditMode]
  public class ImageWithText : Image {
    [SerializeField]
    private Text TextComponent;
    public string Text {
      get => TextComponent.text;
      set => TextComponent.text = value;
    }

    protected override void Awake() {
      base.Awake();

      TextComponent = GetComponentInChildren<Text>(true);
    }
  }
}